using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{

    public GameObject menu;

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
            ToggleMenu(!isMenuShowing);
        }
        if (InputController.instance.GetRightClick())
        {
            UseActive();
        }
    }

    public void ToggleMenu(bool toggle)
    {
        isMenuShowing = toggle;

        if(isMenuShowing)
        {
            menu.SetActive(isMenuShowing);
        }
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
