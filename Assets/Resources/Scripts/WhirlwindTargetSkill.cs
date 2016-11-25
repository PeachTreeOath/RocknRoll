using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WhirlwindTargetSkill : WhirlwindSkill
{
    public override void ExecuteActive(ActiveFinished callback)
    {
        //transform.parent.localScale = new Vector2(sizeChange, sizeChange);
        base.ExecuteActive(callback);
    }
    
}