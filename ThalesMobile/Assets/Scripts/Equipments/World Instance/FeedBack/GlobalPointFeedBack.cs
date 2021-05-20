using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweek.FlagAttributes;
using UnityEngine.Audio;

public class GlobalPointFeedBack : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    GlobalDetectionPoint source;

    [Header("Visual Params")]
    [SerializeField]
    Image revealIconImage;
    [SerializeField]
    Image revealPointerImage;
    [SerializeField]
    Image dotImage;
    [SerializeField]
    CanvasGroup globalVisualCanvas;

    [Header("Reveal Params")]
    [SerializeField]
    RectTransform revealCanvasRT;
    [SerializeField]
    Transform tempLookCamTransform;
    CameraController camController;
    public float animDuration;
    public float revealKeepDuration;
    [SerializeField]
    AnimationCurve revealScaleProgression;
    Vector3 revealBaseScale;

    [Header("Expiration Params")]
    public Color freshColor;
    public Color nearExpiredColor;
    public Color expiredColor;

    [Header("Sound - Appear")]
    [SerializeField]
    AudioMixerGroup targetGroup;
    [SerializeField, TweekFlag(FieldUsage.Sound)]
    AudioClip appearSound;
    [SerializeField, TweekFlag(FieldUsage.Sound)]
    float appearSoundVolume;
    [SerializeField]
    AudioSource appearSoundSource;

    private void Start()
    {
        revealCanvasRT.localScale = Vector3.zero;
        camController = GameManager.Instance.cameraController;
        revealBaseScale = revealCanvasRT.localScale;
    }

    private void Update()
    {
        revealCanvasRT.forward = camController.cam.transform.forward;
        revealCanvasRT.localScale = revealBaseScale * revealScaleProgression.Evaluate(camController.zoomIntensity / 1);
    }

    public void OnEnable()
    {
        appearSoundSource.volume = Mathf.Clamp01(appearSoundVolume);
        GameManager.Instance.soundHandler.PlaySound(appearSound, appearSoundSource, targetGroup);
    }

    public void DisplayReveal(Sprite revealIcon, Sprite revealPointer)
    {
        revealIconImage.sprite = revealIcon;
        revealPointerImage.sprite = revealPointer;

        StartCoroutine(Scale(Vector3.one, animDuration, revealCanvasRT));
    }

    public void HideReveal()
    {
        StartCoroutine(Scale(Vector3.zero, animDuration, revealCanvasRT));
    }

    public void UpdateColor(ExpirationValue targetExpiration)
    {
        switch (targetExpiration)
        {
            case ExpirationValue.Fresh:
                dotImage.color = freshColor;
                break;
            case ExpirationValue.NearExpiration:
                dotImage.color = nearExpiredColor;
                break;
            case ExpirationValue.Expired:
                dotImage.color = expiredColor;
                break;
        }
    }

    IEnumerator Scale(Vector3 target, float duration, RectTransform obj)
    {
        Vector3 start = obj.localScale;
        float timer = 0;

        while (timer < duration)
        {
            obj.localScale = Vector3.Lerp(start, target, timer / duration);
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }

        yield return new WaitForSeconds(revealKeepDuration);
        StartCoroutine(Scale(Vector3.zero, animDuration, revealCanvasRT));
    }
}
