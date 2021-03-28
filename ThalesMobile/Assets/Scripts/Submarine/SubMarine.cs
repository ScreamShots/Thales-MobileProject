using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 28/03/2021 - Script relative to the movement and behavior of SubMarine entity. 
/// </summary>
public class SubMarine : DetectableOceanEntity
{
    [Header("Movement")]
    public float maxSpeed;
    public float acceleration;
    private float currentSpeed;

    [Space]
    public List<Transform> interrestPoints;
    private Transform nextInterrestPoint;
    private int randomNumber;

    private void Start()
    {
        currentSeaLevel = OceanEntities.SeaLevel.submarine;
        PickRandomInterrestPoint();
    }

    private void Update()
    {
        if (nextInterrestPoint != null)
        {
            Move(new Vector2(nextInterrestPoint.position.x, nextInterrestPoint.position.z));
        }
    }

    private void PickRandomInterrestPoint()
    {
        randomNumber = Random.Range(0, interrestPoints.Count);
        nextInterrestPoint = interrestPoints[randomNumber];
    }

    public override void Move(Vector2 targetPosition)
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.y), Time.deltaTime * currentSpeed);
    }

    public override void PathFinding()
    {
        
    }
}
