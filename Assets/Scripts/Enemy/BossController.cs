using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossController : MonoBehaviour
{
    static Color eyeColor = new Color(0.75f, 0.75f, 0.75f, 1f);
    static Vector2 phaseHeightScope = new Vector2(-1.2f, 1.4f);
    static float phaseMaxHeight = 100f;

    [SerializeField] private Material material;
    [SerializeField] private float phaseDuration = 1f;
    [SerializeField] private float phaseInitiateInterval = 0.25f;
    [SerializeField] private float InitiatePeakDuration = 0.5f;
    [SerializeField] private float InitiateFinalDuration = 3f;
    [SerializeField] private float InitiatePeakIntensity = 10f;
    [SerializeField] private float InitiateFinalIntensity = 3.5f;

    [SerializeField] private float floatHeight = 0f;
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatFrequency = 1f;

    [SerializeField] private LaserController laser;

    private bool hovering = true;
    public bool canFire = false;
    private bool wind = false;
    private float previousHeight = 0f;

    private float hp = 20f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        material.SetFloat("_Split_Value", -phaseMaxHeight);
        material.SetColor("_Emission_Color", Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        Hover();

        if (wind)
            rb.AddForce(new Vector3(-3f, 0f, 1f).normalized, ForceMode.Acceleration);
    }

    public void Damage(float damage)
    {
        if (damage <= 0f)
            return;

        hp -= damage;

        if (hp <= 0f)
            ThrowAway();
    }

    public void PhaseIn(float delay)
    {
        Invoke("PhaseIn", delay);
        VolumeManager.Instance.DoomEffect();
    }
    private void PhaseIn()
    {
        material.SetColor("_Emission_Color", eyeColor * 0f);
        StartCoroutine(PhaseHeight(phaseDuration));
        StartCoroutine(InitiateStep(phaseDuration + phaseInitiateInterval));
    }
    IEnumerator PhaseHeight(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            material.SetFloat("_Split_Value", Mathf.Lerp(transform.position.y + phaseHeightScope.x, transform.position.y + phaseHeightScope.y, timeAcc / duration));
            yield return null;
        }
        material.SetFloat("_Split_Value", phaseMaxHeight);
    }
    IEnumerator InitiateStep(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(InitiateLerp(0f, InitiatePeakIntensity, InitiatePeakDuration));
        yield return StartCoroutine(InitiateLerp(InitiatePeakIntensity, InitiateFinalIntensity, InitiateFinalDuration));
    }
    IEnumerator InitiateLerp(float startValue, float endValue, float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            material.SetColor("_Emission_Color", eyeColor * Mathf.Lerp(startValue, endValue, timeAcc / duration));
            yield return null;
        }
        material.SetColor("_Emission_Color", eyeColor * endValue);
    }

    private void Hover()
    {
        if (!hovering)
            return;

        floatHeight = floatAmplitude * Mathf.Sin(floatFrequency * Time.time);
        transform.position = new Vector3(transform.position.x, transform.position.y + floatHeight - previousHeight, transform.position.z);
        transform.LookAt(GameInstance.Instance.playerController.transform.position);
        previousHeight = floatHeight;
    }

    public void FireLaser()
    {
        canFire = true;
        StartCoroutine(IFireLaser());
    }
    IEnumerator IFireLaser()
    {
        while (canFire)
        {
            float randomCooldown = Random.Range(6f, 8f);
            yield return new WaitForSeconds(randomCooldown);
            laser.Fire();
        }
    }

    private void ThrowAway()
    {
        hovering = false;
        canFire = false;
        wind = true;

        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
