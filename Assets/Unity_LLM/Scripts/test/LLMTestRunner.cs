using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class LLMTestRunner
{
    [MenuItem("LLM Test/Run with Config")]
    public static async void RunWithConfig()
    {
        var config = AssetDatabase.LoadAssetAtPath<LLMConfig>("Assets/Unity_LLM/Configs/LLMConfig.asset");
        LLMRequester.SetConfig(config);

        string prompt = "유니티에서 방향키 입력으로 캐릭터를 움직이게 해줘.";
        await LLMRequester.SendPrompt(prompt);
    }

    [MenuItem("LLM Test/Run Object Spawn Prompt")]
    public static async void RunObjectSpawnPrompt()
    {
        var config = AssetDatabase.LoadAssetAtPath<LLMConfig>("Assets/Unity_LLM/Configs/LLMConfig.asset");
        LLMRequester.SetConfig(config);

        string systemPrompt = LLMUtils.GetSystemInstruction();
        string userPrompt = "5x5 사이즈의 큐브를 만들고 이름을 Enemy로 설정해. 위치는 원점에서 적당히 떨어진 곳으로. JSON 형식으로 알려줘. 다른 불필요한 설명은 없이 데이터만.";

        string finalPrompt = $"{systemPrompt}\n\n{userPrompt}";
        Debug.Log($"📦 Final Prompt: \n{finalPrompt}");

        string response = await LLMRequester.SendPrompt(finalPrompt);
        Debug.Log($"📦 Response: \n{response}");

        // ```json ... ``` 제거
        string jsonRaw = Regex.Replace(response, @"```json\s*([\s\S]+?)```", "$1").Trim();

        // 설명 제거
        jsonRaw = LLMUtils.ExtractFirstJsonBlock(jsonRaw);

        if (string.IsNullOrEmpty(jsonRaw))
        {
            Debug.LogError("❌ JSON 추출 실패");
            return;
        }

        Debug.Log($"📦 RAW JSON: \n{jsonRaw}");


        LLMSceneExecutor.ExecuteActions(jsonRaw);
    }
}
