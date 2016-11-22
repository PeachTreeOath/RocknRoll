using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        
        IAttackable obj = col.GetComponent<IAttackable>();
        GameObject gameObj = col.gameObject;
        if (obj != null)
        {
            Vector2 direction = gameObj.transform.position - transform.position;
            gameObject.GetComponent<IAttackable>().ReceiveAttack(direction, impulseStrength, jitterScale);
        }
        
    }*/
}
