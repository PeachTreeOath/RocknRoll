using UnityEngine;
using System.Collections;

public abstract class AbstractSkill : MonoBehaviour {
    public delegate void ActiveFinished();
    abstract public void ExecuteActive(ActiveFinished callback);
    abstract public void StopActive();
    protected abstract void DrawPreview();

    public float duration;
    public float cooldown;

    protected Hero hero;
    protected ActiveFinished callback;
    protected float elapsedTime;
    protected bool isExecuting;
    protected bool isPreviewing;

    protected virtual void Update() {
        if (isPreviewing) {
            DrawPreview();
        }
        if (isExecuting) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > duration) {
                isExecuting = false;
            }
        }
    }

    protected virtual void startPreviewing() {
        isPreviewing = true;
    }

    protected virtual void stopPreviewing() {
        isPreviewing = false;
    }

    public void SetHero(Hero hero) {
        this.hero = hero;
    }

    public void StartTimer() {
        elapsedTime = 0;
        isExecuting = true;
    }
}
