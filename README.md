# AiChatTest

A simple WinForms app that uses OpenAI web API (text and image generation) to provide a fun interactive experience.
This is just a learning experience for using generative AI, so there is no deep game play here. My learning goals:
* Image generation - text to image used to generate the initial character images, in-painting (OpenAI calls it image edit) are used to dynamically update the character portrait.
* Text generation - use of system messages to set up role playing. Maintain the memory for the character to continue the conversation.
* Prompt engineering - use prompt engineering to ask LLM to expose character emotional state, so I could extract that to update character portrait.

# Setup
You need to modify WinFormsAiChat/Form1.cs to enter OpenAI API key for the app to run correctly.
The app supports using local Open API compatible servers. Modify `TextUseLocal` and `ImageUseLocal` constants in WinFormsAiChat/Form1.cs to have the code running against a local web server.
I hardcoded the host name to use with my [LocalGenAI](https://github.com/joe-chiu/LocalGenAI) framework, but it should work with other OpenAI compatible servers with minor modification.

# Game Play
* There are two different characters to choose from
* You are given a goal to accomplish on the UI. Try to probe the character for their hidden back stories, motivation and restrictions on helping you with your goal.
* For added fun, AI's emotion state is shown in the status bar and their portraigt would be updated to reflect that during the chat.
* I have not finish the part to recognize the goal has been accomplished, so it's honor system for now ;-)

<img width="569" alt="image" src="https://github.com/joe-chiu/AiChatTest/assets/14063642/5dbe7420-bb37-4354-a708-1078c4f9e0af">
