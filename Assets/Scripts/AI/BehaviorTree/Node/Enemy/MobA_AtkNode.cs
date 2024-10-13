using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobA_AtkNode : ActionNode
{
    static float fixedY = 0.5f;
    public float delay = 0f;

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

        var vfx = GameInstance.Instance.mobB_atkPool.Get();
        if (null == vfx.GetComponent<PartycleSystemDisactivate>())
            vfx.AddComponent<PartycleSystemDisactivate>();

        var ftp = vfx.GetComponent<FixToPlayer>();
        if (ftp == null)
        {
            ftp = vfx.AddComponent<FixToPlayer>();
            ftp.x = true;
            ftp.y = false;
            ftp.z = false;
            ftp.useLifetime = true;
            ftp.lifetime = 0.9f;
        }
        ftp.Reset();
        vfx.transform.position = new Vector3(GameInstance.Instance.playerController.transform.position.x, fixedY, 0f);
        vfx.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        vfx.transform.localScale = new Vector3(0.4f, 0.6f, 0.2f);
        vfx.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.95f);
        GameInstance.Instance.playerController.Damage(1f);
    }
}
