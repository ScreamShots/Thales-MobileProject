using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class Ship : PlayerOceanEntity
    {
        private Transform _transform;
        private float currentSpeed = 0;

        void Start()
        {
            movementType = MovementType.sea;
            _transform = transform;

            coords = new Coordinates(_transform.position, Vector2.up, 0);
        }

        void FixedUpdate()
        {
            if (currentTargetPoint != nullVector)
            {
                Move(currentTargetPoint);
            }
        }
        public override void Move(Vector2 targetPosition)
        {
            //Calculate direction to target and store it in coords.
            Vector2 dir = targetPosition - coords.position;
            if (Vector2.Angle(coords.direction, dir) > Time.fixedDeltaTime * rotateSpeed) 
            {
                int turnSide = Vector2.SignedAngle(coords.direction, dir) > 0 ? 1 : -1;
                float currentAngle = Vector2.SignedAngle(Vector2.right, coords.direction) + turnSide * Time.fixedDeltaTime * rotateSpeed;
                coords.direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                _transform.rotation = Quaternion.Euler(transform.rotation.x, -currentAngle + 90, transform.rotation.z); 

                if(currentSpeed > 1f)
                {
                    //Update the plane's position.
                    coords.position += coords.direction.normalized * currentSpeed * Time.fixedDeltaTime;

                    //Store the new position in the coords.
                    _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
                }
            }
            else
            {
                if (currentSpeed < speed)
                    currentSpeed += acceleration * Time.deltaTime;
                else
                    currentSpeed = speed;

                coords.direction = dir;

                //Update the plane's position.
                coords.position += coords.direction.normalized * currentSpeed * Time.fixedDeltaTime;

                //Store the new position in the coords.
                _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
            }


            if ((targetPosition - coords.position).magnitude < 2f)
            {
                if (currentSpeed > 0.5)
                {
                    currentSpeed -= acceleration * 3 * Time.deltaTime;
                }
                if((targetPosition - coords.position).magnitude < 0.1f)
                {
                    currentSpeed = 0;
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
            throw new System.NotImplementedException();
        }

        public Vector2 Rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }

    }

}
