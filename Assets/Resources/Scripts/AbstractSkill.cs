using UnityEngine;
using System.Collections;

public abstract class AbstractSkill : MonoBehaviour
{
    public delegate void ActiveFinished();
    abstract public void ExecuteActive(ActiveFinished callback);
    abstract public void StopActive();

    public float duration;
    public float cooldown;

    protected Hero hero;
    protected ActiveFinished callback;
    protected float elapsedTime;
    protected bool isExecuting;

    protected virtual void Update()
    {
        if (isExecuting)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > duration)
            {
                isExecuting = false;
            }
        }
    }

    public void SetHero(Hero hero)
    {
        this.hero = hero;
    }

    public void StartTimer()
    {
        elapsedTime = 0;
        isExecuting = true;
    }
}
