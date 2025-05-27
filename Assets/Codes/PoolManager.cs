using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefaps;

    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefaps.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 비활성화된 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 못 찾았으면 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(prefaps[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
