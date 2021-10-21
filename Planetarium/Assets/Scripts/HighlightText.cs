using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HighlightText : EventTrigger
{
    private TextMeshProUGUI button;

    private Color32 oldColor;
    private Color32 newColor = new Color32(255, 128, 0, 255);

    private float oldFontSize;

    private void Awake()
    {
        button = GetComponentInChildren<TextMeshProUGUI>();

        oldColor = button.color;
        oldFontSize = button.fontSize;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        button.color = newColor;
        button.fontSize = oldFontSize + 10f;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        button.color = oldColor;
        button.fontSize = oldFontSize;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        button.color = oldColor;
        button.fontSize = oldFontSize;
    }
}
