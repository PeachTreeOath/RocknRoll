using UnityEngine;
using System.Collections;

public interface IAttackable {

    void ReceiveForce(Vector2 direction, float forceStrength, float jitterScale);

    void ReceiveDragChange(float drag);

    void ReceiveVacuum(Vector2 source, float strength);
}
