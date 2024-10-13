using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    static Vector2 phaseHeightScope = new Vector2(-1.2f, 1.4f);
    static float phaseMaxHeight = 100f;

    [SerializeField] private Material material;
    [SerializeField] private float phaseDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_Split_Value", -phaseMaxHeight);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GameInstance.Instance.playerController.transform.position);
    }

    public void PhaseIn()
    {
        StartCoroutine(PhaseHeight(phaseDuration));
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
}
