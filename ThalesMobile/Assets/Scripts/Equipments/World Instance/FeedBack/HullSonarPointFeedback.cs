using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Tweek.FlagAttributes;

namespace PlayerEquipement
{
    [TweekClass]
    public class HullSonarPointFeedback : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        HullSonarDetectionPoint source;

        [Header("Visual Params")]
        [SerializeField]
        Image revealIconImage;
        [SerializeField]
        Image revealPointerImage;
        [SerializeField]
        CanvasGroup globalVisualCanvas;

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
            globalVisualCanvas.alpha = 1 - (source.timer / source.fadeDuration);
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

        IEnumerator Scale(Vector3 target, float duration, RectTransform obj)
        {
            Vector3 start = obj.localScale;
            float timer = 0;

            while(timer < duration)
            {
                obj.localScale = Vector3.Lerp(start, target, timer / duration);
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;
            }
        }
    }
}

