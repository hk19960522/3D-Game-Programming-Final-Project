using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEventRecevier : MonoBehaviour, IPointerClickHandler
{
    public Inventory recive;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        recive.PanelClick();
    }
}
