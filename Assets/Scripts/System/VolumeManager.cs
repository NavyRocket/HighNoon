using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : SingletonMonoBehaviour<VolumeManager>
{
    Volume volume = null;
    Vignette vig = null;
    DepthOfField dof = null;
    ColorAdjustments col = null;
    PaniniProjection pnn = null;
    ChromaticAberration chr = null;

    static private Vector2 vigEye = new Vector2(0.25f, 1f);
    static private Vector2 colEye = new Vector2(0.2f, -10f);

    [SerializeField] private float closeEyeDuration = 1f;
    [SerializeField] private float openEyeDuration = 0.5f;
    [SerializeField] private float doomIncreaseDuration = 0.5f;
    [SerializeField] private float doomInterval = 0.5f;
    [SerializeField] private float doomDecreaseDuration = 0.5f;

    private Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vig);
        volume.profile.TryGet<DepthOfField>(out dof);
        volume.profile.TryGet<ColorAdjustments>(out col);
        volume.profile.TryGet<PaniniProjection>(out pnn);
        volume.profile.TryGet<ChromaticAberration>(out chr);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseEye()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ICloseEye(closeEyeDuration));
    }
    IEnumerator ICloseEye(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            GameInstance.Instance.heartPanelCG.alpha = EasingFunction.EaseOutSine(1f, 0f, timeAcc / duration);
            vig.intensity.value = EasingFunction.EaseOutSine(vigEye.x, vigEye.y, timeAcc / duration);
            col.postExposure.value = EasingFunction.EaseOutSine(colEye.x, colEye.y, timeAcc / duration);
            yield return null;
        }

        GameInstance.Instance.heartPanelCG.alpha = 0f;
        vig.intensity.value = vigEye.y;
        col.postExposure.value = colEye.y;
    }

    public void OpenEye()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(IOpenEye(openEyeDuration));
    }
    IEnumerator IOpenEye(float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            GameInstance.Instance.heartPanelCG.alpha = EasingFunction.EaseOutSine(0, 1f, timeAcc / duration);
            vig.intensity.value = EasingFunction.EaseOutSine(vigEye.y, vigEye.x, timeAcc / duration);
            col.postExposure.value = EasingFunction.EaseOutSine(colEye.y, colEye.x, timeAcc / duration);
            yield return null;
        }

        GameInstance.Instance.heartPanelCG.alpha = 1f;
        vig.intensity.value = vigEye.x;
        col.postExposure.value = colEye.x;
    }

    private void SetScreenAlpha()
    {

    }

    public void DoomEffect()
    {
        StartCoroutine(DoomStep(doomInterval));
    }
    IEnumerator DoomStep(float delayBetween)
    {
        yield return StartCoroutine(DoomLerp(0f, 1f, doomIncreaseDuration));
        yield return new WaitForSeconds(delayBetween);
        yield return StartCoroutine(DoomLerp(1f, 0f, doomDecreaseDuration));
    }
    IEnumerator DoomLerp(float startValue, float endValue, float duration)
    {
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            pnn.distance.value = Mathf.Lerp(startValue, endValue, timeAcc / duration);
            chr.intensity.value = Mathf.Lerp(startValue, endValue, timeAcc / duration);
            yield return null;
        }
        pnn.distance.value = endValue;
        chr.intensity.value = endValue;
    }

}
