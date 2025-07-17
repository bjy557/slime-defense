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
        // ?????? ???? ???? ???????? ?????? ????
        //textName.text = data.itemName.Replace(" ", "\n");
        textName.text = data.itemName;

        switch (data.itemType)
        {
            case ItemData.ItemType.CriticalChance:
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
                textValue.text = data.values[level].ToString() + "/s";
                break;
            default:
                textValue.text = data.values[level].ToString();
                break;
        }

        textCost.text = data.costs[level].ToString();
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Attack:
                level++;

                if (weapon == null) return;
                float nextValue = data.baseDamage;

                nextValue = data.values[level];

                weapon.UpgradeDamage(nextValue);
                break;
            case ItemData.ItemType.AttackSpeed:
                level++;

                float nextRate = data.values[level];
                weapon.UpgradeSpeed(nextRate);
                break;
            case ItemData.ItemType.CriticalChance:
                level++;

                float nextCriticalChance = data.values[level];
                weapon.UpgradeCriticalChance(nextCriticalChance);
                break;
            case ItemData.ItemType.CriticalDamage:
                level++;

                float nextCriticalDamage = data.values[level];
                weapon.UpgradeCriticalDamage(nextCriticalDamage);
                break;
            case ItemData.ItemType.AttackRange:
                level++;

                float nextRange = data.values[level];
                weapon.UpgradeScanRange(nextRange);
                break;

            case ItemData.ItemType.Health:
                level++;

                double nextHealth = data.values[level];
                gear.UpgradeMaxHealth(nextHealth);
                break;
            case ItemData.ItemType.Regeneration:
                level++;

                float nextRegen = data.values[level];
                gear.UpgradeRegen(nextRegen);
                break;
            case ItemData.ItemType.Defense:
                level++;

                float nextDef = data.values[level];
                gear.UpgradeDefense(nextDef);
                break;
            case ItemData.ItemType.Reflection:
                level++;

                float nextReflection = data.values[level];
                gear.UpgradeReflection(nextReflection);
                break;
            case ItemData.ItemType.LifeSteal:
                level++;

                float nextLifeSteal = data.values[level];
                gear.UpgradeLifeSteal(nextLifeSteal);
                break;
            case ItemData.ItemType.GoldMultiplier:
                level++;

                float nextGoldMulti = data.values[level];
                gear.UpgradeGoldMulti(nextGoldMulti);
                break;
            case ItemData.ItemType.GoldPerWave:
                level++;

                float nextGoldWave = data.values[level];
                gear.UpgradeGoldWave(nextGoldWave);
                break;
            case ItemData.ItemType.CoinMultiplier:
                level++;

                float nextCoinMulti = data.values[level];
                gear.UpgradeCoinMulti(nextCoinMulti);
                break;
            case ItemData.ItemType.CoinPerWave:
                level++;

                float nextCoinWave = data.values[level];
                gear.UpgradeCoinWave(nextCoinWave);
                break;

            default:
                break;
        }



        if (level == data.values.Length - 1)
        {
            GetComponentInChildren<Button>().interactable = false;
        }

    }
}
