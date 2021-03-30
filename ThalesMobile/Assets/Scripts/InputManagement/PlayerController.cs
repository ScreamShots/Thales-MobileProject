using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
public class PlayerController : MonoBehaviour
{
    public PlayerOceanEntity currentSelectedEntity;
    [HideInInspector] public Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEntityMoveTarget(Vector2 target)
    {
        currentSelectedEntity.currentTargetPoint = target;
    }



}
