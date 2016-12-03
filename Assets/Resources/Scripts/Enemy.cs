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

    private float myMass = 1f;
    private Vector2 positionLastFrame;  //initial position
    private Vector2 velocityLastFrame;  //initial velocity
    private Vector2 constAccel;
    private Vector2 netAccelToApplyWc;
    private Vector2 positionAfterUpdate;
    private Vector2 velocityAfterUpdate;
    private bool positionAfterUpdateCalculated;
    private float lastUpdateTime;
    private float minTimeStep = 0.025f;
    private Vector2 gravityAccel = new Vector2(0, -1.0f);

    void Start()
    {
        physicsLayer = LayerMask.GetMask("Physics");
        collider = GetComponent<Collider2D>();
        //maxExtent = collider.bounds.extents.magnitude + 0.01f; //e.g. radius
        maxExtent = (collider.bounds.extents.x + collider.bounds.extents.x + 0.01f) / 2; //e.g. radius, approximate
        phys = GetComponent<CustomPhysics>();
        positionLastFrame = transform.position;
        positionAfterUpdate = transform.position;
        positionAfterUpdateCalculated = false;
        netAccelToApplyWc = Vector2.zero;
        velocityLastFrame = Vector2.zero;
        velocityAfterUpdate = Vector2.zero;
        constAccel = gravityAccel;
        lastUpdateTime = 0;
    }

    void FixedUpdate()
    {
        float dt = Time.time - lastUpdateTime;
        if (dt >= minTimeStep) {
            //Debug.Log("timeStep=" + dt);

            //Figure out where we will be next frame (actually at the very end of this frame)
            //This is a combination of our current position and any force that will be applied by an external object

            Vector2 idealNextPos = getNextPosition(dt, Vector2.zero);
            checkCollisions(idealNextPos);

            //figure out the real next position including collision data (note this can be skipped if no collisions were detected)
            getNextPosition(dt, netAccelToApplyWc);

            lastUpdateTime = Time.time;
        }



    }

    void LateUpdate() {
        //velocityLastFrame = (positionAfterUpdate - (Vector2)transform.position) / Time.deltaTime; //TODO time is off by 1 frame
        velocityLastFrame = velocityAfterUpdate;
        positionLastFrame = transform.position;
        transform.position = positionAfterUpdate;
        netAccelToApplyWc = Vector2.zero;
        positionAfterUpdateCalculated = false;
    }

    /// <summary>
    /// Determine the position of this object at the end of the current frame based on forces applied
    /// during this frame. Calculations for all object interactions are done with the objects in the state
    /// of the current frame.
    /// This is equivalent to the position the object would be in if undisturbed by any other objects.
    /// </summary>
    public Vector2 getNextPosition(float dt, Vector2 addForce) {

        // pos = 1/2 at^2 + vt + p. Since we are using a time step instead of cumulative time we need
        //to factor in the previous state then add our deltas within the last time step. 
        Vector2 vel = velocityLastFrame + (constAccel+addForce) * dt;
        Vector2 pos =  positionLastFrame + vel * dt; 

        velocityAfterUpdate = vel;
        positionAfterUpdate = pos;
        positionAfterUpdateCalculated = true;
        return positionAfterUpdate;
    }

    /// <summary>
    /// Check for objects between the current position and the targetPos. Currently this only looks at the 
    /// positions for this time step.
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
        //Debug.DrawRay(exMid, dir * maxDist, Color.cyan, 1.0f, false);

        hitInfo[1] = Physics2D.Raycast(ex1, dir, maxDist, physicsLayer.value);
        //Debug.DrawRay(ex1, dir * maxDist, Color.green, 1.0f, false);

        hitInfo[2] = Physics2D.Raycast(ex2, dir, maxDist, physicsLayer.value);
        //Debug.DrawRay(ex2, dir * maxDist, Color.red, 1.0f, false);

        //Debug.Log("Num results = " + results);
        GameObject goHit;
        for (int i = 0; i < 3; i++) {
        //for (int i = 0; i < results; i++) {
            if (hitInfo[i]) {
                if (collider.bounds.Intersects(hitInfo[i].collider.bounds)) {
                    //if (collider.IsTouching(hitInfo[i].collider)) { //only works with physics
                    //Debug.Log("myBounds=" + collider.bounds);
                    //Debug.Log("otherBounds=" + hitInfo[i].collider.bounds);
                    //Debug.Log("Hit some shit: [" + i + "] =>" + hitInfo[i].collider.gameObject.name);
                    goHit = hitInfo[i].collider.gameObject;
                    CustomPhysics gop = goHit.GetComponent<CustomPhysics>();
                    if (gop != null) {
                        Vector2 hitPt = collider.bounds.ClosestPoint(goHit.transform.position);
                        Vector2 n = (hitPt - (Vector2)goHit.transform.position).normalized;
                        Vector2 forceIn = velocityLastFrame * myMass; //TODO this is current V not V in the future when the collision happens
                        Vector2 myForceOut;
                        Vector2 otherForceOut;
                        float duration;
                        gop.curMat.collide(phys.curMat, hitPt, n, forceIn, out duration, out myForceOut, out otherForceOut);
                        myForceOut *= duration;
                        Debug.Log("MyForceOut=" + myForceOut);
                        //netAccelToApplyWc += myForceOut / myMass; // F = ma
                        netAccelToApplyWc += myForceOut ; // F = ma
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
        float d;
        phys.curMat.collide(otherPhys.curMat, Vector2.zero, surfaceNormal, force, out d, out forceOut, out myOut); 

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
