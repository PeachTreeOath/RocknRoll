using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WhirlwindLargeSkill : WhirlwindSkill
{
    public float sizeChange = 1.25f;

    public override void ExecuteActive(ActiveFinished callback)
    {
        base.ExecuteActive(callback);
        transform.parent.localScale = new Vector2(sizeChange, sizeChange);
    }

    public override void StopActive()
    {
        base.StopActive();
        transform.parent.localScale = Vector2.one;
    }
}