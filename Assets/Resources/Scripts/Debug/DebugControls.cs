using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DebugControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void dbgResetScene() {
        SceneManager.LoadScene("Battle");
    }
}
