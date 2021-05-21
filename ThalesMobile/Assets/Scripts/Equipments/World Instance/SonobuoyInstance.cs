using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using PlayerEquipement;
using System.Linq;
using System.Collections.Specialized;

/// <summary>
/// Antoine Leroux - 31/03/2021 - Script relative to the behavior detection of a sonobuoy. 
/// Rémi Sécher - 11/04/2021 - Rework to incorporate Sonobout instance to DetectionObject logic
/// </summary>
public class SonobuoyInstance : DetectionObject
{

    private SonobuoyDeployer source;
    [SerializeField]
    SonobuoyInstanceFeedback feedbackBehavior;

    float lifeTime;
    [HideInInspector]
    public float detectionRange;

    protected override void Start()
    {
        base.Start();
        feedbackBehavior.Init();
    }

    protected override void Update()
    {
        base.Update();
        if (gameObject.activeInHierarchy)
        {
            DetectElementsInsideRange();
        }        
    }

    // Use this function to deploy a sonobuoy where your finger touch the screen. 
    public void EnableSonobuoy(Vector2 target, float _range, float _lifeTime, float turbulentSeaFactor, SonobuoyDeployer _source)
    {
        levelManager = GameManager.Instance.levelManager;

        transform.position = Coordinates.ConvertVector2ToWorld(target);
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);

        if (levelManager.environnement.zones[levelManager.environnement.ZoneIn(target) - 1].state == ZoneState.SeaTurbulent)
            detectionRange = _range * turbulentSeaFactor;
        else detectionRange = _range;

        lifeTime = _lifeTime;
        source = _source;

        if (!levelManager.sonobuoysInScene.Contains(this)) levelManager.sonobuoysInScene.Add(this);
        if (!levelManager.activatedDetectionObjects.Contains(this)) levelManager.activatedDetectionObjects.Add(this);

        gameObject.SetActive(true);
        feedbackBehavior.Init();

        StartCoroutine(LifeTime());
    }

    public void DisableSonobuoy()
    {
        gameObject.SetActive(false);

        foreach (DetectableOceanEntity entity in detectedEntities.ToList())
        {
            RemoveDetectable(entity);
        }
        detectedEntities.Clear();

        transform.localPosition = Vector3.zero;
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);

        if (levelManager.sonobuoysInScene.Contains(this)) levelManager.sonobuoysInScene.Remove(this);
        if (levelManager.activatedDetectionObjects.Contains(this)) levelManager.activatedDetectionObjects.Remove(this);

        
        source.availaibleSonobuoys.Add(this);
        source.usedSonobuoys.Remove(this);
        

        StopAllCoroutines();
    }

    IEnumerator LifeTime()
    {
        float timer = 0;

        while (timer < lifeTime)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        DisableSonobuoy();
    }

    float distance;
    private void DetectElementsInsideRange()
    {
        distance = 0f;

        if (levelManager == null) levelManager = GameManager.Instance.levelManager;

        foreach(DetectableOceanEntity entity in levelManager.submarineEntitiesInScene)
        {
            distance = Mathf.Abs(Vector2.Distance(entity.coords.position, coords.position));

            if(distance <= detectionRange)
            {
                if (entity.currentDetectableState != DetectableState.cantBeDetected)
                {
                    if (!detectedEntities.Contains(entity)) AddDetectable(entity);
                }
                else if (detectedEntities.Contains(entity)) RemoveDetectable(entity);
            }
            else
            {
                if (detectedEntities.Contains(entity)) RemoveDetectable(entity);
            }
        }
    }

    protected override void RefreshFeedBack(DetectionState newState)
    {
        base.RefreshFeedBack(newState);

        if (newState == DetectionState.noDetection) feedbackBehavior.DetectionRangeFeedBack(false);
        else if (detectionState == DetectionState.noDetection) feedbackBehavior.DetectionRangeFeedBack(true);

        //if (newState == DetectionState.revealedDetection) feedbackBehavior.UpdateReveal(true);
        //else if (detectionState == DetectionState.revealedDetection && newState != DetectionState.revealedDetection) feedbackBehavior.UpdateReveal(false);
    }

    protected override void detectedEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.detectedEntities_CollectionChanged(sender, e);

        List<System.Type> detectedTypes = new List<System.Type>();
        List<DetectableOceanEntity> entities = new List<DetectableOceanEntity>();

        foreach(DetectableOceanEntity entity in detectedEntities)
        {
            if (!detectedTypes.Contains(entity.GetType()))
            {
                detectedTypes.Add(entity.GetType());
                entities.Add(entity);
            }                
        }

        feedbackBehavior.UpdateIcons(entities);
    }
}
