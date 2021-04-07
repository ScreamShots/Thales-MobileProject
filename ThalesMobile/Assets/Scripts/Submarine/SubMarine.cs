using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VigilanceState
{
    calm,
    worried,
    panicked
}

/// <summary>
/// Antoine Leroux - 28/03/2021 - Script relative to the movement and behavior of SubMarine entity. 
/// </summary>
public class Submarine : DetectableOceanEntity
{
    private Transform _transform;

    [Header("References")]
    public LevelManager levelManager;

    [Header("Movement")]
    public float maxSpeed;
    public float acceleration;
    private float currentSpeed;
    private bool movingToNextPoint;

    [Header("Vigilance")]
    public float currentVigilance = 0f;
    public VigilanceState currentState;

    [Header("Vigilance State")]
    public Vector2 calmStateValues;
    public Vector2 worriedStateValues;
    public Vector2 panickedStateValues;
    private bool reachWorriedState;

    [Header("Submarine Range")]
    public float detectionRangeCalm;
    public float detectionRangeWorried;
    public float detectionRangePanicked;
    private float currentRange;
    public GameObject rangeVisual;

    [Header("Vigilance Incrase Values")]
    public float sonobuoyVigiIncr;
    public float fregateStationaryVigiIncr;
    public float fregateMoveVigiIncr;   
    private bool submarineDetectFregate;
    private List<Transform> sonobuoys;
    private List<float> sonobuoysDistance;

    [Header("Objectif")]
    public int pointsToHack;
    private int pointsHacked = 0;
    private float hackingTimer = 0f;

    [Space]
    public List<InterestPoint> interestPoints;
    private InterestPoint nextInterestPoint;
    private Vector2 nextInterestPointPosition;
    private int randomNumber;

    protected override void Start()
    {
        _transform = transform;
        coords.position = Coordinates.ConvertWorldToVector2(_transform.position);
        currentSeaLevel = OceanEntities.SeaLevel.submarine;
        PickRandomInterrestPoint();
    }

    private void Update()
    {
        // Movement.
        UpdateInterestPoint();
        
        // Vigilance.
        UpdateState();
        UpdateSubmarineRange();
        DetectFregate();
        DetectSonobuoy();
    }

    #region Movement
    private void PickRandomInterrestPoint()
    {
        randomNumber = Random.Range(0, interestPoints.Count);
        nextInterestPoint = interestPoints[randomNumber];
        movingToNextPoint = true;
    }

    private void UpdateInterestPoint()
    {
        if (pointsHacked < pointsToHack)
        {
            nextInterestPointPosition = Coordinates.ConvertWorldToVector2(nextInterestPoint.transform.position);
            // Move Submarine to the target point.
            if (nextInterestPoint != null && movingToNextPoint)
            {
                Move(nextInterestPointPosition);
            }

            // Hacking the interrest point with hacking time specific to the interrest point.
            if ((nextInterestPointPosition - coords.position).magnitude < 0.2f)
            {
                Capture();
            }
        }
        else
        {
            // Player lose.
        }
    }

    private void Capture()
    {
        hackingTimer += Time.deltaTime;
        nextInterestPoint.currentHackState = HackState.inHack;
        nextInterestPoint.hackProgression = ((hackingTimer - 0) / nextInterestPoint.hackTime) * 100f;

        if (hackingTimer >= nextInterestPoint.hackTime)
        {
            nextInterestPoint.currentHackState = HackState.doneHack;
            interestPoints.RemoveAt(randomNumber);
            pointsHacked++;
            PickRandomInterrestPoint();
            hackingTimer = 0;
        }
    }

    public override void Move(Vector2 targetPosition)
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        //Calculate direction to target and store it in coords.
        coords.direction = targetPosition - coords.position;

        //Update the plane's position.
        coords.position += coords.direction.normalized * currentSpeed * Time.deltaTime;

        //Store the new position in the coords.
        _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

        if ((targetPosition - coords.position).magnitude < 0.1f)
        {
            movingToNextPoint = false;
            currentSpeed = 0;
        }
    }

    public override void PathFinding()
    {

    }
    #endregion


    #region Vigilance
    private void UpdateState()
    {
        if (currentVigilance >= calmStateValues.x && currentVigilance < calmStateValues.y && !reachWorriedState)
        {
            currentState = VigilanceState.calm;
        }
        else if ((currentVigilance >= worriedStateValues.x && currentVigilance < worriedStateValues.y) || (currentVigilance >= calmStateValues.x && currentVigilance < calmStateValues.y && reachWorriedState))
        {
            currentState = VigilanceState.worried;
            reachWorriedState = true;
        }
        else if (currentVigilance >= panickedStateValues.x && currentVigilance <= panickedStateValues.y)
        {
            currentState = VigilanceState.panicked;
        }

        if (currentVigilance >= 100)
        {
            currentVigilance = 100;
        }
    }

    private void UpdateSubmarineRange()
    {
        if (currentState == VigilanceState.calm)
        {
            rangeVisual.transform.localScale = new Vector2(detectionRangeCalm * 2, detectionRangeCalm * 2);
            currentRange = detectionRangeCalm;
        }
        else if (currentState == VigilanceState.worried)
        {
            rangeVisual.transform.localScale = new Vector2(detectionRangeWorried * 2, detectionRangeWorried * 2);
            currentRange = detectionRangeWorried;
        }
        else if (currentState == VigilanceState.panicked)
        {
            rangeVisual.transform.localScale = new Vector2(detectionRangePanicked * 2, detectionRangePanicked * 2);
            currentRange = detectionRangePanicked;
        }
    }

    private void IncreaseVigilance(float valuePerSecond)
    {
        currentVigilance += valuePerSecond * Time.deltaTime;
    }

    private void DetectFregate()
    {
        /*float distanceFromFregate = Vector3.Distance(transform.position, // Fregate position);

        if (distanceFromFregate < currentRange)
        {
            submarineDetectFregate = true;

            if (// Fregate is moving)
            {
                IncreaseVigilance(fregateMoveVigiIncr);
            }
            else
            {
                IncreaseVigilance(fregateStationaryVigiIncr);
            }
        }
        else
        {
            submarineDetectFregate = false;
        }*/
    }

    private void DetectSonobuoy()
    {
        sonobuoys.Clear();

        for (int x = 0; x < levelManager.sonobuoysInScene.Count; x++)
        {
            sonobuoys.Add(levelManager.sonobuoysInScene[x].transform);
        }

        sonobuoysDistance = new List<float>(new float[sonobuoys.Count]);

        for (int x = 0; x < sonobuoys.Count; x++)
        {
            sonobuoysDistance[x] = Vector3.Distance(transform.position, sonobuoys[x].position);

            if (sonobuoysDistance[x] < currentRange)
            {
                IncreaseVigilance(sonobuoyVigiIncr);
            }
        }
    }
    #endregion
}
