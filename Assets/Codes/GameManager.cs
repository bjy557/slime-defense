using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;

    [Header("# Player Info")]
    public float health;
    public float maxHealth;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public GameObject uiResult;
    public GameObject enemyCleaner;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart()
    {
        isLive = true;
        health = maxHealth;

        // Attack 아이템 중 level이 0인 것을 찾아 무기 생성
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);

        Weapon weapon = new Weapon();

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

        gameTime += Time.deltaTime;
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        }
    }

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
