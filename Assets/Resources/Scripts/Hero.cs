﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class Hero : MonoBehaviour, IPointerClickHandler
{

    public GameObject menu;
    public GameObject whirlwindPrefab;
    private AbstractSkill activeSkill;
    private HeroAttackSector[] attackSectors;
    private bool isMenuShowing;
    private Sprite defaultSprite;
    private RuntimeAnimatorController defaultAnimator;
    private float radius;

    // Use this for initialization
    void Start()
    {
        attackSectors = GetComponentsInChildren<HeroAttackSector>();
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        defaultAnimator = GetComponent<Animator>().runtimeAnimatorController;
        InputController.instance.RegisterHero(this);

        CreateSkills();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ToggleMenu(bool toggle)
    {
        isMenuShowing = toggle;
        menu.SetActive(isMenuShowing);
    }

    // Load in skill prefabs and parent their hitboxes and scripts under the hero unit
    private void CreateSkills()
    {
        GameObject whirlwind = (GameObject)Instantiate(whirlwindPrefab, transform.position, Quaternion.identity);
        whirlwind.transform.SetParent(transform);
        WhirlwindSkill skill = whirlwind.GetComponent<WhirlwindSkill>();
        skill.SetHero(this);
        whirlwind.SetActive(false);
        activeSkill = skill;
    }

    private void UseActive()
    {
        foreach(HeroAttackSector sector in attackSectors)
        {
            sector.Toggle(false);
        }
        activeSkill.ExecuteActive();
    }

    private void StopActive()
    {
        foreach (HeroAttackSector sector in attackSectors)
        {
            sector.Toggle(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            bool tempIsMenuShowing = !isMenuShowing;
            InputController.instance.ClearMenuSelection();
            ToggleMenu(tempIsMenuShowing);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InputController.instance.ClearMenuSelection();
            UseActive();
        }
    }
}
