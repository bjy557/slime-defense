#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

public class SpawnerDataLoader : MonoBehaviour
{
    [MenuItem("Tools/Load Spawner Data from CSV")]
    public static void LoadSpawnerData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Tier1");
        if (csvFile == null)
        {
            Debug.LogError("Tier1.csv 파일이 Resources 폴더에 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("CSV에 유효한 데이터가 없습니다.");
            return;
        }

        List<SpawnData> spawnDataList = new List<SpawnData>();

        for (int i = 1; i < Mathf.Min(lines.Length, 1001); i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Trim().Split(',');
            if (tokens.Length < 2) continue;

            if (TryParseAbbreviated(tokens[0], out int hp) &&
                TryParseAbbreviated(tokens[1], out int dmg))
            {
                SpawnData data = new SpawnData
                {
                    health = hp,
                    damage = dmg
                };
                spawnDataList.Add(data);
            }
        }

        Spawner spawner = Object.FindFirstObjectByType<Spawner>();
        if (spawner == null)
        {
            Debug.LogError("씬 내에 Spawner 오브젝트가 없습니다.");
            return;
        }

        Undo.RecordObject(spawner, "Spawner Data Updated");
        spawner.spawnData = spawnDataList.ToArray();
        EditorUtility.SetDirty(spawner);

        Debug.Log($"✅ {spawnDataList.Count}개의 SpawnData가 Spawner에 성공적으로 적용되었습니다.");
    }

    static readonly Dictionary<string, double> suffixMultiplier = new()
    {
        { "K", 1e3 }, { "M", 1e6 }, { "B", 1e9 }, { "T", 1e12 },
        { "Q", 1e15 }, { "s", 1e18 }, { "S", 1e21 },
        { "O", 1e24 }, { "N", 1e27 }, { "D", 1e30 }
    };

    static bool TryParseAbbreviated(string input, out int result)
    {
        input = input.Trim();

        // 단위 없는 경우 (그냥 숫자)
        if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double raw))
        {
            result = Mathf.FloorToInt((float)raw);
            return true;
        }

        // 단위 포함된 경우
        string numPart = input[..^1];
        string suffix = input[^1].ToString();

        if (suffixMultiplier.ContainsKey(suffix) &&
            double.TryParse(numPart, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
        {
            double expanded = val * suffixMultiplier[suffix];
            result = Mathf.FloorToInt((float)expanded);
            return true;
        }

        result = 0;
        return false;
    }
}
#endif
