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
        if (Input.GetKeyDown(KeyCode.N))
            CloseEye();
        if (Input.GetKeyDown(KeyCode.M))
            OpenEye();
    }

    public void CloseEye()
    {
        StartCoroutine(ICloseEye(closeEyeDuration));
    }
    IEnumerator ICloseEye(float duration)
    {
        float timeAcc = 0f;

        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            vig.intensity.value = EasingFunction.EaseOutSine(vigEye.x, vigEye.y, timeAcc / duration);
            col.postExposure.value = EasingFunction.EaseOutSine(colEye.x, colEye.y, timeAcc / duration);
            yield return null;
        }

        vig.intensity.value = vigEye.y;
        col.postExposure.value = colEye.y;
    }

    public void OpenEye()
    {
        StartCoroutine(IOpenEye(openEyeDuration));
    }
    IEnumerator IOpenEye(float duration)
    {
        float timeAcc = 0f;

        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            vig.intensity.value = EasingFunction.EaseOutSine(vigEye.y, vigEye.x, timeAcc / duration);
            col.postExposure.value = EasingFunction.EaseOutSine(colEye.y, colEye.x, timeAcc / duration);
            yield return null;
        }

        vig.intensity.value = vigEye.x;
        col.postExposure.value = colEye.x;
    }
}
