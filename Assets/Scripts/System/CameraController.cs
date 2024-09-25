using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 offset;
    [SerializeField] float smoothSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameInstance.Instance.GetPlayerController().transform;
        offset = GameInstance.Instance.GetPlayerController().transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y - offset.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
