﻿using UnityEngine;
using System.Collections;

public class SceneCleanup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit2D(Collider2D col)
    {
        //Debug.Log("Destroying " + col.gameObject.name);
        Destroy(col.gameObject);
    }
}
