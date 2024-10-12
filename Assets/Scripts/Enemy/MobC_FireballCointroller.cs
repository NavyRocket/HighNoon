using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobC_FireballController : MonoBehaviour
{
    [SerializeField] private float force = 1f;
    [SerializeField] private float lifeTime = 1f;
    private float timeAcc = 0f;

    public Vector3 initialScale = Vector3.one;
    public Vector3 finalScale = Vector3.one;

    private Rigidbody rb;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = initialScale;
    }

    private void Update()
    {
        timeAcc += Time.deltaTime;
        if (timeAcc >= lifeTime)
        {
            timeAcc = 0f;
            gameObject.SetActive(false);
        }

        float t = timeAcc / lifeTime;
        transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
        sr.color = new Color(1f, 1f, 1f, EasingFunction.EaseInQuad(1f, 0f, timeAcc / lifeTime));
    }

    private void OnEnable()
    {
        timeAcc = 0f;
        transform.localScale = initialScale;
    }

    public void Fire(Vector3 position, Vector3 direction)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = position;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
