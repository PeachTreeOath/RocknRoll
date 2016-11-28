using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CustomPhysics), typeof(Collider2D))]
public class Enemy : MonoBehaviour, IAttackable
{

    private LayerMask physicsLayer = -1; //can't set value here
    private Collider2D collider;
    private float maxExtent;
    private CustomPhysics phys;

    private Vector2 positionLastFrame;
    private Vector2 netForceToApplyWc;
    private Vector2 positionAfterUpdate;
    private bool positionAfterUpdateCalculated;

    // Use this for initialization
    void Start()
    {
        physicsLayer = LayerMask.GetMask("Physics");
        collider = GetComponent<Collider2D>();
        //maxExtent = collider.bounds.extents.magnitude + 0.01f; //e.g. radius
        maxExtent = (collider.bounds.extents.x + collider.bounds.extents.x + 0.01f) / 2; //e.g. radius, approximate
        phys = GetComponent<CustomPhysics>();
        positionLastFrame = transform.position;
        positionAfterUpdateCalculated = false;
        netForceToApplyWc = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //apply gravity
        netForceToApplyWc += new Vector2(0, -0.01f); //This may need to be applied later, maybe in late update?
        Vector2 nextPos = getNextPosition();
        checkCollisions(nextPos);



    }

    void LateUpdate() {
        positionLastFrame = transform.position;
        transform.position = positionAfterUpdate;
        //netForceToApplyWc = Vector2.zero; //reset for next frame
    }

    /// <summary>
    /// Determine the position of this object at the end of the current frame based on forces applied
    /// during this frame. Calculations for all object interactions are done with the objects in the state
    /// of the current frame.
    /// </summary>
    public Vector2 getNextPosition() {
        
        Vector2 mvDirection = (Vector2) transform.position - positionLastFrame;
        Vector2 pos = (Vector2) transform.position + netForceToApplyWc;

        positionAfterUpdate = pos;
        positionAfterUpdateCalculated = true;
        return positionAfterUpdate;
    }

    /// <summary>
    /// Check for objects between the current position and the targetPos
    /// </summary>
    private void checkCollisions(Vector2 targetPos) {
        Vector2 dir = targetPos - (Vector2)transform.position;
        float dirLen = dir.magnitude;
        float maxDist = dirLen * 20; //room for error
        dir /= dirLen; //normalize
        //get two maximum extent points perpendicular to the direction. We will cast rays from here.
        Vector2 exMid = (Vector2) transform.position + maxExtent * dir;
        Vector2 ex1 = new Vector2(-dir.y, dir.x) * maxExtent + (Vector2)transform.position;
        Vector2 ex2 =  new Vector2(dir.y, -dir.x)  * maxExtent+ (Vector2)transform.position;

        RaycastHit2D[] hitInfo = new RaycastHit2D[3];
        //cast from the center and one from each side along the current trajectory
        hitInfo[0] = Physics2D.Raycast(exMid, dir, maxDist, physicsLayer.value);
        //int results = collider.Raycast(dir, hitInfo, maxDist, physicsLayer.value);
        Debug.DrawRay(exMid, dir, Color.cyan, 0.25f, false);

        hitInfo[1] = Physics2D.Raycast(ex1, dir, maxDist, physicsLayer.value);
        Debug.DrawRay(ex1, dir, Color.green, 0.25f, false);

        hitInfo[2] = Physics2D.Raycast(ex2, dir, maxDist, physicsLayer.value);
        Debug.DrawRay(ex2, dir, Color.red, 0.25f, false);

        //Debug.Log("Num results = " + results);
        GameObject goHit;
        for (int i = 0; i < 3; i++) {
        //for (int i = 0; i < results; i++) {
            if (hitInfo[i]) {
                if (collider.bounds.Intersects(hitInfo[i].collider.bounds)) {
                //if (collider.IsTouching(hitInfo[i].collider)) { //only works with physics
                    Debug.Log("Hit some shit: " + hitInfo[i].collider.gameObject.name);
                    goHit = hitInfo[i].collider.gameObject;
                    CustomPhysics gop = goHit.GetComponent<CustomPhysics>();
                    if (gop != null) {
                        Vector2 hitPt = collider.bounds.ClosestPoint(goHit.transform.position);
                        Vector2 n = (hitPt - (Vector2)goHit.transform.position).normalized;
                        Vector2 v = new Vector2(0f, 1.0f);//TODO
                        Vector2 myForceOut;
                        Vector2 otherForceOut;
                        gop.curMat.collide(phys.curMat, n, v, out myForceOut, out otherForceOut);
                        Debug.Log("MyForceOut=" + myForceOut);
                        positionAfterUpdate += myForceOut;
                    }
                }
            }
        }

    }

    public void ReceiveForce(CustomPhysics otherPhys, Vector2 surfaceNormal, Vector2 direction, float impulseStrength, out Vector2 myOut)
    {
        Vector2 jitterVector = UnityEngine.Random.insideUnitCircle * 0.1f; //wtf is this
        Vector2 force = jitterVector + direction + (direction.normalized * impulseStrength);
        Vector2 forceOut;
        phys.curMat.collide(otherPhys.curMat, surfaceNormal, force, out forceOut, out myOut); 

        //rBody.AddForce(myOut, ForceMode2D.Impulse);
        //rBody.AddForce(direction * impulseStrength, ForceMode2D.Impulse);
    }

    public void ReceiveDragChange(float drag)
    {
        //rBody.drag = drag;
    }

    // Strength is a value between 0-1 used for lerping. Since this function is probably called
    // every frame, set this to a low number for a slow vacuum.
    public void ReceiveVacuum(Vector2 source, float strength)
    {
        Vector2 newPos = Vector2.Lerp(transform.position, source, strength);
        transform.position = newPos;
    }

    public void ReceiveDmg(float amt) {
        HealthBar hb = GetComponentInChildren<HealthBar>();
        if (hb != null) {
            hb.changeHealth(amt);
        }
    }
}
