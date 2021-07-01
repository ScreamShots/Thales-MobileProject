using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
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
    [SerializeField]
    RectTransform relativeScaler;

    [Header("Reveal Params")]
    [SerializeField]
    RectTransform revealCanvasRT;
    [SerializeField]
    Transform tempLookCamTransform;
    CameraController camController;
    public float animDuration;
    [SerializeField]
    AnimationCurve revealScaleProgression;
    Vector3 revealBaseScale;
    Coroutine currentAnim;
    float resetTimer;

    [Header("Expiration Params")]
    public Color freshColor;
    public Color nearExpiredColor;
    public Color expiredColor;
    public Color revealedColor;

    [Header("Sound - Appear")]
    [SerializeField]
    AudioMixerGroup targetGroup;
    [SerializeField ]
    AudioClip appearSound;
    [SerializeField ]
    float appearSoundVolume;
    [SerializeField]
    AudioSource appearSoundSource;
    [SerializeField]
     
    AudioClip submarinDetected;
    [SerializeField]
     
    float submarinDetectedVolume;
    [SerializeField]
    AudioSource submarinDetectedSource;
    bool alreadyPlayedSound;

    private void Start()
    {
        revealCanvasRT.localScale = Vector3.zero;
        camController = GameManager.Instance.cameraController;
        revealBaseScale = relativeScaler.localScale;
    }

    private void Update()
    {
        revealCanvasRT.forward = camController.cam.transform.forward;
        relativeScaler.localScale = revealBaseScale * revealScaleProgression.Evaluate(camController.zoomIntensity / 1);
    }

    public void OnEnable()
    {
        appearSoundSource.volume = Mathf.Clamp01(appearSoundVolume);
        GameManager.Instance.soundHandler.PlaySound(appearSound, appearSoundSource, targetGroup);
    }

    public void DisplayReveal(Sprite revealIcon, Sprite revealPointer, float revealDuration)
    {
        revealIconImage.sprite = revealIcon;
        revealPointerImage.sprite = revealPointer;

        if (!alreadyPlayedSound && source.linkedEntity.GetType() == typeof(Submarine))
        {
            submarinDetectedSource.volume = Mathf.Clamp01(submarinDetectedVolume);
            GameManager.Instance.soundHandler.PlaySound(submarinDetected, submarinDetectedSource, targetGroup);
            //alreadyPlayedSound = true;
        }        

        if (currentAnim == null)
            currentAnim = StartCoroutine(Scale(Vector3.one, animDuration, revealCanvasRT, true, revealDuration));
          
        else resetTimer = 0;
    }

    public void UpdatePos(Vector3 pointPos, Vector3 detectablePos)
    {
        if(pointPos != detectablePos)
        {
            appearSoundSource.volume = Mathf.Clamp01(appearSoundVolume);
            GameManager.Instance.soundHandler.PlaySound(appearSound, appearSoundSource, targetGroup);
        }        
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
            case ExpirationValue.Revealed:
                dotImage.color = revealedColor;
                break;
        }
    }

    IEnumerator Scale(Vector3 target, float duration, RectTransform obj, bool reset = false, float resetTime = 0)
    {
        Vector3 start = obj.localScale;
        float timer = 0;
        resetTimer = 0;

        while (timer < duration)
        {
            obj.localScale = Vector3.Lerp(start, target, timer / duration);
            yield return null;
            timer += Time.deltaTime;
        }

        obj.localScale = target;

        if (reset)
        {
            while(resetTimer < resetTime)
            {
                yield return null;
                resetTimer += Time.deltaTime;
            }

            currentAnim = StartCoroutine(Scale(Vector3.zero, animDuration, revealCanvasRT));
        }
        else currentAnim = null;      
    }
}
