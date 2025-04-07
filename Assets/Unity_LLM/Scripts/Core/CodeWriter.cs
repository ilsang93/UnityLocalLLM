using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

public static class CodeWriter
{
    private const string FolderPath = "Assets/Generated";

    public static void Write(string codeContent, string baseFileName = null, bool overwriteIfExists = false)
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
            AssetDatabase.Refresh();
        }

        if (string.IsNullOrEmpty(baseFileName))
        {
            baseFileName = ExtractClassName(codeContent) ?? "GeneratedScript";
        }

        string fileName = GetAvailableFileName(baseFileName, overwriteIfExists);
        string path = Path.Combine(FolderPath, fileName);

        File.WriteAllText(path, codeContent);
        Debug.Log((overwriteIfExists ? "♻ 덮어쓰기" : "✅ 새로 생성") + $"됨: {path}");

        AssetDatabase.ImportAsset(path);

        var scriptAsset = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
        if (scriptAsset != null)
        {
            Selection.activeObject = scriptAsset;
        }
        else
        {
            Debug.LogWarning("📁 스크립트는 생성되었지만 Selection 설정에 실패했습니다: " + path);
        }
    }

    public static void WriteCodeToFile(string code, string fileName, bool overwriteIfExists = false)
    {
        Write(code, fileName, overwriteIfExists);
    }

    public static (string fileName, string code) ExtractCodeWithFileName(string input)
    {
        // 파일명 + 코드 블록 형식 우선 탐색
        var fileHeader = Regex.Match(input, @"(?<file>\w+\.cs)\s*```(?:csharp|cs|code)?\s*(?<code>[\s\S]+?)\s*```", RegexOptions.IgnoreCase);
        if (fileHeader.Success)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileHeader.Groups["file"].Value);
            string code = fileHeader.Groups["code"].Value.Trim();
            return (fileName, code);
        }

        // 코드 블럭만 있는 경우
        string codeOnly = ExtractFirstCodeBlock(input);
        if (!string.IsNullOrWhiteSpace(codeOnly))
        {
            string className = ExtractClassName(codeOnly);
            return (className ?? "GeneratedScript", codeOnly);
        }

        // 마지막 fallback: 코드 패턴 감지하여 코드 본문만 추출
        var fallbackMatch = Regex.Match(input, @"class\s+\w+\s*:\s*\w+\s*{[\s\S]+}", RegexOptions.Multiline);
        if (fallbackMatch.Success)
        {
            string code = fallbackMatch.Value.Trim();
            string className = ExtractClassName(code);
            return (className ?? "GeneratedScript", code);
        }

        return (null, null);
    }

    private static string GetAvailableFileName(string baseName, bool overwriteIfExists)
    {
        if (overwriteIfExists)
            return $"{baseName}.cs";

        int index = 1;
        string fileName;

        do
        {
            fileName = $"{baseName}_{index}.cs";
            index++;
        } while (File.Exists(Path.Combine(FolderPath, fileName)));

        return fileName;
    }

    public static string ExtractClassName(string code)
    {
        var match = Regex.Match(code, @"class\s+(\w+)");
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        return null;
    }

    public static string ExtractFirstCodeBlock(string input)
    {
        var match = Regex.Match(input, "```(?:csharp|cs|code)?\\s*([\\s\\S]+?)\\s*```", RegexOptions.IgnoreCase);
        if (match.Success)
            return match.Groups[1].Value.Trim();
        return null;
    }
}
