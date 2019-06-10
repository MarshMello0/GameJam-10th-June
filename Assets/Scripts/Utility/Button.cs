using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler
{
    public EventTrigger.TriggerEvent callBack;

    public void OnPointerClick(PointerEventData eventData)
    {
        callBack.Invoke(eventData);
    }
}
