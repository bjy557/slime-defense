using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public int wave;
    public float spawnTime;

    [Header("# Player Info")]
    public double health;
    public double maxHealth;
    public float regen;
    public float defense;
    public float reflection;
    public float lifeSteal;
    public float goldMulti;
    public float goldWave;
    public float coinMulti;
    public float coinWave;
    public int level;
    public double gold;
    public double coin;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public GameObject uiResult;
    public GameObject enemyCleaner;

    private float waveDuration = 20f;
    private float waveCooldown = 4f;
    private float waveTimer = 0f;
    private bool isCooldown = false;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart()
    {
        isLive = true;

        maxHealth = 5;
        regen = 0;
        defense = 0;
        reflection = 0;
        lifeSteal = 0;
        goldMulti = 1;
        goldWave = 0;
        coinMulti = 1;
        coinWave = 1;

        health = maxHealth;

        wave = 0;
        spawnTime = 2;

        gold = 0;
        coin = 0;

        // Attack ?????? ?? level?? 0?? ???? ???? ???? ????
        Item[] items = Resources.FindObjectsOfTypeAll<Item>();

        GameObject sharedWeapon = new GameObject("FireBall");
        sharedWeapon.AddComponent<Weapon>();
        sharedWeapon.GetComponent<Weapon>().Init();

        GameObject sharedGear = new GameObject("Gear");
        sharedGear.AddComponent<Gear>();
        sharedGear.GetComponent<Gear>().Init();

        foreach (var item in items)
        {
            switch (item.data.itemType)
            {
                case ItemData.ItemType.Attack:
                case ItemData.ItemType.AttackSpeed:
                case ItemData.ItemType.CriticalChance:
                case ItemData.ItemType.CriticalDamage:
                case ItemData.ItemType.AttackRange:
                    item.weapon = sharedWeapon.GetComponent<Weapon>();
                    break;
                case ItemData.ItemType.Health:
                case ItemData.ItemType.Regeneration:
                case ItemData.ItemType.Defense:
                case ItemData.ItemType.Reflection:
                case ItemData.ItemType.LifeSteal:

                case ItemData.ItemType.GoldMultiplier:
                case ItemData.ItemType.GoldPerWave:
                case ItemData.ItemType.CoinMultiplier:
                case ItemData.ItemType.CoinPerWave:
                    item.gear = sharedGear.GetComponent<Gear>();
                    break;
                default:
                    break;
            }
        }

        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        Time.timeScale = 1;
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        // enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(1f);

        uiResult.SetActive(true);
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
            return;

        if (isCooldown)
        {
            waveTimer += Time.deltaTime;
            if (waveTimer >= waveCooldown)
            {
                waveTimer = 0f;
                isCooldown = false;
                wave++;

                // wave가 증가하면 wave per gold, coin 실행
                gold += goldWave;
                coin += coinWave;
            }
        }
        else
        {
            waveTimer += Time.deltaTime;
            if (waveTimer >= waveDuration)
            {
                waveTimer = 0f;
                isCooldown = true;
            }
        }
    }

    public bool IsCooldown()
    {
        return isCooldown;
    }
    public float waveDurationTimer => waveDuration;
    public float waveProgressTimer => waveTimer;
    public float waveCooldownTimer => waveCooldown;

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
