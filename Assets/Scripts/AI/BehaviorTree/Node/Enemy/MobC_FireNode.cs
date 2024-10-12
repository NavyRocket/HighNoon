using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobC_FireNode : ActionNode
{
    Enemy enemy;
    MobC_FireballController fireball;

    protected override void OnStart()
    {
        if (enemy == null)
        {
            enemy = blackboard.owner.GetComponent<Enemy>();
            fireball = Instantiate(enemy.mobC_FireballPrefab, enemy.poolObject).GetComponent<MobC_FireballController>();
        }
        else
        {
            fireball.gameObject.SetActive(true);
        }

        bool faceLeft = blackboard.target.transform.position.x <= blackboard.owner.transform.position.x;
        fireball.Fire(blackboard.owner.transform.position, faceLeft ? Vector3.left : Vector3.right);
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
