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

    public void ReceiveAttack(Vector2 direction, float impulseStrength)
    {
        rBody.AddForce(direction * impulseStrength, ForceMode2D.Impulse);
    }
}
