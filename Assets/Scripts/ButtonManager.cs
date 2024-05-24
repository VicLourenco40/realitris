using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isClicked = false;

    public bool IsClicked {
        get {
            bool clicked = isClicked;
            isClicked = false;
            return clicked;
        }
    }

    public bool isHeld = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        isHeld = false;
    }
}
