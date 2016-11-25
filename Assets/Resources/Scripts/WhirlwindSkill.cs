using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WhirlwindSkill : AbstractSkill
{

    public RuntimeAnimatorController animator;

    protected List<GameObject> collidedList = new List<GameObject>();

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
        hero.animator.runtimeAnimatorController = animator;
        StartTimer();
    }

    public override void StopActive()
    {
        hero.animator.runtimeAnimatorController = hero.defaultAnimator;
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
            obj.ReceiveDragChange(10);
            collidedList.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        IAttackable obj = col.GetComponent<IAttackable>();
        if (obj != null)
        {
            obj.ReceiveDragChange(0);
            collidedList.Remove(col.gameObject);
        }
    }
}