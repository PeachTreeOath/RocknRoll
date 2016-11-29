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

    private float myMass = 0.01f;
    private Vector2 positionLastFrame;
    private Vector2 velocityLastFrame;
    private Vector2 accelLastFrame;
    private Vector2 netAccelToApplyWc;
    private Vector2 positionAfterUpdate;
    private bool positionAfterUpdateCalculated;

    private Vector2 gravityAccel = new Vector2(0, -0.1f);

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
        netAccelToApplyWc = Vector2.zero;
        velocityLastFrame = Vector2.zero;
        accelLastFrame = gravityAccel;
    }

    // Update is called once per frame
    void Update()
    {
        //apply gravity
        //netAccelToApplyWc; += new Vector2(0, -0.01f); //This may need to be applied later, maybe in late update?
        Vector2 nextPos = getNextPosition();
        checkCollisions(nextPos);
        adjustNextPosition(netAccelToApplyWc);



    }

    void LateUpdate() {
        velocityLastFrame = (positionAfterUpdate - (Vector2)transform.position) / Time.deltaTime; //TODO time is off by 1 frame
        positionLastFrame = transform.position;
        transform.position = positionAfterUpdate;
        netAccelToApplyWc = Vector2.zero;
    }

    /// <summary>
    /// Determine the position of this object at the end of the current frame based on forces applied
    /// during this frame. Calculations for all object interactions are done with the objects in the state
    /// of the current frame.
    /// </summary>
    public Vector2 getNextPosition() {

        //Vector2 mvDirection = (Vector2) transform.position - positionLastFrame;
        //Vector2 pos = (Vector2) transform.position + netAccelToApplyWc
        //Vector2 pos = velocityLastFrame * Time.deltaTime + positionLastFrame;  // vt + p
        Vector2 pos =  accelLastFrame * (0.5f * Mathf.Pow(Time.deltaTime, Time.deltaTime)) +
                        velocityLastFrame * Time.deltaTime + 
                        positionLastFrame;  // 1/2 at^2 + vt + p 

        positionAfterUpdate = pos;
        positionAfterUpdateCalculated = true;
        return positionAfterUpdate;
    }

    public Vector2 adjustNextPosition(Vector2 accelUpdates) {
        //This is prototype code only. 
        Vector2 pos =  (accelLastFrame+accelUpdates) * (0.5f * Mathf.Pow(Time.deltaTime, Time.deltaTime)) +
                        velocityLastFrame * Time.deltaTime + 
                        positionLastFrame;  // 1/2 at^2 + vt + p 

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
        float maxDist = Math.Max(1, dirLen * 2); //room for error
        dir /= dirLen; //normalize
        //get two maximum extent points perpendicular to the direction. We will cast rays from here.
        Vector2 exMid = (Vector2) transform.position + maxExtent * dir;
        Vector2 ex1 = new Vector2(-dir.y, dir.x) * maxExtent + (Vector2)transform.position;
        Vector2 ex2 =  new Vector2(dir.y, -dir.x)  * maxExtent+ (Vector2)transform.position;

        RaycastHit2D[] hitInfo = new RaycastHit2D[3];
        //cast from the center and one from each side along the current trajectory
        hitInfo[0] = Physics2D.Raycast(exMid, dir, maxDist, physicsLayer.value);
        //int results = collider.Raycast(dir, hitInfo, maxDist, physicsLayer.value);
        Debug.DrawRay(exMid, dir * maxDist, Color.cyan, 1.0f, false);

        hitInfo[1] = Physics2D.Raycast(ex1, dir, maxDist, physicsLayer.value);
        Debug.DrawRay(ex1, dir * maxDist, Color.green, 1.0f, false);

        hitInfo[2] = Physics2D.Raycast(ex2, dir, maxDist, physicsLayer.value);
        Debug.DrawRay(ex2, dir * maxDist, Color.red, 1.0f, false);

        //Debug.Log("Num results = " + results);
        GameObject goHit;
        for (int i = 0; i < 3; i++) {
        //for (int i = 0; i < results; i++) {
            if (hitInfo[i]) {
                if (collider.bounds.Intersects(hitInfo[i].collider.bounds)) {
                    //if (collider.IsTouching(hitInfo[i].collider)) { //only works with physics
                    Debug.Log("myBounds=" + collider.bounds);
                    Debug.Log("otherBounds=" + hitInfo[i].collider.bounds);
                    Debug.Log("Hit some shit: [" + i + "] =>" + hitInfo[i].collider.gameObject.name);
                    goHit = hitInfo[i].collider.gameObject;
                    CustomPhysics gop = goHit.GetComponent<CustomPhysics>();
                    if (gop != null) {
                        Vector2 hitPt = collider.bounds.ClosestPoint(goHit.transform.position);
                        Vector2 n = (hitPt - (Vector2)goHit.transform.position).normalized;
                        Vector2 forceIn = velocityLastFrame * myMass; //TODO this is current V not V in the future when the collision happens
                        Vector2 myForceOut;
                        Vector2 otherForceOut;
                        gop.curMat.collide(phys.curMat, n, forceIn, out myForceOut, out otherForceOut);
                        Debug.Log("MyForceOut=" + myForceOut);
                        netAccelToApplyWc += myForceOut / myMass; // F = ma
                        //Debug.Break();
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
