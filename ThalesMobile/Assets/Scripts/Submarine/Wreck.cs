using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Antoine Leroux - 31/03/2021 - . 
/// </summary>
public class Wreck : DetectableOceanEntity
{
    private Transform _transform;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.levelManager.submarineEntitiesInScene.Add(this);

        _transform = transform;
        coords.position = Coordinates.ConvertWorldToVector2(_transform.position);
        currentSeaLevel = OceanEntities.SeaLevel.submarine;
    }

    public override void Move(Vector2 targetPosition)
    {

    }

}
