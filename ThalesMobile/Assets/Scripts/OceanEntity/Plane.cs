using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class Plane : PlayerOceanEntity
    {
        private Transform _transform;

        [HideInInspector]public Transform currentTargetPoint;

        void Start()
        {
            movementType = MovementType.air;
            _transform = transform;
        }

        void Update()
        {
            if(currentTargetPoint != null)
            {
                Move((Vector2)currentTargetPoint.position);
            }
        }
        public override void Move(Vector2 targetPosition)
        {
            //Calculate direction to target and store it in coords.
            coords.direction = targetPosition - (Vector2)_transform.position;

            //Update the plane's position.
            _transform.position += (Vector3)coords.direction.normalized * speed * Time.deltaTime;
            
            //Store the new position in the coords.
            coords.position = _transform.position;
        }

        public override void PathFinding()
        {
            throw new System.NotImplementedException();
        }

        public override void Waiting()
        {
            throw new System.NotImplementedException();
        }


    }
}
