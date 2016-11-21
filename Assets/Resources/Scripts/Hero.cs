using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
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

    void OnMouseOver()
    {
        if (InputController.instance.GetLeftClick())
        {
            InputController.instance.ClearMenuSelection();
            ToggleMenu(!isMenuShowing);
        }
        if (InputController.instance.GetRightClick())
        {
            InputController.instance.ClearMenuSelection();
            UseActive();
        }
    }

    public void ToggleMenu(bool toggle)
    {
        isMenuShowing = toggle;
        menu.SetActive(isMenuShowing);
    }

    private void UseActive()
    {
        transform.localScale *= 2;
        Invoke("StopActive", 2f);
    }

    private void StopActive()
    {
        transform.localScale /= 2;
    }
}
