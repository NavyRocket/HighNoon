using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCargoController : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] Transform evtTrigger;

    private bool bossSceneTrain = false;

    // Start is called before the first frame update
    void Start()
    {
        bossSceneTrain = GameInstance.Instance.bossSceneReady && !GameInstance.Instance.bossPeak;
        if (bossSceneTrain)
        {
            door.gameObject.SetActive(false);
            evtTrigger.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
