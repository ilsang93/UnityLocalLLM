public static class LLMSessionTracker
{
    private const string SessionAssetPath = "Assets/Unity_LLM/Configs/LLMSessionState.asset";

    public static LLMSessionState Load()
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<LLMSessionState>(SessionAssetPath);
#else
        return null;
#endif
    }

    public static void Record(string prompt, string response, string fileName, string code)
    {
        var session = Load();
        if (session != null)
        {
            session.AddRecord(prompt, response, fileName, code);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(session);
#endif
        }
    }
}
