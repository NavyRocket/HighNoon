using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixToTransform : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = Vector3.zero;

    private Camera mainCamera;
    private RectTransform uiElement;

    void Start()
    {
        uiElement = GetComponent<RectTransform>();
        mainCamera = GameInstance.Instance.cameraController.GetComponent<Camera>();
    }

    void Update()
    {
        if (target == null || uiElement == null || mainCamera == null)
            return;

        uiElement.position = mainCamera.WorldToScreenPoint(target.position + offset);
    }
}
