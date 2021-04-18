using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using PlayerEquipement;
using System.Linq;

/// <summary>
/// Antoine Leroux - 31/03/2021 - Script relative to the behavior detection of a sonobuoy. 
/// Rémi Sécher - 11/04/2021 - Rework to incorporate Sonobout instance to DetectionObject logic
/// </summary>
public class SonobuoyInstance : DetectionObject
{

    private float lifeTime;
    private float detectionRange;
    private SonobuoyDeployer source;

    [Header("Range Display - Temp")]
    public GameObject mesh;
    public GameObject rangeVisual;
    private SpriteRenderer rangeSprite;
    public Color undetectedElementColor;
    public Color detectedElementColor;


    protected void Awake()
    {
        rangeSprite = rangeVisual.GetComponent<SpriteRenderer>();
        rangeVisual.transform.localScale = new Vector2(detectionRange * 2, detectionRange * 2);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            ChangeColorRange();
            DetectElementsInsideRange();
        }        
    }

    // Use this function to deploy a sonobuoy where your finger touch the screen. 
    public void EnableSonobuoy(Vector2 target, float _range, float _lifeTime, SonobuoyDeployer _source)
    {
        levelManager = GameManager.Instance.levelManager;

        transform.position = Coordinates.ConvertVector2ToWorld(target);
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);

        detectionRange = _range;
        lifeTime = _lifeTime;
        source = _source;

        if (!levelManager.sonobuoysInScene.Contains(this)) levelManager.sonobuoysInScene.Add(this);
        if (!levelManager.activatedDetectionObjects.Contains(this)) levelManager.activatedDetectionObjects.Add(this);

        gameObject.SetActive(true);

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
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        DisableSonobuoy();
    }

    float distance;
    private void DetectElementsInsideRange()
    {
        distance = 0f;

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

    private void ChangeColorRange()
    {
        if (detectionState == DetectionState.noDetection)
        {
            rangeSprite.color = undetectedElementColor;
        }
        else if (detectionState != DetectionState.noDetection)
        {
            rangeSprite.color = detectedElementColor;
        }
    }
}
