using UnityEngine;
using System.Collections;

public class InputController : Singleton<InputController>
{

    public bool GetLeftClick()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetRightClick()
    {
        return Input.GetMouseButtonDown(1);
    }

}
