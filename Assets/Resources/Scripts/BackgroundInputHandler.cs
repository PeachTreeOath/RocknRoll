using UnityEngine;
using System.Collections;

// Used to clear menu selection when ground is selected
public class BackgroundInputHandler : MonoBehaviour {

	void OnMouseDown()
    {
        InputController.instance.ClearMenuSelection();
    }
}
