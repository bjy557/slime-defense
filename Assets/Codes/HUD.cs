using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Waves, Level, Kill, Time, Health }
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
            case InfoType.Waves:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float gameTime =GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(gameTime / 60);
                int sec = Mathf.FloorToInt(gameTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                double curHealth = GameManager.instance.health;
                double maxHealth = GameManager.instance.maxHealth;
                mySlider.value = (float)(curHealth / maxHealth);
                break;
        }
    }
}
