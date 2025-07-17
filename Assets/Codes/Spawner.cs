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

            // 👇 몬스터 수 공식 적용
            monstersPerWave = Mathf.RoundToInt(14.9f * Mathf.Pow(wave + 1, 0.23f));
            monstersPerWave = Mathf.Max(monstersPerWave, 1); // 최소 1마리

            // 👇 스폰 간격 계산
            spawnInterval = Mathf.Max(20f / monstersPerWave, 0.1f); // 너무 빠른 스폰 방지

            //Debug.Log($"[Wave {wave + 1}] 몬스터 수: {monstersPerWave}, 스폰 간격: {spawnInterval:F2}초");
        }

        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            timer = 0;
            Spawn();

            //Debug.Log($"[Wave {wave + 1}] 몬스터 스폰됨: {currentSpawned + 1}/{monstersPerWave}");
        }
    }

    void Spawn()
    {
        if (currentSpawned >= monstersPerWave)
            return;

        GameObject enemy = GameManager.instance.pool.Get(0);
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