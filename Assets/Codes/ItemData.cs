using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Attack,
        AttackSpeed,
        CriticalChance,
        CriticalDamage,
        AttackRange,
        Health,
        Regeneration,
        Defense,
        Reflection,
        LifeSteal,
        GoldMultiplier,
        GoldPerWave,
        CoinMultiplier,
        CoinPerWave
    }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] values;
    public int[] costs;
    public int[] permanents;

    [Header("# Weapon")]
    public GameObject projectile;
}
