using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EventTrigger.TriggerEvent callBack;
    
    [SerializeField] private Sprite unSelectedSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Image spriteRenderer;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        callBack.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.sprite = selectedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.sprite = unSelectedSprite;
    }
}
