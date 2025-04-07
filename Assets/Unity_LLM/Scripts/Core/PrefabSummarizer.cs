// PrefabSummarizer.cs
// Unity 프리팹 파일의 루트 오브젝트 및 컴포넌트 요약

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabSummarizer
{
    public class PrefabInfo
    {
        public string path;
        public string name;
        public List<string> components = new();
    }

#if UNITY_EDITOR
    public static PrefabInfo AnalyzePrefab(string assetPath)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if (prefab == null) return null;

        var info = new PrefabInfo
        {
            path = assetPath,
            name = prefab.name
        };

        var comps = prefab.GetComponents<Component>();
        foreach (var comp in comps)
        {
            if (comp != null)
                info.components.Add(comp.GetType().Name);
        }

        return info;
    }

    public static List<PrefabInfo> AnalyzeAllPrefabs(ProjectScanner.DirectoryEntry root)
    {
        var results = new List<PrefabInfo>();

        foreach (var file in root.files)
        {
            if (file.name.EndsWith(".prefab"))
            {
                string relativePath = file.path.Replace(Application.dataPath, "Assets");
                var result = AnalyzePrefab(relativePath);
                if (result != null) results.Add(result);
            }
        }

        foreach (var child in root.subdirectories)
        {
            results.AddRange(AnalyzeAllPrefabs(child));
        }

        return results;
    }
#endif
}