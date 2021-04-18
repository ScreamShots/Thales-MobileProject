using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;
using System.Linq;

/// <summary>
/// Rémi Sécher - 08/04/21 - Class that Handle detectionPoint spawn by HullSonar Equipement on detected entities pos
/// </summary>

public class HullSonarDetectionPoint : DetectionObject
{
    [SerializeField]
    HullSonarPointFeedback feedbackBehavior;
    [HideInInspector]
    public float fadeDuration;
    HullSonar source;

    //Get the point from his state of unused in the pool and activate in GameWorld
    public void ActivatePoint(DetectableOceanEntity detectedElement, float _fadeDuration, HullSonar _source)
    {
        levelManager = GameManager.Instance.levelManager;

        source = _source;
        levelManager.activatedDetectionObjects.Add(this);

        AddDetectable(detectedElement);

        transform.position = Coordinates.ConvertVector2ToWorld(detectedElement.coords.position);
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);
        fadeDuration = _fadeDuration;

        gameObject.SetActive(true);

        StartCoroutine(Fade());
    }

    //Place the point back in pool
    public void DesactivatePoint()
    {
        gameObject.SetActive(false);

        foreach(DetectableOceanEntity entity in detectedEntities.ToList())
        {
            RemoveDetectable(entity);
        }
        detectedEntities.Clear();

        transform.localPosition = Vector3.zero;
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);

        levelManager.activatedDetectionObjects.Remove(this);
        source.availableDetectionPoints.Add(this);
        source.usedDetectionPoints.Remove(this);

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

        if(newState == DetectionState.revealedDetection)
        {
            if(detectedEntities[0] != null) feedbackBehavior.DisplayReveal(detectedEntities[0].detectFeedback.hullSonarRevealIcon);
        }
        else if(detectionState == DetectionState.revealedDetection)
        {
            feedbackBehavior.HideReveal();
        }
    }
}
