using UnityEngine;
using System.Collections;

public interface IAttackable {

    void ReceiveAttack(Vector2 direction, float impulseStrength, float jitterScale);
}
