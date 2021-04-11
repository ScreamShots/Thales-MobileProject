using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 30/03/2021 - . 
/// </summary>
public class SpermWhale : DetectableOceanEntity
{
    [Header("Movement")]
    public float speed;
    private Transform _transform;

    [Space]
    public List<Transform> pointsToFollow;
    private Vector2 nextPoint;
    private int currentPoint = 0;

    private void Start()
    {
        _transform = transform;
        coords.position = Coordinates.ConvertWorldToVector2(_transform.position);
        currentSeaLevel = OceanEntities.SeaLevel.submarine;
        nextPoint = Coordinates.ConvertWorldToVector2(pointsToFollow[currentPoint].position);       
    }

    protected override void Update()
    {
        base.Update();
        Move(nextPoint);
    }

    public override void Move(Vector2 targetPosition)
    {
        //Calculate direction to target and store it in coords.
        coords.direction = targetPosition - coords.position;

        //Update the plane's position.
        coords.position += coords.direction.normalized * speed * Time.deltaTime;

        //Store the new position in the coords.
        _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

        if ((targetPosition - coords.position).magnitude < 0.1f)
        {
            if (currentPoint < (pointsToFollow.Count - 1))
            {
                currentPoint++;
            }
            else
            {
                currentPoint = 0;
            }
            nextPoint = Coordinates.ConvertWorldToVector2(pointsToFollow[currentPoint].position);
        }
    }

    public override void PathFinding()
    {
        
    }
}
