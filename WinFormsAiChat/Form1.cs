namespace WinFormsAiChat;

using Ckeisc.AiChatEngine;

public partial class Form1 : Form
{
    // TODO - put OpenAI API key here
    private const string OpenAiApiKey = "";
    private const bool TextUseLocal = false;
    private const bool ImageUseLocal = false;
    private AiChatEngine engine;

    private IEnumerable<string> Display
    {
        get
        {
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem is AiCharacter)
            {
                AiCharacter character = (AiCharacter)comboBox1.SelectedItem;
                return engine.GetDisplayChatHistory(character)
                    .Select(entry => $"{entry.Item1}: {entry.Item2}");
            }

            return new List<string>();
        }
    }

    public Form1()
    {
        InitializeComponent();

        this.engine = new AiChatEngine(
            OpenAiApiKey,
            textUseLocalApi: TextUseLocal, 
            imageUseLocalApi: ImageUseLocal);
        this.comboBox1.DataSource = this.engine.Characters;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        this.UpdateUI(initialRender: true);
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        await SendMessage();
    }

    private async void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await SendMessage();
        }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!this.IsHandleCreated)
        {
            return;
        }

        this.pictureBox1.ImageLocation = string.Empty;
        this.richTextBox1.Clear();
        this.textBox2.Clear();
        this.UpdateUI(initialRender: true);
    }

    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(textBox1.Text))
        {
            string message = textBox1.Text;
            textBox1.Clear();
            AiCharacter character = this.GetSelectedCharacter();
            await this.engine.SendMessage(character, message);
            this.UpdateUI(initialRender: false);
        }
    }

    private void UpdateUI(bool initialRender)
    {
        AiCharacter character = this.GetSelectedCharacter();
        textBox2.Text = character.UserGoal;
        toolStripStatusLabel1.Text = $"Emotion: {character.Emotion}";
        // when doing initial render, draw image from dll resources and do not go over network
        this.Invoke(() => this.UpdateImage(initialRender));

        foreach (string line in this.Display)
        {
            if (!richTextBox1.Text.Contains(line))
            {
                richTextBox1.AppendText(line + "\n\n");
            }
        }

        richTextBox1.ScrollToCaret();
    }

    private AiCharacter GetSelectedCharacter()
    {
        if (comboBox1.SelectedItem != null && comboBox1.SelectedItem is AiCharacter)
        {
            AiCharacter character = (AiCharacter)comboBox1.SelectedItem;
            return character;
        }
        throw new InvalidOperationException("unexpected character selection state");
    }

    private async void UpdateImage(bool useDefault)
    {
        AiCharacter character = this.GetSelectedCharacter();
        try
        {
            string? imagePath = await this.engine.GetImagePath(
                character, useDefault, useCache: true);
            if (File.Exists(imagePath))
            {
                pictureBox1.ImageLocation = imagePath;
            }
        }
        catch (HttpRequestException e)
        {
            MessageBox.Show($"Image generation backend failed: {e.Message}");
        }
    }
}
