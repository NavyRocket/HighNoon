using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] float initLine = 10000f;
    [SerializeField] float line = 100000f;
    [SerializeField] float bounce = 0f;
    [SerializeField] float normal = 20f;

    [SerializeField] Material material;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float thickness = 0.5f;

    [SerializeField] float narrowDuration = 1f;
    [SerializeField] float interval = 1f;
    [SerializeField] float bounceDuration = 0.05f;
    [SerializeField] float secondInterval = 1f;
    [SerializeField] float boostDuration = 0.5f;

    [SerializeField] float thicknessA = 0.1f;
    [SerializeField] float thicknessB = 0.1f;
    [SerializeField] float thicknessC = 0.1f;
    [SerializeField] float thicknessD = 0.1f;

    private bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Fire();

        lineRenderer.endWidth = thickness;
        lineRenderer.startWidth = thickness;
    }

    public void Fire()
    {
        StartCoroutine(FireLaser());
    }
    IEnumerator FireLaser()
    {
        StartCoroutine(INarrow(narrowDuration));
        yield return new WaitForSeconds(narrowDuration + interval);
        StartCoroutine(IBounce(bounceDuration));
        yield return new WaitForSeconds(narrowDuration + interval + bounceDuration + secondInterval);
        StartCoroutine(IBoost(boostDuration));
    }

    IEnumerator INarrow(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            SetSizeFlow(Mathf.Lerp(line, bounce, timeAcc / duration), 0f, Mathf.Lerp(thicknessA, thicknessB, timeAcc / duration));
            yield return null;
        }
        SetSizeFlow(line, 0f, thicknessB);
    }
    IEnumerator IBounce(float duration)
    {
        canAttack = true;
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            SetSizeFlow(Mathf.Lerp(line, bounce, timeAcc / duration), Mathf.Lerp(0f, 1f, timeAcc / duration), Mathf.Lerp(thicknessB, thicknessC, timeAcc / duration));
            yield return null;
        }
        SetSizeFlow(bounce, 1f, thicknessC);
        canAttack = false;
    }
    IEnumerator IBoost(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            SetSizeFlow(Mathf.Lerp(bounce, normal, timeAcc / duration), 1f, Mathf.Lerp(thicknessC, thicknessD, timeAcc / duration));
            yield return null;
        }
        SetSizeFlow(normal, 1f, thicknessD);
    }

    void SetSizeFlow(float size, float flow, float thickness)
    {
        material.SetFloat("_Laser_Size", size);
        material.SetFloat("_Flow", flow);
        this.thickness = thickness;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canAttack)
            return;
        if (!other.CompareTag("Player"))
            return;

        GameInstance.Instance.playerController.Damage(2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!canAttack)
            return;
        if (!other.CompareTag("Player"))
            return;

        GameInstance.Instance.playerController.Damage(2f);
    }
}
