using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

// Used to clear menu selection when ground is selected
public class BackgroundInputHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        InputController.instance.ClearMenuSelection();
    }
}
