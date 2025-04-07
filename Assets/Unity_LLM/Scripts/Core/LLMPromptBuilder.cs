using System.IO;

public static class LLMPromptBuilder
{
    public static string BuildPrompt(string userCommand)
    {
        if (userCommand.StartsWith("[TASK:OBJECTS]"))
        {
            string contextJson = LLMContextBuilder.BuildContextJson();
            return
$@"You are a Unity assistant generating Unity scene objects.

Respond only with a valid JSON array of GameObjects, and wrap the array with [OBJECTS] and [/OBJECTS] tags.
Do NOT include any explanations, descriptions, comments, or code formatting.
Each object must include type, name, position (as {{x,y,z}}), and optionally scale and rotation.

Output format (strict):
[OBJECTS]
[
  {{
    ""type"": ""cube"",
    ""name"": ""Player"",
    ""position"": {{ ""x"": 0, ""y"": 1, ""z"": 0 }},
    ""scale"": {{ ""x"": 1, ""y"": 1, ""z"": 1 }}
  }}
]
[/OBJECTS]

[CONTEXT]
{contextJson}

[USER REQUEST]
{userCommand.Replace("[TASK:OBJECTS]", "").Trim()}";
        }

        if (userCommand.StartsWith("[TASK:SCRIPT]"))
        {
            string contextJson = LLMContextBuilder.BuildContextJson();
            return
$@"You are a Unity C# script generator.
Respond only with valid code or [SCRIPT] ... [/SCRIPT] blocks.
No explanation, no extra markdown formatting.

[CONTEXT]
{contextJson}

[USER REQUEST]
{userCommand.Replace("[TASK:SCRIPT]", "").Trim()}";
        }

        if (userCommand.StartsWith("[TASK:EDIT]"))
        {
            var session = LLMSessionTracker.Load();
            if (session != null && !string.IsNullOrWhiteSpace(session.lastGeneratedFile))
            {
                string path = "Assets/Generated/" + session.lastGeneratedFile + ".cs";
                string code = File.Exists(path) ? File.ReadAllText(path) : "";
                return $@"[TASK:EDIT]
You are editing an existing Unity C# script.
Only return the modified script in valid C# format without explanation.

Current code of {session.lastGeneratedFile}.cs:
```csharp
{code}
[USER REQUEST] {userCommand}";
            }
        }

        // fallback
        return userCommand;
    }
}