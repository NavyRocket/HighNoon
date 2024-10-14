using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 offset;
    [SerializeField] float smoothSpeed = 1f;

    private Vector3 _initialShakePos = Vector3.zero;
    private Vector3 _magnitude = Vector3.one;
    private float _fixedZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameInstance.Instance.GetPlayerController().transform;
        offset = GameInstance.Instance.GetPlayerController().transform.position - transform.position;
        _fixedZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y - offset.y, _fixedZ);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void Shake(float duration, Vector3 magnitude)
    {
        _initialShakePos = transform.localPosition;
        _magnitude = magnitude;
        StartCoroutine(Shake(duration));
    }

    public IEnumerator Shake(float duration)
    {
        float timer = 0;
        while (timer <= duration)
        {
            Vector3 random = new Vector3(Random.Range(-1f, 1f) * _magnitude.x, Random.Range(-1f, 1f) * _magnitude.y, Random.Range(-1f, 1f) * _magnitude.z);
            transform.localPosition = _initialShakePos + random;;
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _initialShakePos;
    }

    public void SetTargetToPlayer(float delay)
    {
        Invoke("SetTarget", delay);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetTarget()
    {
        target = GameInstance.Instance.playerController.transform;
        GameInstance.Instance.playerController.DefaultWalking();
    }
}
