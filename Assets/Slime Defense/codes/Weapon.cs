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

    void Update()
    {
        
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                break;
        }
    }
}
