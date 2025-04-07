using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class LLMJsonParser
{
    public static SceneNode[] Parse(string json)
    {
        try
        {
            // 👉 0. JSON 배열 직접 처리
            if (json.TrimStart().StartsWith("["))
            {
                return JsonConvert.DeserializeObject<SceneNode[]>(json);
            }

            // 👉 1. 기존 SceneRoot 구조
            if (json.Contains("\"objects\""))
            {
                var root = JsonConvert.DeserializeObject<SceneRoot>(json);
                return root.objects;
            }

            // 👉 2. wrapper 구조
            if (json.Contains("\"scene\"") && json.Contains("\"collection\""))
            {
                var root = JsonConvert.DeserializeObject<SceneSceneWrapper>(json);
                return root.scene.collection.objects;
            }

            // 👉 3. 간단한 레이아웃 구조
            if (json.Contains("\"player\"") && json.Contains("\"ground\""))
            {
                var temp = JsonConvert.DeserializeObject<SimpleSceneLayout>(json);
                var nodes = new List<SceneNode>();

                nodes.Add(new SceneNode
                {
                    type = temp.ground.type ?? "plane",
                    name = "Ground",
                    position = new Vector3Data(0, 0, 0),
                    scale = new Vector3Data(temp.size, 1, temp.ground.height)
                });

                var pos = temp.player.position.Length == 2
                    ? new Vector3Data(temp.player.position[0], temp.player.position[1], 0)
                    : new Vector3Data(temp.player.position[0], temp.player.position[1], temp.player.position[2]);

                nodes.Add(new SceneNode
                {
                    type = temp.player.type ?? "cube",
                    name = temp.player.color != null ? $"Player_{temp.player.color}" : "Player",
                    position = pos
                });

                return nodes.ToArray();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ JSON 파싱 실패: {e.Message}");
        }

        Debug.LogWarning("⚠ 예상한 구조를 찾지 못했습니다. 기본적으로 Cube 하나 생성합니다.");
        return new[] { new SceneNode { type = "cube", name = "DefaultCube" } };
    }
}