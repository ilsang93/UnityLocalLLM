using UnityEditor;
using UnityEngine;

public class LLMCodeGenerator
{
    [MenuItem("LLM Test/Generate Script from Prompt")]
    public static async void RunPrompt()
    {
        var config = AssetDatabase.LoadAssetAtPath<LLMConfig>("Assets/Unity_LLM/Configs/LLMConfig.asset");
        LLMRequester.SetConfig(config);

        string prompt = "플레이어가 화살표 방향키로 움직일 수 있는 MonoBehaviour 스크립트를 만들어줘. 클래스 이름은 PlayerController야.";
        string response = await LLMRequester.SendPrompt(prompt);

        string code = LLMUtils.ExtractFirstCSharpCode(response);
        if (string.IsNullOrEmpty(code))
        {
            Debug.LogWarning("❌ 코드 블록을 찾지 못했어요.");
            return;
        }

        CodeWriter.WriteCodeToFile(code, "PlayerController");
    }
}