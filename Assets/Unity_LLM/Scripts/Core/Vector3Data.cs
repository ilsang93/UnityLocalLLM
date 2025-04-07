using UnityEngine;

[System.Serializable]
public class Vector3Data
{
    public float x, y, z;

    public Vector3Data() { }
    public Vector3Data(float x, float y, float z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}
