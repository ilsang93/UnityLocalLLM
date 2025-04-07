using UnityEditor;
using UnityEngine;
using System;

public static class LLMSceneExecutor
{
    public static void ExecuteActions(string json)
    {
        try
        {
            SceneNode[] nodes = LLMJsonParser.Parse(json);
            foreach (var node in nodes)
            {
                InstantiateNodeRecursive(node, null);
            }

        }
        catch (Exception e)
        {
            Debug.LogError("❌ JSON 파싱 실패: " + e.Message);
        }
    }

    private static void InstantiateNodeRecursive(SceneNode node, Transform parent)
    {
        if (string.IsNullOrEmpty(node.type))
        {
            Debug.LogWarning("⚠ Primitive type이 정의되지 않았습니다. 기본값 Cube를 사용합니다.");
            node.type = "cube";
        }

        GameObject obj = GameObject.CreatePrimitive(ParsePrimitive(node.type));
        obj.name = string.IsNullOrEmpty(node.name) ? node.type : node.name;

        obj.transform.parent = parent;

        if (node.position != null) obj.transform.localPosition = node.position.ToVector3();
        if (node.rotation != null) obj.transform.localEulerAngles = node.rotation.ToVector3();
        if (node.size != null) obj.transform.localScale = node.size.ToVector3();
        else if (node.scale != null) obj.transform.localScale = node.scale.ToVector3();

        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
    }

    private static PrimitiveType ParsePrimitive(string type)
    {
        if (string.IsNullOrEmpty(type))
        {
            Debug.LogWarning("⚠️ Primitive type이 정의되지 않았습니다. 기본값 Cube를 사용합니다.");
            return PrimitiveType.Cube;
        }

        return type.ToLower() switch
        {
            "cube" => PrimitiveType.Cube,
            "plane" => PrimitiveType.Plane,
            "sphere" => PrimitiveType.Sphere,
            "capsule" => PrimitiveType.Capsule,
            "cylinder" => PrimitiveType.Cylinder,
            "quad" => PrimitiveType.Quad,
            _ => PrimitiveType.Cube
        };
    }
}
