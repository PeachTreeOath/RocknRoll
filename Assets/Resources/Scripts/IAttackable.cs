using UnityEngine;
using System.Collections;

public interface IAttackable {

    void ReceiveForce(CustomPhysics phys,  Vector2 surfaceNormal, Vector2 forceDirection, float forceStrength, out Vector2 forceOut);

    void ReceiveDragChange(float drag);

    void ReceiveVacuum(Vector2 source, float strength);

    void ReceiveDmg(float amt);
}
