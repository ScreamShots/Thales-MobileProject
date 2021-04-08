using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rémi Sécher - 08/04/21 - Class that Handle detectionPoint spawn by HullSonar Equipement on detected entities pos
/// </summary>

public class HullSonarDetectionPoint : DetectionObject
{
    [HideInInspector]
    public float fadeDuration;

    //Get the point from his state of unused in the pool and activate in GameWorld
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

    //Place the point back in pool
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

    //Progressive fade starting at spawn
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

    //FeedBack modification depending on the state of detectionState
    protected override void RefreshFeedBack(DetectionState newState)
    {
        base.RefreshFeedBack(newState);
    }
}
