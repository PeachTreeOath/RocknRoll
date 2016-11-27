using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class HeroAttackSector : MonoBehaviour {

    public float DMG_PER_HIT = 1;
    public float autoAttackCooldown = 2;
    public float impulseStrength;
    public float jitterScale = .1f;

    private float lastAttackTime;
    private bool attackReady;

    public GameObject assignedHero;

    //TODO clean this up
    private Material yellowMat;
    private Material greenMat;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private Hero hero;

    private List<KeyValuePair<GameObject, ContactPoint2D>> collidedList = new List<KeyValuePair<GameObject, ContactPoint2D>>();

    // Use this for initialization
    void Start() {
        // TODO: Colors are just for testing
        yellowMat = Resources.Load<Material>("Materials/YellowMat");
        greenMat = Resources.Load<Material>("Materials/GreenMat");
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        hero = transform.parent.GetComponent<Hero>();

        ReadyAttack(true);
    }

    // Update is called once per frame
    void Update() {

        if (!attackReady && Time.time - lastAttackTime > autoAttackCooldown) {
            ReadyAttack(true);
        }

        if (attackReady && collidedList.Count > 0) {

            lastAttackTime = Time.time;
            foreach (KeyValuePair<GameObject, ContactPoint2D> kvp in collidedList) {
                GameObject attackedObj = kvp.Key;
                ContactPoint2D cpt = kvp.Value;
                Vector2 direction = attackedObj.transform.position - transform.position;
                IAttackable atk = attackedObj.GetComponent<IAttackable>();
                CustomPhysics myPhysics = assignedHero.GetComponent<CustomPhysics>();
                Vector2 outForce;
                atk.ReceiveForce(myPhysics, cpt.normal, direction, impulseStrength, out outForce);
                atk.ReceiveDmg(DMG_PER_HIT);
            }
            hero.StartAutoAttack();
            collidedList.Clear();
            ReadyAttack(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Trigger Entered");
        }

    void OnCollisionEnter2D(Collision2D col) {
        ContactPoint2D pt = col.contacts[0];
        Debug.Log("Collision Enter");
        if (col.gameObject != null)
        {
            collidedList.Add(new KeyValuePair<GameObject, ContactPoint2D>(col.gameObject, pt));
        }
    }

    //void OnTriggerExit2D(Collider2D col)
    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("Collision Exit");
        IAttackable obj = col.gameObject.GetComponent<IAttackable>();
        if (obj != null)
        {
            //collidedList.Remove(col.gameObject);
        }
    }

    public void Toggle(bool toggle)
    {
        GetComponent<CircleCollider2D>().enabled = toggle;
    }

    private void ReadyAttack(bool enabled)
    {
        if (enabled)
        {
            // spriteRenderer.material = greenMat;
        }
        else
        {
            // spriteRenderer.material = yellowMat;
        }
        attackReady = enabled;
    }
}
