using UnityEngine;
using UnityEngine.UI;

public class FitGridToParent : MonoBehaviour
{
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();

        float width = rt.rect.width;

        // 여백(padding)과 간격(spacing)을 고려하여 cell size 계산
        float spacing = grid.spacing.x;
        float padding = grid.padding.left + grid.padding.right;
        int columns = grid.constraintCount;

        float cellWidth = (width - padding - (spacing * (columns - 1))) / columns;
        float cellHeight = 25;

        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
