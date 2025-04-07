// Assets/Unity_LLM/Scripts/Core/LLMHistoryEntry.cs

[System.Serializable]
public class LLMHistoryEntry
{
    public string timestamp;
    public string userPrompt;
    public string response;
    public string fileName;
    public string code;
}
