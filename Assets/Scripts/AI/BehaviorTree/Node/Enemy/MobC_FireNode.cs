using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobC_FireNode : ActionNode
{
    static float fireY = 1.4f;

    public float delay = 0f;

    Enemy enemy;
    MobC_FireballController fireball;

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

        if (enemy == null)
        {
            enemy = blackboard.owner.GetComponent<Enemy>();
            fireball = Instantiate(enemy.mobC_FireballPrefab, enemy.poolObject).GetComponent<MobC_FireballController>();
        }
        else
        {
            fireball.gameObject.SetActive(true);
        }

        bool faceLeft = blackboard.target.transform.position.x <= blackboard.owner.transform.position.x;
        fireball.Fire(new Vector3(blackboard.owner.transform.position.x, fireY, blackboard.owner.transform.position.z), faceLeft ? Vector3.left : Vector3.right);
    }
}
