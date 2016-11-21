using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Handles communication between heroes after input
public class InputController : Singleton<InputController>
{

    private List<Hero> heroList = new List<Hero>();

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
