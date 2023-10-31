namespace Ckeisc.AiChatEngine;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using Ckeisc.OpenAi;

public class AiChatEngine
{
    private const string NeutralEmotion = "neutral";
    private OpenAiClient client;

    public IList<AiCharacter> Characters
    {
        get;
        private set;
    }

    public AiChatEngine(
        string openAiApiKey, 
        bool textUseLocalApi = false, 
        bool imageUseLocalApi = false)
    {
        this.client = new OpenAiClient(openAiApiKey);
        this.client.TextUseLocalApi = textUseLocalApi;
        this.client.ImageUseLocalApi = imageUseLocalApi;
        this.Characters = new List<AiCharacter>();

        Assembly assembly = typeof(AiChatEngine).Assembly;

        JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        IEnumerable<string> allResources = assembly.GetManifestResourceNames();
        foreach(string res in allResources.Where(name => name.EndsWith(".json")))
        {
            string id = Regex.Match(res, "([^.]+).json").Groups[1].Value;

            using Stream stream = assembly.GetManifestResourceStream(res)!;
            using StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            CharacterData? data = JsonSerializer.Deserialize<CharacterData>(json, jsonOptions);

            if (data != null)
            {
                AiCharacter character = new AiCharacter(
                    data.Name, data.Appearance, data.UserGoal, data.Setup);
                this.Characters.Add(character);

                string? img = allResources.FirstOrDefault(item => item.EndsWith($"{id}.png"));
                string? mask = allResources.FirstOrDefault(item => item.EndsWith($"{id}_mask.png"));

                if (img != null)
                {
                    string path = this.WriteResourceToFile(assembly, img);
                    character.OriginalPicturePath = path;
                }

                if (mask != null)
                {
                    string path = this.WriteResourceToFile(assembly, mask);
                    character.OriginalPictureMaskPath = path;
                }

                // hardcode to neutral, can consider extract from conversation history
                character.Emotion = NeutralEmotion;
            }
        }
    }

    public async Task<ChatMessage> SendMessage(AiCharacter character, string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            ChatMessage myMessage = new ChatMessage()
            {
                Role = ChatRole.User,
                Content = message
            };
            character.ConversationHistory.Add(myMessage);
        }

        ChatCompletionResponse response = await this.client.CreateChat(character.ConversationHistory);
        ChatMessage aiResponse = response.Choices.First().Message;
        character.ConversationHistory.Add(aiResponse);
        character.Emotion = this.ExtratEmotion(aiResponse.Content);
        return aiResponse;
    }

    public async Task<string?> GetImagePath(AiCharacter character, bool useDefault, bool useCache)
    {
        string imagePath = Path.Combine(
            Path.GetTempPath(), $"{character.Name}_{DateTime.Now.ToString("MMddyyyy")}_{character.Emotion}.png");

        if (!useDefault && useCache && File.Exists(imagePath))
        {
            // cache hit, cache is day-stamped, so would refresh on different days
            return imagePath;
        }

        string prompt = character.Appearance + 
            $"The face shows {character.Emotion} expression.";

        ImageResponse? response = null;
        if (!useDefault && 
            character.OriginalPicturePath != null && 
            character.OriginalPictureMaskPath != null)
        {
            response = await this.client.EditImage(
                prompt,
                character.OriginalPicturePath,
                character.OriginalPictureMaskPath,
                ImageResponseFormat.Base64Json);
        }
        else if (useDefault && character.OriginalPicturePath != null)
        {
            // data written out from embedded resource
            imagePath = character.OriginalPicturePath;
        }
        else
        {
            // this is also used as a fallback that image is not present in the resources
            response = await this.client.CreateImage(
                prompt, ImageResponseFormat.Base64Json);
        }

        if (response != null)
        {
            string? base64String = response.Data.FirstOrDefault()?.Base64Json;
            if (base64String != null)
            {
                byte[] bytes = Convert.FromBase64String(base64String);
                File.WriteAllBytes(imagePath, bytes);
            }
        }

        return imagePath;
    }

    public List<Tuple<string, string>> GetDisplayChatHistory(AiCharacter character)
    {
        List<Tuple<string, string>> history = new();
        history = character.ConversationHistory
            .Where(msg => msg.Role != ChatRole.System)
            .Select(msg => this.FormatMessageFidDisplay(character, msg))
            .ToList();

        return history;
    }

    private string ExtratEmotion(string lastMessage)
    {
        int start = lastMessage.LastIndexOf('[');
        if (start > 0)
        {
            string substring = lastMessage.Substring(start);
            int end = substring.IndexOf(']');
            return substring.Substring(1, end - 1);
        }
        return NeutralEmotion;
    }

    private Tuple<string, string> FormatMessageFidDisplay(AiCharacter character, ChatMessage message)
    {
        Tuple<string, string> ret;
        if (message.Role == ChatRole.Assistant)
        {
            // filter out emotion state encoded in the assistant output
            string cleanedMessage = Regex.Replace(message.Content, @"\[(.+)\]", "");
            ret = new Tuple<string, string>(character.Name, cleanedMessage);
        }
        else
        {
            ret = new Tuple<string, string>("You", message.Content);
        }

        return ret;
    }

    private string WriteResourceToFile(Assembly assembly, string resourceId)
    {
        string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.png");
        using Stream srcStream = assembly.GetManifestResourceStream(resourceId)!;
        using FileStream destStream = File.Create(path);
        srcStream.CopyTo(destStream);
        return path;
    }
}

public class AiCharacter
{
    public string Name { get; private set; }

    public string Appearance { get; private set; }

    public string UserGoal { get; private set; }

    public string Emotion { get; internal set; } = "";

    // full conversation history, including system messages
    internal IList<ChatMessage> ConversationHistory { get; private set; }

    internal string? OriginalPicturePath { get; set; }

    internal string? OriginalPictureMaskPath { get; set; }

    internal AiCharacter(string name, string appearance, string userGoal, 
        IEnumerable<ChatMessage> initialSetup)
    {
        this.Name = name;
        this.Appearance = appearance;
        this.UserGoal = userGoal;
        this.ConversationHistory = new List<ChatMessage>(initialSetup);            
    }

    public override string ToString()
    {
        return this.Name;
    }
}

internal class CharacterData
{
    public required string Name { get; set; }

    public required string Appearance { get; set; }

    [JsonPropertyName("user_goal")]
    public required string UserGoal { get; set; }

    public required ChatMessage[] Setup { get; set; }
}
