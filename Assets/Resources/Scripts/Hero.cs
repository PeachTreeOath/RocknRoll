using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class Hero : MonoBehaviour, IPointerClickHandler
{

    // These defaults are used to reset a hero back to its original state once an active is complete
    [HideInInspector]
    public Sprite defaultSprite;
    [HideInInspector]
    public Animator animator;
    public RuntimeAnimatorController defaultAnimator;

    public GameObject menu;
    public GameObject whirlwindPrefab;

    private AbstractSkill activeSkill;
    private HeroAttackSector[] attackSectors;
    private bool isMenuShowing;
    private bool isAttacking;

    // Use this for initialization
    void Start()
    {
        attackSectors = GetComponentsInChildren<HeroAttackSector>();
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        animator = GetComponent<Animator>();
        defaultAnimator = animator.runtimeAnimatorController;
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
        foreach (HeroAttackSector sector in attackSectors)
        {
            sector.Toggle(false);
        }
        activeSkill.ExecuteActive(StopActive);
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

    public void StartAutoAttack()
    {
        animator.Play("Attack");
        isAttacking = true;
    }

}
