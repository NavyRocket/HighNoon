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
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI uiText;

    private bool isWriting = false;
    private Coroutine currentCoroutine;
    private Coroutine currentNestedCoroutine;
    private Coroutine currentNestedNestedCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextTypeWriter(string input)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, maxSpeechAlpha);
        uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, maxSpeechAlpha);

        if (isWriting)
        {
            StopCoroutine(currentCoroutine);
            StopCoroutine(currentNestedCoroutine);
            StopCoroutine(currentNestedNestedCoroutine);
        }
        currentCoroutine = StartCoroutine(ITextTypeWriter(input));
    }
    IEnumerator ITextTypeWriter(string input)
    {
        isWriting = true;

        uiText.text = "";
        for (int i = 0; i <= input.Length; ++i)
        {
            uiText.text = input.Substring(0, i);
            yield return new WaitForSeconds(typingInterval);
        }

        isWriting = false;
        currentNestedCoroutine = StartCoroutine(CloseSpeechAfter(displayAfter));
    }
    IEnumerator CloseSpeechAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentNestedNestedCoroutine = StartCoroutine(FadeSpeech(0.5f));
    }
    IEnumerator FadeSpeech(float duration)
    {
        float timeAcc = 0f;
        float rate = maxSpeechAlpha;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            rate = (1f - timeAcc / duration) * maxSpeechAlpha;
            image.color = new Color(image.color.r, image.color.g, image.color.b, rate);
            uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, rate);
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, 0f);
    }

}
