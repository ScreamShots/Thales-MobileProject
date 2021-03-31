using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 31/03/2021 - Sonobuoy Detectable State is an enum describing the current sonobuoy detection state. 
/// </summary>
public enum SonobuoyDetectionState
{
    undetectedElement,
    detectedElement,
    severalDetectedElements
}

/// <summary>
/// Antoine Leroux - 31/03/2021 - Script relative to the behavior detection of a sonobuoy. 
/// </summary>
public class Sonobuoy : MonoBehaviour
{
    public SonobuoyDetectionState currentDetectionState;
    private Transform _transform;
    private float distanceFromEntity;

    [Header("References")]
    public LevelManager levelManager;

    [Header("Effect")]
    public float sonobuoyLifeTime;
    private float timeBeforeDisableSonobuoy;
    private bool sonobuoyIsActive;
    public float detectionRange;

    [Header("Range Display")]
    public GameObject mesh;
    public GameObject rangeVisual;
    private SpriteRenderer rangeSprite;
    public Color undetectedElementColor;
    public Color detectedElementColor;

    [Header("DEBUG")]
    public List<DetectableOceanEntity> entitiesInsideSonobuoyRange;


    private void Start()
    {
        _transform = transform;
        EnableSonobuoy(Coordinates.ConvertWorldToVector2(_transform.position));

        rangeSprite = rangeVisual.GetComponent<SpriteRenderer>();
        rangeVisual.transform.localScale = new Vector2(detectionRange * 2, detectionRange * 2);
    }

    private void Update()
    {
        LifeTime();
        SwitchDetectionState();
        ChangeColorRange();

        if (sonobuoyIsActive)
        {
            mesh.SetActive(true);
            rangeVisual.SetActive(true);
            DetectElementsInsideRange();
        }
        else
        {
            mesh.SetActive(false);
            rangeVisual.SetActive(false);
            entitiesInsideSonobuoyRange = new List<DetectableOceanEntity>();
        }
    }

    // Use this function to deploy a sonobuoy where your finger touch the screen. 
    private void EnableSonobuoy(Vector2 deploymentPosition)
    {
        _transform.position = Coordinates.ConvertVector2ToWorld(deploymentPosition);
        timeBeforeDisableSonobuoy = sonobuoyLifeTime;  
    }

    private void LifeTime()
    {
        if (timeBeforeDisableSonobuoy > 0)
        {
            sonobuoyIsActive = true;
            timeBeforeDisableSonobuoy -= Time.deltaTime;           
        }
        else
        {
            sonobuoyIsActive = false;
        }
    }

    private void SwitchDetectionState()
    {
        if (entitiesInsideSonobuoyRange.Count < 1)
        {
            currentDetectionState = SonobuoyDetectionState.undetectedElement;
        }
        else if (entitiesInsideSonobuoyRange.Count == 1)
        {
            currentDetectionState = SonobuoyDetectionState.detectedElement;
        }
        else if (entitiesInsideSonobuoyRange.Count > 1)
        {
            currentDetectionState = SonobuoyDetectionState.severalDetectedElements;
        }
    }

    private void DetectElementsInsideRange()
    {
        for (int x = 0; x < levelManager.submarineEntitiesInScene.Count; x++)
        {
            // Calculate distance from the entity
            distanceFromEntity = Vector2.Distance(Coordinates.ConvertWorldToVector2(levelManager.submarineEntitiesInScene[x].transform.position), Coordinates.ConvertWorldToVector2(_transform.position));

            if (distanceFromEntity < detectionRange)
            {
                if (!entitiesInsideSonobuoyRange.Contains(levelManager.submarineEntitiesInScene[x]))
                {
                    entitiesInsideSonobuoyRange.Add(levelManager.submarineEntitiesInScene[x]);
                }
            }
            else
            {
                if (entitiesInsideSonobuoyRange.Contains(levelManager.submarineEntitiesInScene[x]))
                {
                    entitiesInsideSonobuoyRange.Remove(levelManager.submarineEntitiesInScene[x]);
                }
            }
        }
    }

    private void ChangeColorRange()
    {
        if (currentDetectionState == SonobuoyDetectionState.undetectedElement)
        {
            rangeSprite.color = undetectedElementColor;
        }
        else if (currentDetectionState == SonobuoyDetectionState.detectedElement)
        {
            rangeSprite.color = detectedElementColor;
        }
    }
}
