[System.Serializable]
public class LLMActionList
{
    public LLMAction[] actions;
}

[System.Serializable]
public class LLMAction
{
    public string name;
    public string type;         // cube, plane 등
    public Vector3Data position;
    public Vector3Data rotation;
    public Vector3Data scale;
}