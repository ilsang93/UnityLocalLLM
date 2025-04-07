using System.Collections.Generic;
using Newtonsoft.Json;

public class LLMContextBuilder
{
    public class LLMProjectContext
    {
        public List<ScriptAnalyzer.ScriptInfo> scripts = new();
        public List<PrefabSummarizer.PrefabInfo> prefabs = new();
        public List<SceneIndexer.SceneInfo> scenes = new();
    }

#if UNITY_EDITOR
    public static string BuildContextJson()
    {
        var root = ProjectScanner.ScanAssetsDirectory();

        var context = new LLMProjectContext
        {
            scripts = ScriptAnalyzer.AnalyzeAllScripts(root),
            prefabs = PrefabSummarizer.AnalyzeAllPrefabs(root),
            scenes = SceneIndexer.AnalyzeAllScenes(root)
        };

        return JsonConvert.SerializeObject(context, Formatting.Indented);
    }
#endif
}