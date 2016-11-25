using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

// Handles communication between heroes after input
public class InputController : Singleton<InputController>, IPointerClickHandler
{

    public bool isSelecting;

    private List<Hero> heroList = new List<Hero>();
    private List<ISelectionListener> selectionListeners = new List<ISelectionListener>();

    public void RegisterHero(Hero hero)
    {
        heroList.Add(hero);
    }

    public void DeregisterHero(Hero hero)
    {
        heroList.Remove(hero);
    }

    // Heroes are registered to this class so all menus can be deactivated at once.
    public void ClearMenuSelection()
    {
        foreach (Hero hero in heroList)
        {
            hero.ToggleMenu(false);
        }
    }

    // Register click listener
    public void RegisterSelectionListener(ISelectionListener listener)
    {
        selectionListeners.Add(listener);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (ISelectionListener selectionListener in selectionListeners)
        {
            selectionListener.OnSelection(eventData.position);
        }
    }
}
