using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        if (InputController.instance.GetRightClick())
        {
            UseActive();
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
