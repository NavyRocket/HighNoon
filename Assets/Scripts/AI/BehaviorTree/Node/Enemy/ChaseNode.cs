using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class ChaseNode : ActionNode
{
    public float speed = 1f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float f = blackboard.target.transform.position.x - blackboard.owner.transform.position.x <= 0f ? -1f : 1f;
        blackboard.rb.AddForce(f * speed * Time.deltaTime, 0f, 0f);

        return State.Running;
    }
}
