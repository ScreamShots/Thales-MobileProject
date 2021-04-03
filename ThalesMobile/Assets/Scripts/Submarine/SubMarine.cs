using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 28/03/2021 - Script relative to the movement and behavior of SubMarine entity. 
/// </summary>
public class SubMarine : DetectableOceanEntity
{
    [Header("Movement")]
    private Transform _transform;
    public float maxSpeed;
    public float acceleration;
    private float currentSpeed;

    [Header("Objectif")]
    public int pointsToHack;
    private int pointsHacked = 0;
    private float hackingTimer = 0f;

    [Space]
    public List<InterestPoint> interestPoints;
    private InterestPoint nextInterestPoint;
    private int randomNumber;

    protected override void Start()
    {
        _transform = transform;
        currentSeaLevel = OceanEntities.SeaLevel.submarine;
        PickRandomInterrestPoint();
    }

    private void Update()
    {    
        if (pointsHacked < pointsToHack)
        {
            // Move Submarine to the target point.
            if (nextInterestPoint != null)
            {
                Move(new Vector2(nextInterestPoint.transform.position.x, nextInterestPoint.transform.position.z));
            }

            // Hacking the interrest point with hacking time specific to the interrest point.
            if (transform.position == nextInterestPoint.transform.position)
            {
                Capture();
            }
        }
        else
        {
            //LE JOUEUR A PERDU
        }
    }

    private void PickRandomInterrestPoint()
    {
        randomNumber = Random.Range(0, interestPoints.Count);
        nextInterestPoint = interestPoints[randomNumber];
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

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.y), Time.deltaTime * currentSpeed);


        /*
         //Calculate direction to target and store it in coords.
            coords.direction = targetPosition - coords.position;

            //Update the plane's position.
           coords.position += coords.direction.normalized * speed * Time.deltaTime;

            //Store the new position in the coords.
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

            if ((targetPosition - coords.position).magnitude < 0.1f)
            {
                currentTargetPoint = nullVector;
            }
         */
    }

    public override void PathFinding()
    {
        
    }
}
