// LLMEditorWindow.cs
// LLM에게 지시문을 보내고 결과를 받는 Unity 에디터 창 + 작업 유형 선택 기능 추가

using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

public class LLMEditorWindow : EditorWindow
{
    private enum TaskType
    {
        Objects,
        Script,
        Edit
    }

    private TaskType selectedTask = TaskType.Objects;
    private string userCommand = "플레이어가 점프하는 코드를 만들어줘.";
    private Vector2 scroll;
    private string lastPrompt = "";
    private string lastResponse = "";

    [MenuItem("LLM/Command Interface")]
    public static void ShowWindow()
    {
        GetWindow<LLMEditorWindow>("LLM Assistant");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("🧠 Unity LLM Assistant", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        selectedTask = (TaskType)EditorGUILayout.EnumPopup("작업 유형", selectedTask);

        EditorGUILayout.LabelField("명령어 입력:");
        userCommand = EditorGUILayout.TextArea(userCommand, GUILayout.Height(60));

        if (GUILayout.Button("LLM 실행하기"))
        {
            RunLLM();
        }

        EditorGUILayout.Space();
        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.LabelField("📤 Prompt", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(lastPrompt);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("📥 Response", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(lastResponse);

        EditorGUILayout.EndScrollView();
    }

    private async void RunLLM()
    {
        string prefix = selectedTask switch
        {
            TaskType.Objects => "[TASK:OBJECTS] ",
            TaskType.Script => "[TASK:SCRIPT] ",
            TaskType.Edit => "[TASK:EDIT] ",
            _ => ""
        };

        string prompt = LLMPromptBuilder.BuildPrompt(prefix + userCommand);
        lastPrompt = prompt;

        string response = await LLMRequester.SendPrompt(prompt);
        lastResponse = response;

        await LLMAgent.RunUserCommand(prefix + userCommand);
    }
}
