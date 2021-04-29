using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;
namespace OceanEntities
{
    public class Ship : PlayerOceanEntity
    {
        private Transform _transform;
        private float currentSpeed = 0;

        [Header("Equipment")]
        public Equipement passiveEquipement;
        public Equipement activeEquipement;

        private void Start()
        {
            movementType = MovementType.sea;
            _transform = transform;

            coords = new Coordinates(_transform.position, Vector2.up, 0);

            coords.direction = Coordinates.ConvertWorldToVector2(_transform.forward);

            //Equipment initialization.
            passiveEquipement.Init(this);
            activeEquipement.Init(this);
        }

        private void Update()
        {
            if (passiveEquipement.readyToUse)
                passiveEquipement.UseEquipement(this);

            if (currentTargetPoint != nullVector)
            {
                UpdatePath();
            }
        }

        void FixedUpdate()
        {
            if (currentTargetPoint != nullVector)
            {
                Move(currentTargetPoint);
                pathDestination = currentTargetPoint;
            }
        }
        public override void Move(Vector2 targetPosition)
        {
            //Calculate direction to target and store it in coords.
            //Vector2 dir = targetPosition - coords.position;    without pathfinding
            Vector2 dir = pathDirection;
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

                if (dir == Vector2.zero)
                    coords.direction = Coordinates.ConvertWorldToVector2(transform.forward);
                else
                    coords.direction = dir;

                //Update the plane's position.
                coords.position += coords.direction.normalized * currentSpeed * Time.fixedDeltaTime;

                //Store the new position in the coords.
                _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

                _transform.forward = Coordinates.ConvertVector2ToWorld(coords.direction);
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

        public override void Waiting()
        {
            throw new System.NotImplementedException();
        }

        public void UseActiveObject()
        {
            if (activeEquipement.readyToUse && activeEquipement.chargeCount > 0)
                activeEquipement.UseEquipement(this);
        }

    }

}
