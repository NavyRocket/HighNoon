using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEngineController : MonoBehaviour
{
    [SerializeField] CouplerController coupler;
    [SerializeField] float forwardSpeed = 1f;

    [HideInInspector] public bool sendForward = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sendForward)
            transform.Translate(Vector3.right * Time.deltaTime * forwardSpeed);
    }
}
