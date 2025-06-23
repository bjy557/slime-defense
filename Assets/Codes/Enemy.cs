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
        // 8 : 2 확률로 0번 혹은 1번 애니메이션 컨트롤러 및 능력치 수정 해야함
        int randomIndex = Random.Range(0, 10);
        if (randomIndex < 8)
        {
            anim.runtimeAnimatorController = animCon[0]; // 80% 확률로 첫 번째 컨트롤러
            speed = 0.6f;
            maxHealth = data.health;
            health = data.health;
            damage = data.damage;
        }
        else
        {
            anim.runtimeAnimatorController = animCon[1]; // 20% 확률로 두 번째 컨트롤러
            speed = 1.2f; // 속도가 더 빠른 적
            maxHealth = data.health * 0.7f;
            health = data.health * 0.7f;
            damage = data.damage * 0.7f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // Live, Hit Action
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // Die
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time - lastAttackTime > damageCooldown)
        {
            GameManager.instance.health -= damage;
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
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        isKnockback = true;

        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 0.5f, ForceMode2D.Impulse); // 넉백 조절

        yield return knockbackTime;

        isKnockback = false;
    }
}
