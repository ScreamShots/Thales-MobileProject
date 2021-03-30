using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 28/03/2021 - Script relative to the movement and behavior of SubMarine entity. 
/// </summary>
public class Submarine : DetectableOceanEntity
{
    [Header("Movement")]
    private Transform _transform;
    public float maxSpeed;
    public float acceleration;
    private float currentSpeed;
    private bool movingToNextPoint;

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

    private void PickRandomInterrestPoint()
    {
        randomNumber = Random.Range(0, interestPoints.Count);
        nextInterestPoint = interestPoints[randomNumber];
        movingToNextPoint = true;
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
}
