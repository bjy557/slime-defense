#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ItemDataBatchUpdater : MonoBehaviour
{
    [MenuItem("Tools/Batch Update ItemData from LevelData CSV")]
    public static void BatchUpdate()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("LevelDesign");
        if (csvFile == null)
        {
            Debug.LogError("LevelDesign.csv 파일이 Resources 폴더에 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("CSV에 유효한 데이터가 없습니다.");
            return;
        }

        string[] headers = lines[0].Trim().Split(',');
        int attributeCount = headers.Length / 3;

        Dictionary<string, (List<float> values, List<int> costs, List<int> perms)> dataMap = new();

        for (int i = 0; i < attributeCount; i++)
        {
            string attr = headers[i * 3].Trim();
            dataMap[attr] = (new List<float>(), new List<int>(), new List<int>());
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            string[] cells = lines[i].Trim().Split(',');

            for (int j = 0; j < attributeCount; j++)
            {
                string attr = headers[j * 3].Trim();
                var tuple = dataMap[attr];

                if (cells.Length > j * 3 && float.TryParse(cells[j * 3], out float val))
                    tuple.values.Add(val);
                if (cells.Length > j * 3 + 1 && int.TryParse(cells[j * 3 + 1], out int cost))
                    tuple.costs.Add(cost);
                if (cells.Length > j * 3 + 2 && int.TryParse(cells[j * 3 + 2], out int perm))
                    tuple.perms.Add(perm);

                dataMap[attr] = tuple;
            }
        }

        int updated = 0;
        foreach (var kvp in dataMap)
        {
            string name = kvp.Key;
            string path = $"Assets/Data/{name}.asset";

            ItemData data = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (data == null)
            {
                Debug.LogWarning($"⚠️ {name}.asset 파일을 찾을 수 없습니다.");
                continue;
            }

            data.values = kvp.Value.values.ToArray();
            data.costs = kvp.Value.costs.ToArray();
            data.permanents = kvp.Value.perms.ToArray();

            EditorUtility.SetDirty(data);
            updated++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ {updated}개의 ItemData 항목이 CSV로부터 성공적으로 업데이트되었습니다.");
    }
}
#endif
