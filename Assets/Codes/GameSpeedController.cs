using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    public Button speedUpButton;
    public Button speedDownButton;
    public Text speedText;

    private float[] speedLevels = { 0, 0.5f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 5f};
    private int currentLevel = 2; // 1f

    private void Start()
    {
        UpdateSpeedDisplay();

        speedUpButton.onClick.AddListener(IncreaseSpeed);
        speedDownButton.onClick.AddListener(DecreaseSpeed);
    }

    void IncreaseSpeed()
    {
        if (currentLevel < speedLevels.Length - 1)
        {
            currentLevel++;
            ApplySpeed();
        }
    }

    void DecreaseSpeed()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            ApplySpeed();
        }
    }

    void ApplySpeed()
    {
        Time.timeScale = speedLevels[currentLevel];
        UpdateSpeedDisplay();
    }

    void UpdateSpeedDisplay()
    {
        if (speedText != null)
            speedText.text = $"x{speedLevels[currentLevel]:0.0}";
    }
}
