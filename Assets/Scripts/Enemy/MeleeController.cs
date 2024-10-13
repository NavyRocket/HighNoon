using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public Vector3 vfxOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
    //  if (other.CompareTag("Player") && GameInstance.Instance.playerController.canTakeDamage)
    //  {
    //      var vfx = GameInstance.Instance.hitPlayerPool.Get();
    //      if (null == vfx.GetComponent<PartycleSystemDisactivate>())
    //          vfx.AddComponent<PartycleSystemDisactivate>();
    //      vfx.transform.position = other.transform.position + vfxOffset;
    //      vfx.gameObject.SetActive(true);
    //  }
    }
}
