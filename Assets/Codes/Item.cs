using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Text textName;
    Text textValue;
    Text textCost;

    private void Awake()
    {
        Text[] texts = GetComponentsInChildren<Text>();

        foreach (Text txt in texts)
        {
            if (txt.name == "ValueText")
                textValue = txt;
            else if (txt.name == "CostText")
                textCost = txt;
            else if (txt.name == "NameTitle")
                textName = txt;

        }
    }

    private void LateUpdate()
    {
        // 영어의 경우 너무 길어져서 줄바꿈 필요
        //textName.text = data.itemName.Replace(" ", "\n");
        textName.text = data.itemName;

        switch (data.itemType)
        {
            case ItemData.ItemType.CriticalChance:
            case ItemData.ItemType.Defense:
            case ItemData.ItemType.Reflection:
            case ItemData.ItemType.LifeSteal:
                textValue.text = data.values[level].ToString() + "%";
                break;
            case ItemData.ItemType.CriticalDamage:
            case ItemData.ItemType.GoldMultiplier:
            case ItemData.ItemType.CoinMultiplier:
                textValue.text = "x" + data.values[level].ToString();
                break;
            case ItemData.ItemType.AttackRange:
                textValue.text = data.values[level].ToString() + "m";
                break;
            case ItemData.ItemType.Regeneration:
                textValue.text = data.values[level].ToString() + "/sec";
                break;
            default:
                textValue.text = data.values[level].ToString();
                break;
        }

        textCost.text = data.costs[level].ToString();
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Attack:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextValue = data.baseDamage;
                    int nextCost = 0;

                    nextValue += data.baseDamage * data.values[level];
                    nextCost += data.costs[level];

                    weapon.LevelUp(nextValue, nextCost);
                }
                level++;
                break;
            case ItemData.ItemType.AttackSpeed:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    gear = newWeapon.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.values[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            default:
                break;
        }

        

        if (level == data.values.Length)
        {
            GetComponent<Button>().interactable = false;
        }

    }
}
