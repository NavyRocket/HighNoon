using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform; 
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f; // Èçµé¸² °­µµ
    public float shakeInterval = 120f; // Èçµé¸² °£°Ý

    private Vector3 initialCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        initialCameraPosition = cameraTransform.position;

        // Èçµé¸²
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        while (true)
        {
            // 2ºÐ ´ë±â
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
