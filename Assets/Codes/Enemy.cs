using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public double health;
    public double maxHealth;
    public double damage;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    enum EnemyType
    {
        Normal,
        Speed,
        Tank,
        Range,
        Boss
    }
    EnemyType enemyType;

    bool isLive;
    bool isKnockback;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    WaitForSeconds knockbackTime;

    float damageCooldown = 1f;
    float lastAttackTime = -999f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        knockbackTime = new WaitForSeconds(0.2f);
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;

        if (!isKnockback)
        {
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.linearVelocity = Vector2.zero;
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);

        health = maxHealth;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }

    public void Init(SpawnData data)
    {
        int randomIndex = Random.Range(0, 10);
        if (randomIndex < 8)
        {
            enemyType = EnemyType.Normal;
            anim.runtimeAnimatorController = animCon[0];
            speed = 0.6f;
            maxHealth = data.health;
            health = data.health;
            damage = data.damage;
        }
        else
        {
            enemyType = EnemyType.Speed;
            anim.runtimeAnimatorController = animCon[1];
            speed = 1.2f;
            maxHealth = data.health * 0.7f;
            health = data.health * 0.7f;
            damage = data.damage * 0.7f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        float curDamage = collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        ApplyDamage(curDamage, isFromBullet: true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time - lastAttackTime > damageCooldown)
        {
            double realDamage = damage - GameManager.instance.defense;
            if (realDamage < 0)
                realDamage = 0;

            GameManager.instance.health -= realDamage;
            lastAttackTime = Time.time;

            if (GameManager.instance.health <= 0)
            {
                for (int i = 2; i < GameManager.instance.player.transform.childCount; i++)
                {
                    GameManager.instance.player.transform.GetChild(i).gameObject.SetActive(false);
                }
                GameManager.instance.player.GetComponent<Animator>().SetTrigger("Dead");
                GameManager.instance.GameOver();
            }

            // 반사 데미지 처리
            float reflection = GameManager.instance.reflection;
            double reflectedDamage = maxHealth * reflection / 100;
            ApplyDamage(reflectedDamage);
        }
    }

    private void ApplyDamage(double damageAmount, bool isFromBullet = false)
    {
        health -= damageAmount;

        if (isFromBullet)
        {
            float lifeSteal = GameManager.instance.lifeSteal;
            GameManager.instance.health += damageAmount * lifeSteal / 100;
            if (GameManager.instance.health > GameManager.instance.maxHealth)
                GameManager.instance.health = GameManager.instance.maxHealth;
        }

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", true);

        float defaultGold = GameManager.instance.wave / 10 + 1;

        GameManager.instance.gold += (defaultGold * GameManager.instance.goldMulti);

        if (GameManager.instance.isLive)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        isKnockback = true;

        yield return wait; // ???? ?????? ???? ?????? ??????
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 0.5f, ForceMode2D.Impulse); // ???? ????

        yield return knockbackTime;

        isKnockback = false;
    }
}
