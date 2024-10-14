using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class RebirthMenu : MonoBehaviour
{
    [SerializeField] SlotController[] slots;
    [SerializeField] CanvasGroup canvasGroup;

    private Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().speed = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            RandomizeSlot();
        }
        else
        {
            GetComponent<Animator>().Play("Hide");
            GameInstance.Instance.ResetLevel();
            GameInstance.Instance.playerController.Rebirth();
        }
    }
    IEnumerator FadeInAlpha(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            canvasGroup.alpha = EasingFunction.EaseOutCirc(0, 1f, timeAcc / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    IEnumerator FadeOutAlpha(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            canvasGroup.alpha = EasingFunction.EaseOutCirc(1f, 0f, timeAcc / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }


    void RandomizeSlot()
    {
        foreach (SlotController slot in slots)
        {
            slot.SetRandomRebirthType();
        }
    }
}
