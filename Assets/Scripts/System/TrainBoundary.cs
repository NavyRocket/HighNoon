using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBoundary : MonoBehaviour
{
    private float boundaryCenter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        boundaryCenter = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if (other.transform.position.x < boundaryCenter)
        {
            GameInstance.Instance.EnterTrainRight();
        }
        else
        {
            GameInstance.Instance.EnterTrainLeft();
        }
    }
}
