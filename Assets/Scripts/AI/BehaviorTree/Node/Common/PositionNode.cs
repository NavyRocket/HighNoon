using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNode : ActionNode
{
    public Vector3 setPosition = Vector3.zero;

    protected override void OnStart()
    {
        blackboard.owner.transform.position = setPosition;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
