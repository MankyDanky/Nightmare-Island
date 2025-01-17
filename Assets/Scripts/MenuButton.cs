using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    TMP_Text text;
    bool hovered;
    bool pressed;
    public Color hoveredColor;
    public Color pressedColor;
    public Color color;

    public void OnPointerDown(PointerEventData eventData)
    {
        text.color = pressedColor;
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (hovered) {
            text.color = hoveredColor;
        } else {
            text.color = color;
        }
        pressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hovered = true;
        if (!pressed) {
            text.color = hoveredColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        hovered = false;
        if (!pressed) {
            text.color = color;
        }
    }

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }    
}
