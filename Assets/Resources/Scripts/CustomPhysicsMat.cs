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
    /// IncomingForceOut is the force back in the direction of the initial impulse (i.e. the incoming object's new trajectory).
    /// ReceivingForceOut is the force on the other object
    /// </summary>
    /// <param name="other"></param>
    /// <param name="surfaceNormal"></param>
    /// <param name="forceIn"></param>
    /// <param name="incomingForceOut"></param>
    /// <param name="receivingForceOut"></param>
    public void collide(CustomPhysicsMat other, Vector2 surfaceNormal, Vector2 forceIn, out Vector2 incomingForceOut, out Vector2 receivingForceOut) {

        //According to fig newton, forces are applied equally, oppositely and tastefully.  Thus, the resulting output forces will be an
        //average of the properties of each input material as a function of the input force.  I have no idea if that is actually true 
        //since I just made all that up.

        Vector2 tangent = new Vector2(-surfaceNormal.y, surfaceNormal.x);
        float b = (bounce + other.bounce) / 2;
        float fr = (friction + other.friction) / 2;

        Vector2 forceInDir = forceIn.normalized;
        Vector2 forceAway = Vector2.Dot(forceInDir, surfaceNormal)* forceIn;
        Vector2 forceAlong = Vector2.Dot(forceInDir, tangent) * forceIn;

        incomingForceOut = forceAway * b - forceAlong * fr;
        receivingForceOut = new Vector2(incomingForceOut.y, incomingForceOut.x);
    }

}
