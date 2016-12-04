using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WhirlwindTargetSkill : WhirlwindSkill, ISelectionListener
{

    private Vector2 srcPos;
    private Vector2 destPos;

    protected override void Update()
    {
        if (isExecuting)
        {
            // Boomerang effect
            // TODO: Figure out why 3.5 is the magic number...
            float time = Mathf.Sin(elapsedTime / duration * 3.5f);
            transform.parent.position = Vector2.Lerp(srcPos, destPos, time);
        }

        base.Update();
    }

    public override void ExecuteActive(ActiveFinished callback)
    {
        InputController.instance.RegisterSelectionListener(this);
        startPreviewing();
    }

    public void OnSelection(Vector2 position)
    {
        stopPreviewing();
        base.ExecuteActive(callback);
        srcPos = transform.position;
        destPos = position;
    }
}