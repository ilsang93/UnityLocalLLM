using UnityEngine;

[CreateAssetMenu(fileName = "LLMConfig", menuName = "AI/LLM Config")]
public class LLMConfig : ScriptableObject
{
    public string endpointURL = "http://localhost:11434/api/generate";
    public string modelName = "codellama:7b";
    public bool stream = false;
}