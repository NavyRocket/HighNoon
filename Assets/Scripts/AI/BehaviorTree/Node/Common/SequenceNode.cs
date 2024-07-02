using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    protected override void OnStart()
    {
        if (children.Count == 0)
            state = State.Failure;

        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (children.Count <= 0)
            return State.Failure;

        Node child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Success:
                ++current;
                break;
            case State.Failure:
                return State.Failure;
        }

        return current == children.Count ? State.Success : State.Running;
    }
}
