using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Hero : MonoBehaviour, IPointerClickHandler
{

    public GameObject menu;

    private AbstractSkill activeSkill;
    private bool isMenuShowing;

    // Use this for initialization
    void Start()
    {
        InputController.instance.RegisterHero(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMenu(bool toggle)
    {
        isMenuShowing = toggle;
        menu.SetActive(isMenuShowing);
    }

    private void UseActive()
    {
        transform.localScale *= 2;
        Invoke("StopActive", 1f);
    }

    private void StopActive()
    {
        transform.localScale /= 2;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            bool tempIsMenuShowing = !isMenuShowing;
            InputController.instance.ClearMenuSelection();
            ToggleMenu(tempIsMenuShowing);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InputController.instance.ClearMenuSelection();
            UseActive();
        }
    }
}
