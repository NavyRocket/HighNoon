using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
    private Coroutine currentCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TimeScaleLerp(float targetScale, float duration, float lerpDuration, float delay)
    {
        if (null != currentCoroutine)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ITimeScaleLerp(targetScale, duration, lerpDuration, delay));
    }
    public void TimeScaleEaseInSine(float targetScale, float duration, float easeDuration, float delay)
    {
        if (null != currentCoroutine)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ITimeScaleEaseInSine(targetScale, duration, easeDuration, delay));
    }
    public void TimeScaleEaseOutSine(float targetScale, float duration, float easeDuration, float delay)
    {
        if (null != currentCoroutine)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ITimeScaleEaseOutSine(targetScale, duration, easeDuration, delay));
    }

    IEnumerator ITimeScaleLerp(float targetScale, float duration, float lerpDuration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        float originalScale = Time.timeScale;

        float timeAcc = 0f;
        while (timeAcc < lerpDuration)
        {
            timeAcc += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(originalScale, targetScale, timeAcc / lerpDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = targetScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        timeAcc = 0f;
        while (timeAcc < lerpDuration)
        {
            timeAcc += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(targetScale, originalScale, timeAcc / lerpDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = originalScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    IEnumerator ITimeScaleEaseInSine(float targetScale, float duration, float easeDuration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        float originalScale = Time.timeScale;

        float timeAcc = 0f;
        while (timeAcc < easeDuration)
        {
            timeAcc += Time.unscaledDeltaTime;
            Time.timeScale = EasingFunction.EaseInSine(originalScale, targetScale, timeAcc / easeDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = targetScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        timeAcc = 0f;
        while (timeAcc < easeDuration)
        {
            timeAcc += Time.unscaledDeltaTime;
            Time.timeScale = EasingFunction.EaseInSine(targetScale, originalScale, timeAcc / easeDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = originalScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    IEnumerator ITimeScaleEaseOutSine(float targetScale, float duration, float easeDuration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        float originalScale = Time.timeScale;

        float timeAcc = 0f;
        while (timeAcc < easeDuration)
        {
            timeAcc += Time.unscaledDeltaTime;
            Time.timeScale = EasingFunction.EaseOutSine(originalScale, targetScale, timeAcc / easeDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = targetScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        timeAcc = 0f;
        while (timeAcc < easeDuration)
        {
            timeAcc += Time.unscaledDeltaTime;
            Time.timeScale = EasingFunction.EaseOutSine(targetScale, originalScale, timeAcc / easeDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = originalScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
