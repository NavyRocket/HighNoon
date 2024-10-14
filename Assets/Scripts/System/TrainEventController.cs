using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEventController : MonoBehaviour
{
    private static bool isBossPrepared = false;
    private static bool isOutsidePrepared = false;
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        if (!other.gameObject.CompareTag("Player"))
            return;
        if (other.transform.eulerAngles.y == 180f)
            return;

        if (!isBossPrepared)
        {
            isBossPrepared = true;
            GameInstance.Instance.PrepareBoss();
        }
        else if (!isOutsidePrepared)
        {
            isOutsidePrepared = true;
            GameInstance.Instance.PrepareOutside();
            ShiftPlayerZ();
        }

        triggered = true;
    }

    static public void Reset()
    {
        isBossPrepared = false;
        isOutsidePrepared = false;
    }

    private void ShiftPlayerZ()
    {
        StartCoroutine(TranslatePlayerZ(GameInstance.Instance.playerController.transform.position.z + 0.8f, 1.5f));
    }
    IEnumerator TranslatePlayerZ(float destination, float duration)
    {
        float original = GameInstance.Instance.playerController.transform.position.z;
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            float ratio = timeAcc / duration;
            GameInstance.Instance.playerController.transform.position = new Vector3(
                GameInstance.Instance.playerController.transform.position.x,
                GameInstance.Instance.playerController.transform.position.y,
                EasingFunction.EaseInOutSine(original, destination, ratio));
            yield return null;
        }
        GameInstance.Instance.playerController.transform.position = new Vector3(
            GameInstance.Instance.playerController.transform.position.x,
            GameInstance.Instance.playerController.transform.position.y,
            0.8f);
    }
}
