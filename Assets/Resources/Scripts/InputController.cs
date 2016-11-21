using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputController : Singleton<InputController>
{

    private List<Hero> heroList = new List<Hero>();

    public bool GetLeftClick()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetRightClick()
    {
        return Input.GetMouseButtonDown(1);
    }

    public void RegisterHero(Hero hero)
    {
        heroList.Add(hero);
    }

    public void DeregisterHero(Hero hero)
    {
        heroList.Remove(hero);
    }

    public void ClearMenuSelection()
    {
        foreach(Hero hero in heroList)
        {
            hero.ToggleMenu(false);
        }
    }
}
