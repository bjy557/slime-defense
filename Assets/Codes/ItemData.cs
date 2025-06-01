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
        Area,
        Pierce,
        Chain,
        Cooldown,
        Defense,
        Health,
        Regen,
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
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
}
