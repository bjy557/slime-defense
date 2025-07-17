using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { SlimeCount, Level, Kill, Coin, Round, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.SlimeCount:
                if (GameManager.instance.IsCooldown())
                {
                    float cooldownLeft = GameManager.instance.waveCooldownTimer - GameManager.instance.waveProgressTimer;
                    mySlider.value = Mathf.Clamp01(cooldownLeft / GameManager.instance.waveCooldownTimer);
                }
                else
                {
                    float stageLeft = GameManager.instance.waveDurationTimer - GameManager.instance.waveProgressTimer;
                    mySlider.value = Mathf.Clamp01(stageLeft / GameManager.instance.waveDurationTimer);
                }
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.gold);
                break;
            case InfoType.Coin:
                myText.text = string.Format("{0:F0}", GameManager.instance.coin);
                break;
            case InfoType.Round:
                myText.text = $"Stage {GameManager.instance.wave + 1}";
                break;
            case InfoType.Health:
                double curHealth = GameManager.instance.health;
                double maxHealth = GameManager.instance.maxHealth;
                mySlider.value = (float)(curHealth / maxHealth);
                break;
        }
    }
}
