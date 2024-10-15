using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    static float maxSpeechAlpha = 0.8f;

    [SerializeField] float typingInterval = 0.1f;
    [SerializeField] float displayAfter = 1f;
    [SerializeField] TextMeshProUGUI uiText;
    private CanvasGroup canvasGroup;

    private bool isSpeaking = false;
    private Coroutine currentCoroutine;
    private Coroutine currentNestedCoroutine;
    private Coroutine currentNestedNestedCoroutine;
    private Coroutine currentNestedNestedNestedCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Speak(float after, string input)
    {
        currentCoroutine = StartCoroutine(SpeakDelay(after, input));
    }
    IEnumerator SpeakDelay(float delay, string input)
    {
        yield return new WaitForSeconds(delay);
        Speak(input);
    }
    private void Speak(string input)
    {
        SetSpeechAlpha(maxSpeechAlpha);
        if (isSpeaking)
        {
            if (null != currentNestedCoroutine)
              StopCoroutine(currentNestedCoroutine);
            if (null != currentNestedNestedCoroutine)
                StopCoroutine(currentNestedNestedCoroutine);
            if (null != currentNestedNestedNestedCoroutine)
                StopCoroutine(currentNestedNestedNestedCoroutine);
        }
        currentNestedCoroutine = StartCoroutine(ITextTypeWriter(input));
    }
    IEnumerator ITextTypeWriter(string input)
    {
        isSpeaking = true;
        uiText.text = "";
        for (int i = 0; i <= input.Length; ++i)
        {
            uiText.text = input.Substring(0, i);
            yield return new WaitForSeconds(typingInterval);
        }
        currentNestedNestedCoroutine = StartCoroutine(CloseSpeechAfter(displayAfter));
    }
    IEnumerator CloseSpeechAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentNestedNestedNestedCoroutine = StartCoroutine(FadeSpeech(0.5f));
    }
    IEnumerator FadeSpeech(float duration)
    {
        float timeAcc = 0f;
        float rate = maxSpeechAlpha;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            rate = (1f - timeAcc / duration) * maxSpeechAlpha;
            SetSpeechAlpha(rate);
            yield return null;
        }
        SetSpeechAlpha(0f);
        isSpeaking = false;
    }

    private void SetSpeechAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
}

/*
public class SpeechController : MonoBehaviour
{
    static float maxSpeechAlpha = 0.8f;

    [SerializeField] float typingInterval = 0.1f;
    [SerializeField] float displayAfter = 1f;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI uiText;

    private Coroutine currentSpeakCoroutine;
    private Coroutine currentSpeakCoroutine2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Speak(float after, string input)
    {
        if (currentSpeakCoroutine != null)
        {
            StopCoroutine(currentSpeakCoroutine);
            StopCoroutine(currentSpeakCoroutine2);
        }
        currentSpeakCoroutine = StartCoroutine(SpeakAfter(after, input));
    }

    IEnumerator SpeakAfter(float delay, string input)
    {
        yield return new WaitForSeconds(delay);
        if (currentSpeakCoroutine2 != null)
        {
            StopCoroutine(currentSpeakCoroutine2);
        }
        currentSpeakCoroutine2 = StartCoroutine(SpeakRoutine(input));
    }

    IEnumerator SpeakRoutine(string input)
    {
        SetSpeechAlpha(maxSpeechAlpha);

        yield return StartCoroutine(ITextTypeWriter(input));

        yield return new WaitForSeconds(displayAfter);
        yield return StartCoroutine(FadeSpeech(0.5f));
    }

    IEnumerator ITextTypeWriter(string input)
    {
        uiText.text = "";
        for (int i = 0; i <= input.Length; ++i)
        {
            uiText.text = input.Substring(0, i);
            yield return new WaitForSeconds(typingInterval);
        }
    }

    IEnumerator FadeSpeech(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            float rate = (1f - timeAcc / duration) * maxSpeechAlpha;
            SetSpeechAlpha(rate);
            yield return null;
        }
        SetSpeechAlpha(0f);
    }

    private void SetSpeechAlpha(float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, alpha);
    }
}
*/
