using UnityEngine;
using UnityEngine.UI;

public class ClockFade : MonoBehaviour
{
    public enum FadeDirection { OpaqueAtTop, OpaqueAtBottom }

    [SerializeField] private FadeDirection direction = FadeDirection.OpaqueAtTop;
    [SerializeField] private Color baseColor = Color.black;

    void Awake()
    {
        Image img = GetComponent<Image>();
        img.raycastTarget = false;
        img.color = baseColor;
        img.sprite = GenerateGradientSprite();
    }

    private Sprite GenerateGradientSprite()
    {
        int size = 64;
        Texture2D tex = new Texture2D(1, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < size; y++)
        {
            float t = (float)y / (size - 1);
            float alpha = direction == FadeDirection.OpaqueAtTop ? t : 1f - t;
            tex.SetPixel(0, y, new Color(1, 1, 1, alpha));
        }
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, 1, size), new Vector2(0.5f, 0.5f));
    }
}