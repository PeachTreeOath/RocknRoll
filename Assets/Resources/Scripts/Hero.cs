using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{

    private bool isMenuShowing;

    private Canvas canvas;

    // Use this for initialization
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (InputController.instance.GetLeftClick())
        {
            ToggleMenu();
        }
        if (InputController.instance.GetRightClick())
        {
            UseActive();
        }
    }

    private void ToggleMenu()
    {
        isMenuShowing = !isMenuShowing;

        if(isMenuShowing)
        {

            canvas.enabled = isMenuShowing;
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
