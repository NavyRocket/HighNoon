using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobB_HealNode : ActionNode
{
    static float fixedY = 0.5f;

    public float range = 1f;

    protected override void OnStart()
    {
        Enemy[] enemies = GameInstance.Instance.mobPool.GetComponentsInChildren<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(GameInstance.Instance.playerController.transform.position, enemy.transform.position) > range)
                continue;

            if (enemy.enemy == ENEMY.A || enemy.enemy == ENEMY.B)
            {
                var vfx = GameInstance.Instance.healPool.Get();
                vfx.transform.position = new Vector3(enemy.transform.position.x, fixedY, 0f);
            }
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
