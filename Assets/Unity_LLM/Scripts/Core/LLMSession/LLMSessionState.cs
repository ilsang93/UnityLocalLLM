using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LLMSessionState", menuName = "LLM/Session State")]
public class LLMSessionState : ScriptableObject
{
    [Serializable]
    public class LLMCommandRecord
    {
        public string prompt;
        public string response;
        public string fileName;
        public string code;
        public DateTime timestamp;
    }

    public List<LLMCommandRecord> commandHistory = new();
    public string lastGeneratedFile;

    public void AddRecord(string prompt, string response, string fileName, string code)
    {
        commandHistory.Add(new LLMCommandRecord
        {
            prompt = prompt,
            response = response,
            fileName = fileName,
            code = code,
            timestamp = DateTime.Now
        });

        lastGeneratedFile = fileName;
    }
}
