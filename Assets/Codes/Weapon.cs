using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    public float criticalChance;
    public float criticalDamage;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void UpgradeDamage(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Position();
        }
    }

    public void UpgradeSpeed(float rate)
    {
        this.speed = 1f / rate;
    }

    public void UpgradeCriticalChance(float criticalChance)
    {
        this.criticalChance = criticalChance;
    }

    public void UpgradeCriticalDamage(float criticalDamage)
    {
        this.criticalDamage = criticalDamage;
    }

    public void UpgradeScanRange(float scanRange)
    {
        player.scanner.scanRange = scanRange;
    }

    public void Init()
    {
        // Basic Set
        name = "Fire Ball";
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = 1;
        damage = 3;
        count = 0;
        criticalChance = 1f;
        criticalDamage = 1.2f;

        prefabId = 2;

        switch (id)
        {
            case 0:
                speed = 150;
                Position();
                break;
            default:
                speed = 1f;
                break;
        }
    }

    void Position()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * (-360 * i / count);
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            Quaternion additionalRotation = Quaternion.Euler(0f, 0f, -90f);
            bullet.localRotation *= additionalRotation;

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Inifinity Per.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        float distance = Vector3.Distance(transform.position, player.scanner.nearestTarget.position);
        if (distance > player.scanner.scanRange)
            return;

        Vector3 targetPosition = player.scanner.nearestTarget.position;
        Vector3 dir = targetPosition - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        float newDamage = damage;

        // set critical damage with critical chance
        if (Random.value < criticalChance / 100f)
        {
            newDamage = damage * criticalDamage;
        } else
        {
            newDamage = damage;
        }

        bullet.GetComponent<Bullet>().Init(newDamage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
