using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    [HideInInspector] public Node child;

    protected override void OnStart()
    {
        if (blackboard == null) Debug.Log("Blackboard is null");
        if (blackboard.animator == null) Debug.Log("Animator is null");
        blackboard.animator.speed = 2f;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return child.Update();
    }

    public override Node Clone()
    {
        RootNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
