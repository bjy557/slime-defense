using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    public bool isEnemyBullet = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir, bool isEnemyBullet = false)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigid.linearVelocity = dir * 7f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        if (isEnemyBullet && collision.CompareTag("Enemy"))
            return; // ignore enemy bullets hitting other enemies

        if (!isEnemyBullet && collision.CompareTag("Player"))
            return; // ignore player bullets hitting the player

        per--;

        if (per < 0)
        {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
