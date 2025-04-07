// SceneIndexer.cs
// 씬 정보 요약 수집기

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class SceneIndexer
{
    public class SceneInfo
    {
        public string path;
        public string name;
        public List<SceneObjectInfo> rootObjects = new();
    }

    public class SceneObjectInfo
    {
        public string name;
        public List<string> components = new();
    }

    public static List<SceneInfo> AnalyzeAllScenes(ProjectScanner.DirectoryEntry root)
    {
        var results = new List<SceneInfo>();

        foreach (var file in root.files)
        {
            if (file.name.EndsWith(".unity"))
            {
                var scene = AnalyzeScene(file.path);
                if (scene != null)
                    results.Add(scene);
            }
        }

        if (root.subdirectories != null)
        {
            foreach (var dir in root.subdirectories)
            {
                results.AddRange(AnalyzeAllScenes(dir));
            }
        }

        return results;
    }

    public static SceneInfo AnalyzeScene(string scenePath)
    {
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

        var info = new SceneInfo
        {
            path = scenePath,
            name = scene.name
        };

        foreach (var root in scene.GetRootGameObjects())
        {
            var objInfo = new SceneObjectInfo { name = root.name };
            foreach (var comp in root.GetComponents<Component>())
            {
                if (comp != null)
                    objInfo.components.Add(comp.GetType().Name);
            }
            info.rootObjects.Add(objInfo);
        }

        // ✅ 마지막 씬이 아닌 경우에만 닫기
        if (!scene.isLoaded || scene == EditorSceneManager.GetActiveScene())
        {
            Debug.LogWarning("⚠ 마지막 씬은 닫지 않고 유지합니다: " + scene.path);
        }
        else
        {
            EditorSceneManager.CloseScene(scene, true);
        }

        return info;
    }
}