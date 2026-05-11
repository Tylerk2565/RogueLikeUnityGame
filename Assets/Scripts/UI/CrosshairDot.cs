using UnityEngine;
using UnityEngine.UI;

public class CrosshairDot : MonoBehaviour
{
    public Canvas canvas;
    public float dotSize = 6f;
    public Color dotColor = Color.black;

    void Start()
    {
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Crosshair Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        GameObject dotObj = new GameObject("Black Dot Crosshair");
        dotObj.transform.SetParent(canvas.transform);

        Image dotImage = dotObj.AddComponent<Image>();
        dotImage.color = dotColor;

        RectTransform rect = dotObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(dotSize, dotSize);
    }
}