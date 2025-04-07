// Assets/Unity_LLM/Scripts/Core/LLMHistoryManager.cs

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LLMHistoryManager
{
    private const string SavePath = "Assets/Unity_LLM/Configs/LLMHistory.json";

    public static List<LLMHistoryEntry> LoadAll()
    {
        if (!File.Exists(SavePath)) return new List<LLMHistoryEntry>();

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<LLMHistoryListWrapper>(json).entries;
    }

    public static void AddEntry(string prompt, string response, string fileName, string code)
    {
        var list = LoadAll();
        list.Add(new LLMHistoryEntry
        {
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            userPrompt = prompt,
            response = response,
            fileName = fileName,
            code = code
        });

        SaveAll(list);
    }

    public static void SaveAll(List<LLMHistoryEntry> entries)
    {
        var wrapper = new LLMHistoryListWrapper { entries = entries };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(SavePath, json);
    }

    [System.Serializable]
    private class LLMHistoryListWrapper
    {
        public List<LLMHistoryEntry> entries = new();
    }
}
