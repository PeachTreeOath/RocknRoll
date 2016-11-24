using UnityEngine;
using System.Collections;
using System;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    private GameObject hbPrefab; //health bar prefab with Animation attached

    [SerializeField]
    private string animName = "lifebar"; //must match anim clip name

    [SerializeField]
    private float totalHp = 50;

    private GameObject hbInst;
    private Animator anim;
    private float curHp;
    private Vector3 lastPPos; //holds player or enemy position we are attached to from last frame

    void Awake() {
        hbInst = Instantiate(hbPrefab); //maybe we want to use a set gameobject instance instead?
        hbInst.transform.position += gameObject.transform.position;

        anim = hbInst.GetComponentInChildren<Animator>();

        if (anim == null) {
            Debug.LogError("Couldn't load lifebar animation. Ensure component has an animation attached with the correct animation clip name");
        }
        curHp = totalHp;
        Debug.Log("Anim state name " + animName);
    }

    void Start() {
        lastPPos = transform.position;
        anim.speed = 0;
        anim.Play(animName, 0, hpToTime(curHp));
    }

    void Update() {
        hbInst.transform.position += transform.position - lastPPos;
        lastPPos = transform.position;
    }

    void OnDestroy() {
        if (anim != null) {
            anim.Stop();
            anim = null;
        }
        if (hbInst != null) {
            Destroy(hbInst);
            hbInst = null;
        }
    }

    /// <summary>
    /// Subtract an amount of health from this healthbar. Positive numbers lower the health, negative number raise the health.
    /// </summary>
    public void changeHealth(float amt) {
        curHp = Mathf.Clamp(curHp - amt, 0, totalHp);
        float t = hpToTime(curHp);
        if (t >= 1) {
            t = 0.99f;
            //TODO kill
        }
        anim.Play(animName, 0, t);
    }

    /// <summary>
    /// Convert an hp amount to the time that represents that hp level in the animation.
    /// Return value is normalized 0 to 1. A value of >= 1 is zero health meaning the bar should
    /// no longer be displayed.
    /// </summary>
    private float hpToTime(float hp) {
        return 1 - (hp/totalHp); //note 0 time is max hp, value is normalilzed
    }

}
