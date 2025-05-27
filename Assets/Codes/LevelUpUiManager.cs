using UnityEngine;

public class LevelUpUIManager : MonoBehaviour
{
    public GameObject groupAttack;
    public GameObject groupDefense;
    public GameObject groupUtility;

    void Start()
    {
        ShowAttack(); // 기본은 공격 탭으로 시작
    }

    public void ShowAttack()
    {
        groupAttack.SetActive(true);
        groupDefense.SetActive(false);
        groupUtility.SetActive(false);
    }

    public void ShowDefense()
    {
        groupDefense.SetActive(true);
        groupAttack.SetActive(false);
        groupUtility.SetActive(false);
    }

    public void ShowUtility()
    {
        groupUtility.SetActive(true);
        groupAttack.SetActive(false);
        groupDefense.SetActive(false);
    }
}
