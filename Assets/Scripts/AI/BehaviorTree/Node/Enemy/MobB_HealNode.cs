using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobB_HealNode : ActionNode
{
    static float fixedY = 0.5f;
    public float delay = 0f;
    public float range = 1f;
    public float healAmount = 1f;

    protected override void OnStart()
    {
        GameInstance.Instance.StartCoroutine(ExecuteAfterDelay());
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return State.Success;
    }

    private IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        Enemy[] enemies = GameInstance.Instance.mobPool.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(GameInstance.Instance.playerController.transform.position, enemy.transform.position) > range)
                continue;

            if (enemy.enemy == ENEMY.A || enemy.enemy == ENEMY.C)
            {
                var vfx = GameInstance.Instance.mobB_healPool.Get();
                if (null == vfx.GetComponent<PartycleSystemDisactivate>())
                    vfx.AddComponent<PartycleSystemDisactivate>();
                vfx.transform.position = new Vector3(enemy.transform.position.x, fixedY, 0f);
                vfx.transform.rotation = Quaternion.Euler(270f, 0, 0);
                vfx.transform.localScale = Vector3.one * 0.75f;
                vfx.gameObject.SetActive(true);

                enemy.Heal(healAmount);
            }
        }
    }
}
