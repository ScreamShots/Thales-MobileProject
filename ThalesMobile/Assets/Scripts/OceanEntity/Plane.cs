using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class Plane : PlayerOceanEntity
    {
        private Transform _transform;

        private Vector2 waitingPoint = new Vector2(-9999,-9999);
        private Vector2 waitingCircleVector;
        public float waitingRoutineRadius;
        public float minimumSpeed;
        private float currentSpeed = 0;
        

        void Start()
        {
            movementType = MovementType.air;
            _transform = transform;
        }

        void FixedUpdate()
        {
            if(currentTargetPoint != nullVector)
            {
                Move(currentTargetPoint);
                if (waitingPoint != nullVector)
                {
                    waitingPoint = nullVector;
                }
            }
            else
            {
                Waiting();
            }
        }
        public override void Move(Vector2 targetPosition)
        {
            if (currentSpeed < speed)
                currentSpeed += acceleration * Time.fixedDeltaTime;
            else
                currentSpeed = speed;

            //Calculate direction to target and store it in coords.
            Vector2 dir = targetPosition - coords.position;
            if (Vector2.Angle(coords.direction, dir) > Time.fixedDeltaTime * rotateSpeed)
            {
                int turnSide = Vector2.SignedAngle(coords.direction, dir) > 0 ? 1 : -1;
                float currentAngle = Vector2.SignedAngle(Vector2.right, coords.direction) + turnSide * Time.fixedDeltaTime * rotateSpeed;
                coords.direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                _transform.rotation = Quaternion.Euler(transform.rotation.x, -currentAngle + 90, transform.rotation.z);
            }
            else
            {
                coords.direction = dir;
            }

            //Update the plane's position.
            coords.position += coords.direction.normalized * currentSpeed * Time.fixedDeltaTime;

            //Store the new position in the coords.
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

            if (dir.magnitude < 2f)
            {
                if (currentSpeed > minimumSpeed)
                {
                    currentSpeed -= acceleration * 3 * Time.fixedDeltaTime;
                }
                if (dir.magnitude < 0.1f)
                {
                    currentSpeed = minimumSpeed;
                    currentTargetPoint = nullVector;
                }
            }
        }

        public override void PathFinding()
        {
            throw new System.NotImplementedException();
        }
        
        public override void Waiting()
        {
            if(waitingPoint == nullVector)
            {
                waitingPoint = coords.position;
            }
            else
            {
                waitingCircleVector = coords.position - waitingPoint;

                if (waitingCircleVector.magnitude > waitingRoutineRadius)
                {
                    Vector2 dir = new Vector2(waitingCircleVector.y, -waitingCircleVector.x);
                    if (Vector2.Angle(coords.direction, dir) > Time.fixedDeltaTime * rotateSpeed)
                    {
                        int turnSide = Vector2.SignedAngle(coords.direction, dir) > 0 ? 1 : -1;
                        float currentAngle = Vector2.SignedAngle(Vector2.right, coords.direction) + turnSide * Time.fixedDeltaTime * rotateSpeed/2;
                        coords.direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                    }
                    else{

                        coords.direction = dir;
                        if (currentSpeed > minimumSpeed)
                        {
                            currentSpeed -= acceleration * 3 * Time.fixedDeltaTime;
                        }
                        else
                        {
                            currentSpeed = minimumSpeed;
                        }
                    }

                    coords.position += coords.direction.normalized * currentSpeed * Time.fixedDeltaTime;

                    coords.position += (waitingCircleVector.normalized * waitingRoutineRadius) - waitingCircleVector;

                    _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
                    _transform.forward = new Vector3(coords.direction.x, 0, coords.direction.y);
                }
                else
                {

                    Move(new Vector2(_transform.forward.x, _transform.forward.z) * waitingRoutineRadius);
                    currentSpeed = minimumSpeed;
                }
            }
       }
    }
}
