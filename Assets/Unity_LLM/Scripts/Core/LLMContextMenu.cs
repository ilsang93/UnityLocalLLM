using UnityEditor;
using UnityEngine;

public class LLMContextMenu
{
    [MenuItem("LLM/Generate Project Context JSON")]
    public static void GenerateContextJson()
    {
#if UNITY_EDITOR
        string contextJson = LLMContextBuilder.BuildContextJson();
        Debug.Log("📦 LLM Project Context:\n" + contextJson);
#endif
    }
}
