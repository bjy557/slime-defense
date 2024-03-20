using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Position();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Position();
                break;
            default:
                break;
        }
    }

    void Position()
    {
        for (int i=0; i<count; i++)
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

            Quaternion additionalRotation = Quaternion.Euler(0f, 0f, 90f);
            bullet.localRotation *= additionalRotation;

            bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Inifinity Per.
        }
    }
}
