using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour, IAttackable
{

    private Rigidbody2D rBody;

    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReceiveForce(Vector2 direction, float impulseStrength, float jitterScale)
    {
        Vector2 jitterVector = UnityEngine.Random.insideUnitCircle * jitterScale;
        rBody.velocity = jitterVector + direction + (direction.normalized * impulseStrength);
        //rBody.AddForce(direction * impulseStrength, ForceMode2D.Impulse);
    }

    public void ReceiveDragChange(float drag)
    {
        rBody.drag = drag;
    }

    // Strength is a value between 0-1 used for lerping. Since this function is probably called
    // every frame, set this to a low number for a slow vacuum.
    public void ReceiveVacuum(Vector2 source, float strength)
    {
        Vector2 newPos = Vector2.Lerp(transform.position, source, strength);
        transform.position = newPos;
    }
}
