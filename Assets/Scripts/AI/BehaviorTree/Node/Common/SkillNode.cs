using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode : DecoratorNode
{
    public bool useManualSkillName = false;
    [HideInInspector] public string selectedSkillName;
    [HideInInspector] public string manualSkillName;

    private bool skillAvailable = false;

    protected override void OnStart()
    {
        if (blackboard.skillController.UseSkill(useManualSkillName ? manualSkillName : selectedSkillName))
        {
            skillAvailable = true;
        }
        else
        {
            skillAvailable = false;
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (skillAvailable)
        {
            return child.Update();
        }

        return State.Failure;
    }
}
