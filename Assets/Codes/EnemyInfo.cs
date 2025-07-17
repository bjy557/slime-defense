using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    public enum InfoType { Health, Damage }
    public InfoType type;

    private Text myText;
    private Spawner spawner;

    void Awake()
    {
        myText = GetComponent<Text>();
        spawner = FindAnyObjectByType<Spawner>();
    }

    void Update()
    {
        int wave = GameManager.instance.wave;

        if (wave < 0 || wave >= spawner.spawnData.Length)
        {
            myText.text = "N/A";
            return;
        }

        var data = spawner.spawnData[wave];

        switch (type)
        {
            case InfoType.Health:
                myText.text = $"{data.health:F0}";
                break;
            case InfoType.Damage:
                myText.text = $"{data.damage:F0}";
                break;
        }
    }
}
