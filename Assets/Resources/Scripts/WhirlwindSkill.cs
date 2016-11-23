using UnityEngine;
using System.Collections;
using System;

public class WhirlwindSkill : AbstractSkill
{

    public RuntimeAnimatorController animator;

    protected override void Update()
    {
        base.Update();
        if(!isExecuting)
        {
            StopActive();
        }
    }

    public override void ExecuteActive(ActiveFinished callback)
    {
        this.callback = callback;
        gameObject.SetActive(true);
        hero.GetComponent<Animator>().runtimeAnimatorController = animator;
        StartTimer();
    }

    public override void StopActive()
    {
        hero.GetComponent<Animator>().runtimeAnimatorController = hero.defaultAnimator;
        if (callback != null)
        {
            callback();
        }
        gameObject.SetActive(false);
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