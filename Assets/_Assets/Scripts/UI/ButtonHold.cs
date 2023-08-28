using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnButtonPressed;
    public event Action OnButtonReleased;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonReleased?.Invoke();
    }
}
