// Assets/Unity_LLM/Editor/HistoryViewerWindow.cs

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class HistoryViewerWindow : EditorWindow
{
    private Vector2 scroll;
    private List<LLMHistoryEntry> history;

    [MenuItem("LLM Tools/View LLM History")]
    public static void ShowWindow()
    {
        GetWindow<HistoryViewerWindow>("LLM History");
    }

    private void OnEnable()
    {
        history = LLMHistoryManager.LoadAll();
    }

    private void OnGUI()
    {
        if (history == null || history.Count == 0)
        {
            EditorGUILayout.HelpBox("LLM 히스토리가 없습니다.", MessageType.Info);
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int i = history.Count - 1; i >= 0; i--)
        {
            var entry = history[i];
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("🕓 " + entry.timestamp, EditorStyles.miniLabel);
            EditorGUILayout.LabelField("📥 Prompt:", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(entry.userPrompt, GUILayout.Height(40));

            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("📤 Response:", EditorStyles.boldLabel);
            if (!string.IsNullOrWhiteSpace(entry.code))
                EditorGUILayout.TextArea(entry.code, GUILayout.Height(100));
            else
                EditorGUILayout.TextArea(entry.response, GUILayout.Height(60));

            if (!string.IsNullOrWhiteSpace(entry.fileName))
                EditorGUILayout.LabelField("📄 File: " + entry.fileName + ".cs");

            if (GUILayout.Button("📋 복사하기"))
            {
                GUIUtility.systemCopyBuffer = entry.code ?? entry.response;
                Debug.Log("✅ 복사됨!");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        EditorGUILayout.EndScrollView();
    }
}
