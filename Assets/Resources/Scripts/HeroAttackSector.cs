using UnityEngine;
using System.Collections;

public class HeroAttackSector : MonoBehaviour
{

    public float autoAttackCooldown = 2;

    private float lastAttackTime;
    private bool attackPerformedThisFrame;

    //TODO clean this up
    private Material yellowMat;
    private Material greenMat;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

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
        if (Time.time - lastAttackTime > autoAttackCooldown)
        {
            ReadyAttack(true);
        }
    }

    void LateUpdate()
    {
        if (attackPerformedThisFrame)
        {
            ReadyAttack(false);
            attackPerformedThisFrame = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        attackPerformedThisFrame = true;
        //TODO: change this to deal with only enemies
        col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 650));
    }

    private void ReadyAttack(bool enabled)
    {
        if (enabled)
        {
            spriteRenderer.material = greenMat;
        }
        else
        {
            spriteRenderer.material = yellowMat;
        }
        col.enabled = enabled;
    }
}
