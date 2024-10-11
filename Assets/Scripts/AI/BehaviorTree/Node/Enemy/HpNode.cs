using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class HpNode : DecoratorNode
{
    public float hpLimit = 0;
    public bool isHpLowCheck = true;
    public bool includeLimitValue = true;
    private bool success = false;

    protected override void OnStart()
    {
        success = false;

        float hp = blackboard.Get<float>("Hp");

        success = includeLimitValue ?
            isHpLowCheck ? hp <= hpLimit : hp >= hpLimit :
            isHpLowCheck ? hp < hpLimit : hp > hpLimit;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return success ? child.Update() : State.Failure;
    }
}
