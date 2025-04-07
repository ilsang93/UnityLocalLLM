[System.Serializable]
public class SceneDefinition
{
    public string type;
    public LLMAction[] children;
}

[System.Serializable]
public class SceneNode
{
    public string type;
    public string name;

    public Vector3Data position;
    public Vector3Data rotation;
    public Vector3Data scale;
    public Vector3Data size;

    public SceneNode[] children;
}