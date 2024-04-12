using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    int _current = 0;
    
    protected override void OnStart()
    {
        _current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Node child = children[_current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Success:
                ++_current;
                break;
            case State.Failure:
                return State.Failure;
        }

        return _current == children.Count ? State.Success : State.Running;
    }
}
