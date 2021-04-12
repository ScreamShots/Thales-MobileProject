using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;

/// <summary>
/// Antoine Leroux - 07/04/2021 - Vigilance State is an enum describing the current submarine state.
/// </summary>
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
    public Ship ship;

    [Header("Movement")]
    public float maxSpeed;
    public float acceleration;
    [HideInInspector] public float currentSpeed;
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
    [HideInInspector] public List<Transform> sonobuoys;
    [HideInInspector] public List<float> sonobuoysDistance;

    [Header("Counter Measures")]
    public DecoyInstance decoy;
    private bool decoyIsCreateFlag;
    public List<CounterMeasure> counterMeasures;

    [Header("Objectif")]
    public int pointsToHack;
    private int pointsHacked = 0;
    private float hackingTimer = 0f;

    [Space]
    public List<InterestPoint> interestPoints;
    private InterestPoint nextInterestPoint;
    private Vector2 nextInterestPointPosition;
    private int randomNumber;

    private void Start()
    {
        _transform = transform;
        coords.position = Coordinates.ConvertWorldToVector2(_transform.position);
        currentSeaLevel = OceanEntities.SeaLevel.submarine;
        PickRandomInterrestPoint();
        levelManager.enemyEntitiesInScene.Add(this);
    }

    protected override void Update()
    {
        base.Update();

        // Movement.
        if (!decoy.decoyIsActive)
        {
            UpdateInterestPoint();
            decoyIsCreateFlag = false;
        }
        else
        {
            SubmarineDecoyMovement();

            if (!decoyIsCreateFlag)
            {
                decoyIsCreateFlag = true;
                PickRandomInterrestPoint();
            }
        }
        
        // Vigilance.
        UpdateState();
        UpdateSubmarineRange();
        DetectFregate();
        DetectSonobuoy();

        // Counter Measures.
        if (currentVigilance >= 0)
        {
            UpdateCounterMeasures();
        }   
    }

    #region Movement
    public void PickRandomInterrestPoint()
    {
        // If the submarine currently hacking a point.
        if (hackingTimer > 0)
        {
            nextInterestPoint.currentHackState = HackState.unhacked;
            nextInterestPoint.hackProgression = 0f;
            hackingTimer = 0;
            randomNumber = Random.Range(0, interestPoints.Count);
            nextInterestPoint = interestPoints[randomNumber];
            movingToNextPoint = true;
        }
        else
        {
            randomNumber = Random.Range(0, interestPoints.Count);
            nextInterestPoint = interestPoints[randomNumber];
            movingToNextPoint = true;
        }
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
            hackingTimer = 0;
            nextInterestPoint.currentHackState = HackState.doneHack;
            interestPoints.RemoveAt(randomNumber);
            pointsHacked++;
            PickRandomInterrestPoint();
        }
    }

    private void SubmarineDecoyMovement()
    {
        // The submarine still going in his direction
        if (decoy.randomDirection == 0)
        {
            coords.direction = coords.direction;
            coords.position += coords.direction.normalized * Time.deltaTime * currentSpeed;
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
        }
        // The submarine go in decoy angle direction 
        else
        {
            coords.direction = Coordinates.ConvertWorldToVector2(Quaternion.Euler(0, decoy.decoyAngle, 0) * Coordinates.ConvertVector2ToWorld(coords.direction.normalized));
            coords.position += coords.direction * Time.deltaTime * currentSpeed;
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
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

        if (currentVigilance <= 0)
        {
            currentVigilance = 0;
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
        float distanceFromFregate = Vector3.Distance(transform.position, ship.transform.position);

        if (distanceFromFregate < currentRange)
        {
            submarineDetectFregate = true;

            if (ship.currentTargetPoint != ship.nullVector)
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
        }
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

    #region CounterMeasures
    private void UpdateCounterMeasures()
    {
        // Lauch Heading Change counter measure.
        if ((currentState == VigilanceState.worried || currentState == VigilanceState.panicked) && submarineDetectFregate)
        {
            if (!UsingCounterMeasure())
            {
                counterMeasures[0].UseCounterMeasure(this);
            }
        }
        // Lauch Radio Silence counter measure.
        if (currentVigilance >= 100)
        {
            if (!UsingCounterMeasure())
            {               
                counterMeasures[1].UseCounterMeasure(this);
            }
        }
        // Lauch Bait Decoy counter measure.
        if ((currentState == VigilanceState.worried || currentState == VigilanceState.panicked) && currentDetectableState == DetectableState.revealed)
        {
            if (!UsingCounterMeasure())
            {              
                counterMeasures[2].UseCounterMeasure(this);
            }
        }
    }

    private bool UsingCounterMeasure()
    {
        for (int x = 0; x < counterMeasures.Count; x++)
        {
            if (!counterMeasures[x].readyToUse)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
