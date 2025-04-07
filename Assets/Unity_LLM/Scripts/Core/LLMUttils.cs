using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class LLMUtils
{
    public static string ExtractFirstCSharpCode(string response)
    {
        var match = Regex.Match(response, @"```(?:csharp|cs)?\s*(.*?)```", RegexOptions.Singleline);
        string rawCode = match.Success ? match.Groups[1].Value.Trim() : response.Trim();

        return CleanUpMalformedUsings(rawCode);
    }

    // 잘못된 using UnityEngine {...} 구조를 정리
    public static string CleanUpMalformedUsings(string code)
    {
        // "using UnityEngine {" → "using UnityEngine;\n"
        code = Regex.Replace(code, @"using\s+UnityEngine\s*\{", "using UnityEngine;\n");

        // 기타 잘못된 using
        code = Regex.Replace(code, @"using\s+([a-zA-Z0-9_.]+)\s*\{", "using $1;\n");

        return code;
    }

    public static string ExtractBetween(string input, string start, string end)
    {
        int startIndex = input.IndexOf(start);
        int endIndex = input.IndexOf(end);
        if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
            return null;

        startIndex += start.Length;
        return input.Substring(startIndex, endIndex - startIndex).Trim();
    }

    public static string ExtractFirstJsonBlock(string raw)
    {
        int firstBrace = raw.IndexOf('{');
        int lastBrace = raw.LastIndexOf('}');

        if (firstBrace == -1 || lastBrace == -1 || lastBrace <= firstBrace)
        {
            Debug.LogError("❌ JSON 블록을 찾을 수 없습니다.");
            return null;
        }

        return raw.Substring(firstBrace, lastBrace - firstBrace + 1);
    }
    public static string GetSystemInstruction()
    {
        return
            "format sample : \n" +
            "{\n" +
            "  \"objects\": [\n" +
            "    {\n" +
            "      \"type\": \"cube\",\n" +
            "      \"name\": \"Player\",\n" +
            "      \"position\": { \"x\": 0, \"y\": 1, \"z\": 0 },\n" +
            "      \"scale\": { \"x\": 1, \"y\": 1, \"z\": 1 }\n" +
            "    },\n" +
            "    {\n" +
            "      \"type\": \"plane\",\n" +
            "      \"name\": \"Floor\",\n" +
            "      \"position\": { \"x\": 0, \"y\": 0, \"z\": 0 },\n" +
            "      \"scale\": { \"x\": 10, \"y\": 1, \"z\": 10 }\n" +
            "    }\n" +
            "  ]\n" +
            "}";
    }

    public static bool IsValidJson(string json)
    {
        try
        {
            var token = JToken.Parse(json);
            return token.Type == JTokenType.Object || token.Type == JTokenType.Array;
        }
        catch
        {
            return false;
        }
    }

    public static string ExtractTaggedBlock(string input, string tag)
    {
        string openTag = $"[{tag}]";
        string closeTag = $"[/{tag}]";

        int start = input.IndexOf(openTag);
        int end = input.IndexOf(closeTag);

        if (start != -1 && end != -1 && end > start)
        {
            return input.Substring(start + openTag.Length, end - (start + openTag.Length)).Trim();
        }

        return null;
    }
}
