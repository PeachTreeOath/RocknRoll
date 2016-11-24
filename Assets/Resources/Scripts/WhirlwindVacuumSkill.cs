using UnityEngine;
using System.Collections;

public class WhirlwindVacuumSkill : WhirlwindSkill
{

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        foreach (GameObject attackedObj in collidedList)
        {
            // Create a random source every frame to give the whirlwind some randomness to its vacuum
            Vector2 source = (Vector2)transform.position + Random.insideUnitCircle * 2;
            attackedObj.GetComponent<IAttackable>().ReceiveVacuum(source, 0.1f);
        }
    }

}
