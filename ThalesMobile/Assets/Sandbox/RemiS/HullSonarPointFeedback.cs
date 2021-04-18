using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Update()
    {
        if (source.gameObject.activeInHierarchy && revealIconCanvas != null)
        {
            revealIconCanvas.transform.LookAt(GameManager.Instance.cameraController.cam.transform);
        }
    }

    public void DisplayReveal(Sprite revealIcon)
    {
        revealIconCanvas.gameObject.SetActive(true);
        revealIconImage.sprite = revealIcon;
    }

    public void HideReveal()
    {
        revealIconCanvas.gameObject.SetActive(false);
    }
}
