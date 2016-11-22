using UnityEngine;
using System.Collections;

public abstract class AbstractSkill : MonoBehaviour
{

    abstract public void ExecuteActive();

    public float duration;
    public float cooldown;

    public Hero hero;

    public void SetHero(Hero hero)
    {
        this.hero = hero;
    }
}
