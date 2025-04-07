// ScriptAnalyzer.cs
// C# 스크립트 파일을 분석하여 클래스 및 메서드 정보를 추출

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class ScriptAnalyzer
{
    public class ScriptInfo
    {
        public string fileName;
        public string className;
        public string baseClass;
        public List<string> methodNames = new();
    }

    public static ScriptInfo AnalyzeScript(string filePath)
    {
        if (!File.Exists(filePath)) return null;

        string code = File.ReadAllText(filePath);
        var info = new ScriptInfo
        {
            fileName = Path.GetFileName(filePath)
        };

        // 클래스 추출
        var classMatch = Regex.Match(code, @"class\s+(\w+)(\s*:\s*(\w+))?");
        if (classMatch.Success)
        {
            info.className = classMatch.Groups[1].Value;
            info.baseClass = classMatch.Groups[3].Value;
        }

        // 메서드 추출
        var methodMatches = Regex.Matches(code, @"\b(?:public|private|protected|internal)\s+(?:\w+[<>\w\[\]]*\s+)?(\w+)\s*\(");
        foreach (Match m in methodMatches)
        {
            string methodName = m.Groups[1].Value;
            if (!string.IsNullOrEmpty(methodName))
                info.methodNames.Add(methodName);
        }

        return info;
    }

    public static List<ScriptInfo> AnalyzeAllScripts(ProjectScanner.DirectoryEntry root)
    {
        var results = new List<ScriptInfo>();

        foreach (var file in root.files)
        {
            if (file.name.EndsWith(".cs"))
            {
                var result = AnalyzeScript(file.path);
                if (result != null) results.Add(result);
            }
        }

        foreach (var child in root.subdirectories)
        {
            results.AddRange(AnalyzeAllScripts(child));
        }

        return results;
    }
}