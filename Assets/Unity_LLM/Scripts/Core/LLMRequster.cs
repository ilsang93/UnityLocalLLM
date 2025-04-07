using UnityEngine;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using UnityEditor;

[System.Serializable]
public class OllamaResponse
{
    public string response;
}

public class LLMRequester
{
    private static readonly HttpClient client = new HttpClient();
    private static LLMConfig config;

    public static void SetConfig(LLMConfig cfg)
    {
        config = cfg;
    }

    public static async Task<string> SendPrompt(string prompt)
    {
        if (config == null)
        {
            Debug.LogWarning("LLM Config not set. set default config.");
            SetConfig(AssetDatabase.LoadAssetAtPath<LLMConfig>("Assets/Unity_LLM/Configs/LLMConfig.asset"));
            return null;
        }
        var requestJson = new
        {
            model = config.modelName,          
            prompt = prompt,                   
            temperature = 0.7f,                
            top_p = 0.9f,                      
            stream = false                     
        };

        string json = JsonConvert.SerializeObject(requestJson);

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(config.endpointURL, content);
        string result = await response.Content.ReadAsStringAsync();

        // JSON 문자열을 Unity 내장 파서로 처리
        OllamaResponse parsed = JsonUtility.FromJson<OllamaResponse>(result);
        string clean = WebUtility.HtmlDecode(parsed.response);

        return clean;
    }
}
