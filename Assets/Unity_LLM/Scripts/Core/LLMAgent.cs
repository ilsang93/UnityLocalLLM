// Assets/Unity_LLM/Scripts/Core/LLMAgent.cs

using System.Threading.Tasks;
using UnityEngine;

public static class LLMAgent
{
    public static async Task RunUserCommand(string prompt)
    {
        Debug.Log("📤 Sending prompt to LLM:\n" + prompt);

        string formattedPrompt = LLMPromptBuilder.BuildPrompt(prompt);
        string response = await LLMRequester.SendPrompt(formattedPrompt);

        if (string.IsNullOrWhiteSpace(response))
        {
            Debug.LogWarning("⚠ 응답이 비어 있습니다.");
            return;
        }

        Debug.Log("📥 Response from LLM:\n" + response);

        // ✅ 응답 해석 전담 클래스에게 위임
        LLMResponseInterpreter.Process(response);
    }
}