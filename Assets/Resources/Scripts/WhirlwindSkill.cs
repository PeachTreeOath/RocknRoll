using UnityEngine;
using System.Collections;
using System;

public class WhirlwindSkill : AbstractSkill
{

    public RuntimeAnimatorController animator;

    public void Update()
    {

    }

    public override void ExecuteActive()
    {
        gameObject.SetActive(true);
        hero.GetComponent<Animator>().runtimeAnimatorController = animator;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        IAttackable obj = col.GetComponent<IAttackable>();
        if (obj != null)
        {
            col.GetComponent<Rigidbody2D>().drag = 10;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        IAttackable obj = col.GetComponent<IAttackable>();
        if (obj != null)
        {
            col.GetComponent<Rigidbody2D>().drag = 0;
        }
    }
}