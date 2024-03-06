using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseInputCaptureWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<PointerEventData> onPointerEnter;
    public UnityEvent<PointerEventData> onPointerExit;

    public void OnPointerEnter(PointerEventData data)
    {
        onPointerEnter.Invoke(data);
    }

    public void OnPointerExit(PointerEventData data)
    {
        onPointerExit.Invoke(data);
    }
}
