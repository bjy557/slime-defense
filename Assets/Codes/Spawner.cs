using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

    int wave = -1;

    int currentSpawned = 0;
    int monstersPerWave;
    float spawnInterval = 1f;

    bool bossSpawned = false;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive || GameManager.instance.IsCooldown())
            return;

        if (wave != GameManager.instance.wave)
        {
            wave = GameManager.instance.wave;

            currentSpawned = 0;

            // calculate monsters per wave based on wave number
            monstersPerWave = Mathf.RoundToInt(14.9f * Mathf.Pow(wave + 1, 0.23f));
            monstersPerWave = Mathf.Max(monstersPerWave, 1); // minimum 1 monster per wave

            // calculate spawn interval based on monsters per wave
            spawnInterval = Mathf.Max(20f / monstersPerWave, 0.1f); // prevent too short intervals
        }

        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            timer = 0;
            Spawn();
        }

        if (GameManager.instance.wave % 10 == 0 && bossSpawned)
        {
            bossSpawned = false; // false in next wave
        }
    }

    void Spawn()
    {
        if (currentSpawned >= monstersPerWave)
            return;

        GameObject enemy = GameManager.instance.pool.Get(0);

        if (GameManager.instance.wave % 10 == 9 && !bossSpawned)
        {
            enemy.GetComponent<Enemy>().isBoss = true; // set boss flag
            bossSpawned = true;
        }
        else
        {
            enemy.GetComponent<Enemy>().isBoss = false;
        }

        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        
        wave = GameManager.instance.wave;
        enemy.GetComponent<Enemy>().Init(spawnData[wave]);

        currentSpawned++;
    }

    public int montersPerWave => this.monstersPerWave;
}

[System.Serializable]
public class SpawnData
{
    public double health;
    public double damage;
}