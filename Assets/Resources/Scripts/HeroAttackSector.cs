using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class HeroAttackSector : MonoBehaviour
{

    public float DMG_PER_HIT = 1;
    public float autoAttackCooldown = 2;
    public float impulseStrength;
    public float jitterScale = .1f;

    private float lastAttackTime;
    private bool attackReady;

    //TODO clean this up
    private Material yellowMat;
    private Material greenMat;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private List<GameObject> collidedList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        // TODO: Colors are just for testing
        yellowMat = Resources.Load<Material>("Materials/YellowMat");
        greenMat = Resources.Load<Material>("Materials/GreenMat");
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        ReadyAttack(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (!attackReady && Time.time - lastAttackTime > autoAttackCooldown)
        {
            ReadyAttack(true);
        }

        if (attackReady && collidedList.Count > 0)
        {

            lastAttackTime = Time.time;
            foreach (GameObject attackedObj in collidedList)
            {
                Vector2 direction = attackedObj.transform.position - transform.position;
                IAttackable atk = attackedObj.GetComponent<IAttackable>();
                atk.ReceiveForce(direction, impulseStrength, jitterScale);
                atk.ReceiveDmg(DMG_PER_HIT);
            }
            collidedList.Clear();
            ReadyAttack(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        IAttackable obj = col.GetComponent<IAttackable>();
        if (obj != null)
        {
            collidedList.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        IAttackable obj = col.GetComponent<IAttackable>();
        if (obj != null)
        {
            collidedList.Remove(col.gameObject);
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
