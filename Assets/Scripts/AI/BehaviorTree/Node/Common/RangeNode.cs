using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RangeNode : DecoratorNode
{
    public float range = 0f;
    public bool inRange = false;
    public bool checkOnce = false;

    bool success = false;

    protected override void OnStart()
    {
        float distance = (blackboard.owner.transform.position - blackboard.target.transform.position).magnitude;
        success = inRange ? range < distance : range >= distance;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!checkOnce)
        {
            float distance = (blackboard.owner.transform.position - blackboard.target.transform.position).magnitude;
            success = inRange ? range < distance : range >= distance;
        }

        if (success)
        {
            return child.Update();
        }

        return State.Failure;
    }
}
