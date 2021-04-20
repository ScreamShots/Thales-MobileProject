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
    [HideInInspector]
    public HullSonar source;
    [HideInInspector]
    public Coroutine fadeCouroutine;
    [HideInInspector]
    public float timer;

    //Get the point from his state of unused in the pool and activate in GameWorld
    public IEnumerator ActivatePoint(DetectableOceanEntity detectedElement, float _fadeDuration, HullSonar _source)
    {
        levelManager = GameManager.Instance.levelManager;

        source = _source;
        levelManager.activatedDetectionObjects.Add(this);        

        transform.position = Coordinates.ConvertVector2ToWorld(detectedElement.coords.position);
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);
        fadeDuration = _fadeDuration;

        gameObject.SetActive(true);

        yield return null;

        AddDetectable(detectedElement);

        fadeCouroutine = StartCoroutine(Fade());
    }

    //Place the point back in pool
    public IEnumerator DesactivatePoint()
    {
        if(detectionState == DetectionState.revealedDetection)
        {
            feedbackBehavior.HideReveal();
            yield return new WaitForSeconds(feedbackBehavior.revealAppearAnim.anim.animationTime + (feedbackBehavior.revealAppearAnim.anim.animationTime/5));
        }

        gameObject.SetActive(false);
        source.DesactivatePoint(this, detectedEntities[0]);

        foreach(DetectableOceanEntity entity in detectedEntities.ToList())
        {
            RemoveDetectable(entity);
        }
        detectedEntities.Clear();

        transform.localPosition = Vector3.zero;
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);        

        StopAllCoroutines();
    }

    //Progressive fade starting at spawn
    IEnumerator Fade()
    {
        timer = 0;

        while (timer < fadeDuration)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        StartCoroutine(DesactivatePoint());
    }

    public void ResetFade(float _fadeTimer)
    {
        StopCoroutine(fadeCouroutine);
        fadeCouroutine = StartCoroutine(Fade());
    }

    //FeedBack modification depending on the state of detectionState
    protected override void RefreshFeedBack(DetectionState newState)
    {
        base.RefreshFeedBack(newState);

        if(newState == DetectionState.revealedDetection)
        {
            if(detectedEntities[0] != null && gameObject.activeInHierarchy) feedbackBehavior.DisplayReveal(detectedEntities[0].detectFeedback.hullSonarRevealIcon, detectedEntities[0].detectFeedback.hullSonarRevealPointer);
        }
        else if(detectionState == DetectionState.revealedDetection)
        {
            if(gameObject.activeInHierarchy) feedbackBehavior.HideReveal();
        }
    }

    [ContextMenu("Clear")]
    public void ClearDetected()
    {
        DetectableOceanEntity test = detectedEntities[0];
        detectedEntities.Clear();
        detectedEntities.Add(test);
    }
}
