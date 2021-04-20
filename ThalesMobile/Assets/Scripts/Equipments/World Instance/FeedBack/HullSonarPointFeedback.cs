using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerEquipement
{
    public class HullSonarPointFeedback : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        HullSonarDetectionPoint source;

        [Header("Visual Params")]
        [SerializeField]
        Canvas revealIconCanvas;
        [SerializeField]
        Image revealIconImage;
        [SerializeField]
        Image revealPointerImage;
        [SerializeField]
        CanvasGroup globalVisualCanvas;
        
        public TweeningAnimator revealAppearAnim;

        private void Start()
        {
            revealAppearAnim.GetCanvasGroup();
            revealAppearAnim.rectTransform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (source.gameObject.activeInHierarchy && revealIconCanvas != null)
            {
                revealIconCanvas.transform.LookAt(GameManager.Instance.cameraController.cam.transform);
            }

            globalVisualCanvas.alpha = 1 - (source.timer / source.fadeDuration);
        }

        public void DisplayReveal(Sprite revealIcon, Sprite revealPointer)
        {
            revealIconImage.sprite = revealIcon;
            revealPointerImage.sprite = revealPointer;

            StartCoroutine(revealAppearAnim.anim.Play(revealAppearAnim, revealAppearAnim.rectTransform.anchoredPosition));
        }

        public void HideReveal()
        {
            StartCoroutine(revealAppearAnim.anim.PlayBackward(revealAppearAnim, revealAppearAnim.rectTransform.anchoredPosition, false));
        }
    }
}

