using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform; 
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f; // ��鸲 ����
    public float shakeInterval = 120f; // ��鸲 ����

    private Vector3 initialCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        initialCameraPosition = cameraTransform.position;

        // ��鸲
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        while (true)
        {
            // 2�� ���
            yield return new WaitForSeconds(shakeInterval);


            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        Vector3 currentCameraPosition = cameraTransform.position;

        while (elapsedTime < shakeDuration)
        {

            Vector3 randomPoint = currentCameraPosition + Random.insideUnitSphere * shakeMagnitude;
            cameraTransform.position = new Vector3(randomPoint.x, randomPoint.y, currentCameraPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        cameraTransform.position = currentCameraPosition;
    }
}
