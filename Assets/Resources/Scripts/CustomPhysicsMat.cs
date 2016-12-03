using System;
using System.Collections.Generic;
using UnityEngine;

//TODO this doesn't need to be a monobehavior. make a scriptable object perhaps.
public class CustomPhysicsMat : MonoBehaviour {

    [SerializeField]
    private float bounce; //0 to 1. 1 is fully elastic.
    [SerializeField]
    private float friction; //0 to 1. 1 is super stop.
    [SerializeField]
    private float aeroCoeff;
    [SerializeField]
    private float bouyancy;
    [SerializeField]
    private float attract;
    [SerializeField]
    private float attractDist;
    [SerializeField]
    private float repel;
    [SerializeField]
    private float repelDist;

    /// <summary>
    /// Calculate the two force vectors that result when two objects collide.
    /// duration is the estimated duration of the contact
    /// IncomingForceOut is the force back in the direction of the initial impulse (i.e. the incoming object's new trajectory).
    /// ReceivingForceOut is the force on the other object
    /// </summary>
    /// <param name="other"></param>
    /// <param name="surfaceNormal"></param>
    /// <param name="forceIn"></param>
    /// <param name="incomingForceOut"></param>
    /// <param name="receivingForceOut"></param>
    public void collide(CustomPhysicsMat other, Vector2 hitPt, Vector2 surfaceNormal, Vector2 forceIn, out float duration, out Vector2 incomingForceOut, out Vector2 receivingForceOut) {

        //According to fig newton, forces are applied equally, oppositely and tastefully.  Thus, the resulting output forces will be an
        //average of the properties of each input material as a function of the input force.  I have no idea if that is actually true 
        //since I just made all that up.

        Vector2 tangent = new Vector2(-surfaceNormal.y, surfaceNormal.x);
        Debug.Log("Impact normal=" + surfaceNormal + ", tangent=" + tangent);
        Debug.Break();
        float b = (bounce + other.bounce) / 2;
        float fr = (friction + other.friction) / 2;

        float forceInAmt = forceIn.magnitude;
        Vector2 forceInDir = forceIn / forceInAmt;
        Vector2 forceOutDir = Vector2.Reflect(forceInDir, surfaceNormal);
        float fDotn = Vector2.Dot(forceInDir, surfaceNormal);
        Vector2 forceAway =  fDotn * forceInAmt * forceOutDir;
        Vector2 forceAlong = Vector2.Dot(forceInDir, tangent) * forceIn;

        Vector2 totalForceOut = forceAway * b - forceAlong * fr;
        //TODO outgoing force should be capped to not allow the object to fall through?
        Debug.Log("incomingForce=" + forceIn + ", amt=" + forceInAmt);
        Debug.Log("forceOutDir=" + forceOutDir + ", fullAmt=" + forceAway);
        Debug.Log("incomingForceOut=" + totalForceOut);
        float EPS2 = 0.45f; //wiggle room
        Vector2 zeroForce = -fDotn * (forceInAmt+EPS2) * surfaceNormal; //need to have this output otherwise collision will pass through
        Debug.Log("zeroForce=" + zeroForce);
        float xOut = Math.Abs(totalForceOut.x) > Math.Abs(zeroForce.x) ? totalForceOut.x : zeroForce.x;
        float yOut = Math.Abs(totalForceOut.y) > Math.Abs(zeroForce.y) ? totalForceOut.y : zeroForce.y;
        incomingForceOut = new Vector2(xOut, yOut);
        receivingForceOut = Vector2.zero; //TODO
        //TODO duration based on speed, angle, how soft the object is.
        duration = 0.75f;
    }

}
