using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;

    Player player;

    GameManager gm;

    float timer;

    public void Init()
    {
        name = "Gear";
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        player = GameManager.instance.player;
        gm = GameManager.instance;

        timer = 0f;

        gm.maxHealth = 5;
        gm.regen = 0;
        gm.defense = 0;
        gm.reflection = 0;
        gm.lifeSteal = 0;

        gm.goldMulti = 1;
        gm.goldWave = 0;
        gm.coinMulti = 1;
        gm.coinWave = 1;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            gm.health += gm.regen;
            if (gm.health > gm.maxHealth)
            {
                gm.health = gm.maxHealth;
            }
            timer = 0f;
        }
    }

    public void UpgradeMaxHealth(double nextHealth)
    {
        double curMaxHealth = gm.maxHealth;
        double subHealth = nextHealth - curMaxHealth;

        gm.health += subHealth; // 업그레이드 되는 양 만큼만 체력을 채움
        gm.maxHealth  = nextHealth;
    }

    public void UpgradeRegen(float nextRegen)
    {
        gm.regen = nextRegen;
    }

    public void UpgradeDefense(float nextDef)
    {
        gm.defense = nextDef;
    }

    public void UpgradeReflection(float nextReflection)
    {
        gm.reflection = nextReflection;
    }

    public void UpgradeLifeSteal(float nextLifeSteal)
    {
        gm.lifeSteal = nextLifeSteal;
    }


    public void UpgradeGoldMulti(float nextGoldMulti)
    {
        gm.goldMulti = nextGoldMulti;
    }

    public void UpgradeGoldWave(float nextGoldWave)
    {
        gm.goldWave = nextGoldWave;
    }

    public void UpgradeCoinMulti(float nextCoinMulti)
    {
        gm.coinMulti = nextCoinMulti;
    }

    public void UpgradeCoinWave(float nextCoinWave)
    {
        gm.coinWave = nextCoinWave;
    }
}
