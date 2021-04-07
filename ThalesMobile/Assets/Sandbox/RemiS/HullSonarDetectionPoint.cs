using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullSonarDetectionPoint : DetectionObject
{
    [HideInInspector]
    public float fadeDuration;

    public void ActivatePoint(DetectableOceanEntity detectedElement, float _fadeDuration)
    {
        levelManager.activatedDetectionObjects.Add(this);
        detectedEntities.Add(detectedElement);
        transform.position = Coordinates.ConvertVector2ToWorld(detectedElement.coords.position);
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);
        fadeDuration = _fadeDuration;
        detectionState = DetectionState.unknownDetection;
        gameObject.SetActive(true);
        StartCoroutine(Fade());
    }

    public void DesactivatePoint()
    {
        gameObject.SetActive(false);
        levelManager.activatedDetectionObjects.Remove(this);
        detectedEntities.Clear();
        detectionState = DetectionState.noDetection;
        transform.localPosition = Vector3.zero;
        detectionState = DetectionState.noDetection;
        StopAllCoroutines();
    }

    IEnumerator Fade()
    {
        float timer = 0;

        while (timer < fadeDuration)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        DesactivatePoint();
    }

    protected override void RefreshFeedBack(DetectionState newState)
    {
        base.RefreshFeedBack(newState);
    }
}
