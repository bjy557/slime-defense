using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    public Vector3 offset = new Vector3(-0.05f, 0.06f, 0);

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position + offset);
        rect.localScale = Vector3.one * 0.8f;
    }
}
