using UnityEngine;

public static class LLMResponseInterpreter
{
    public static void Process(string response)
    {
        Debug.Log("📥 응답 해석 시작");
        bool handled = false;

        // 0. [OBJECTS] 태그 우선 처리
        if (response.Contains("[OBJECTS]") && response.Contains("[/OBJECTS]"))
        {
            string objectBlock = LLMUtils.ExtractBetween(response, "[OBJECTS]", "[/OBJECTS]");
            if (!string.IsNullOrWhiteSpace(objectBlock) && LLMUtils.IsValidJson(objectBlock))
            {
                LLMSceneExecutor.ExecuteActions(objectBlock);
                Debug.Log("✅ OBJECTS 처리 완료");
                handled = true;
            }
        }

        // 1. 코드 블록 추출
        var (fileName, code) = CodeWriter.ExtractCodeWithFileName(response);
        if (!string.IsNullOrWhiteSpace(code))
        {
            CodeWriter.Write(code, fileName);
            LLMSessionTracker.Record("...", response, fileName, code);
            LLMHistoryManager.AddEntry("...", response, fileName, code);
            Debug.Log("✅ 코드 처리 완료 (마크업)");
            handled = true;
        }
        else
        {
            // ✨ 태그 기반 코드 추출 (예: [SCRIPT] ... [/SCRIPT])
            string taggedCode = LLMUtils.ExtractTaggedBlock(response, "SCRIPT");
            if (!string.IsNullOrWhiteSpace(taggedCode))
            {
                string className = CodeWriter.ExtractClassName(taggedCode) ?? "GeneratedScript";
                CodeWriter.Write(taggedCode, className);
                LLMSessionTracker.Record("...", response, className, taggedCode);
                LLMHistoryManager.AddEntry("...", response, className, taggedCode);
                Debug.Log("✅ 코드 처리 완료 (태그)");
                handled = true;
            }
        }

        // 2. JSON 처리
        string json = LLMUtils.ExtractFirstJsonBlock(response);
        if (!string.IsNullOrWhiteSpace(json) && LLMUtils.IsValidJson(json))
        {
            LLMSceneExecutor.ExecuteActions(json);
            Debug.Log("✅ JSON 처리 완료");
            handled = true;
        }

        // 3. 힌트 출력
        if (!handled && response.Length < 600)
        {
            Debug.Log("📎 LLM 힌트: " + response.Trim());
        }

        if (!handled)
        {
            Debug.LogWarning("⚠ 처리할 수 있는 항목이 없습니다.");
        }
    }
}
