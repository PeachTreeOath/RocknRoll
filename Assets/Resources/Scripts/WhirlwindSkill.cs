using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WhirlwindSkill : AbstractSkill
{

    public RuntimeAnimatorController animator;

    protected List<GameObject> collidedList = new List<GameObject>();

    protected Vector2 previewSrcLocation;
    protected Vector2 previewDestLocation;
    [SerializeField]
    private GameObject previewPrefab;
    private GameObject previewInstance;
    private LineRenderer targetLine;

    protected override void Update()
    {
        base.Update();
        if (!isExecuting && !isPreviewing)
        {
            StopActive();
        }
    }

    protected override void startPreviewing() {
        if (previewInstance == null && previewPrefab != null) {
            gameObject.SetActive(true);
            previewInstance = (GameObject)Instantiate(previewPrefab, new Vector2(-100,-100), Quaternion.identity);
            float size = GetComponent<Collider2D>().bounds.extents.magnitude;
            previewInstance.transform.localScale = new Vector2(size,size);
            targetLine = previewInstance.GetComponent<LineRenderer>();
        }
        base.startPreviewing();
    }

    protected override void stopPreviewing() {
        base.stopPreviewing();
        if (previewInstance != null) {
            Destroy(previewInstance);
            previewInstance = null;
        }
    }

    protected override void DrawPreview() {
        Vector2 targetPos =  InputController.instance.GetPointerPosition();
        previewInstance.transform.position = targetPos;
        Vector2 dir = targetPos - (Vector2)transform.position;
        dir = dir.normalized;
        targetLine.SetPosition(0, transform.position);
        targetLine.SetPosition(1, targetPos - Vector2.Scale(dir, previewInstance.transform.localScale)); //stop outside bounds
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
        foreach (GameObject col in collidedList)
        {
            if (col != null) {
                IAttackable obj = col.GetComponent<IAttackable>();
                if (obj != null) {
                    obj.ReceiveDragChange(0);
                }
            }
        }

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