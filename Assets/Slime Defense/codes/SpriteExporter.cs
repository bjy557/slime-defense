using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteExporter : MonoBehaviour
{
    public Sprite[] sprites;

    void Start()
    {
        foreach (Sprite sprite in sprites)
        {
            Texture2D texture = sprite.texture;
            Rect rect = sprite.rect;
            Texture2D newTexture = new Texture2D((int)rect.width, (int)rect.height);

            Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            newTexture.SetPixels(pixels);
            newTexture.Apply();

            byte[] bytes = newTexture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/ExportedSprites/" + sprite.name + ".png", bytes);
        }

        Debug.Log("Sprites exported successfully.");
    }
}
