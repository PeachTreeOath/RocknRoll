using UnityEngine;
using System.Collections;

public abstract class AbstractSkill : ScriptableObject
{

    abstract public void ExecuteActive();

    public float duration;
    public float cooldown;

}
