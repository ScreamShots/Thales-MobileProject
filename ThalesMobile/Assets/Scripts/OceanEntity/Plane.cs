using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class Plane : PlayerOceanEntity
    {
        private Transform _transform;

        [HideInInspector]public Transform currentTargetPoint;

        private Vector2 waitingPoint = new Vector2();
        private Vector2 waitingCircleVector;
        public float waitingRoutineRadius;

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
                if(waitingPoint != null)
                {
                    waitingPoint = Vector2.zero;
                }
            }
            else
            {
                Waiting();
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

            if (coords.position == targetPosition)
            {
                currentTargetPoint = null;
            }
        }

        public override void PathFinding()
        {
            throw new System.NotImplementedException();
        }
        
        public override void Waiting()
        {
            if(waitingPoint == Vector2.zero)
            {
                waitingPoint = coords.position;
            }
            else
            {
                waitingCircleVector = coords.position - waitingPoint;

                if (waitingCircleVector.magnitude > waitingRoutineRadius)
                {
                    coords.direction = new Vector2(waitingCircleVector.y, -waitingCircleVector.x);

                    _transform.position += (Vector3)coords.direction.normalized * speed * Time.deltaTime;

                    _transform.position += (Vector3)(waitingCircleVector.normalized * waitingRoutineRadius) - (Vector3)waitingCircleVector;
                 
                    coords.position = _transform.position;
                }
                else
                {
                    Move(Vector2.up * waitingRoutineRadius);
                }
            }
       }
    }
}
