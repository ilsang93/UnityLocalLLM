[System.Serializable]
public class SceneRoot
{
    public SceneNode[] objects;
}

[System.Serializable]
public class SceneSceneWrapper
{
    public SceneCollection scene;
}

[System.Serializable]
public class SceneCollection
{
    public string name;
    public SceneRoot collection;
}

[System.Serializable]
public class SimpleSceneLayout
{
    public int size;
    public SimpleGround ground;
    public SimplePlayer player;
}

[System.Serializable]
public class SimpleGround
{
    public string type;
    public float height;
}

[System.Serializable]
public class SimplePlayer
{
    public string type;
    public float[] position;
    public string color;
}
