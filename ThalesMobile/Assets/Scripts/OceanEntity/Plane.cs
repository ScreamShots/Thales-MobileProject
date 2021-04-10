using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;

namespace OceanEntities
{
    public class Plane : PlayerOceanEntity
    {
        private Transform _transform;
        private float currentSpeed = 0;
        private float currentRotateSpeed;


        //Waiting routine variables
        [Header("Waiting routine")]
        private Vector2 waitingPoint = new Vector2(-9999,-9999);
        private Vector2 waitingTarget = new Vector2(-9999, -9999);

        private Vector2 tempTarget;
        private Vector2 planeToCenter;

        private float radius = 1;
        private float waitingRoutineRadius = 0;
        public float minimumSpeed;


        private bool canWait;


        [Header("Equipment")]
        public Equipement passiveEquipement;
        public Equipement activeEquipement;

        void Start()
        {
            movementType = MovementType.air;
            _transform = transform;
            currentTargetPoint = Coordinates.ConvertWorldToVector2(_transform.forward * 2);

            //Equipment initialization.
            passiveEquipement.Init();
            activeEquipement.Init();
        }


        private void Update()
        {
            if (passiveEquipement.readyToUse && passiveEquipement.chargeCount > 0)
                passiveEquipement.UseEquipement(coords);
        }

        void FixedUpdate()
        {
            if(currentTargetPoint != nullVector)
            {
                Move(currentTargetPoint);
                if (waitingPoint != nullVector)
                {
                    waitingPoint = nullVector;
                    canWait = false;
                }
            }
            else
            {
                Waiting();
            }
        }

        public override void Move(Vector2 targetPosition)
        {
            canWait = false;
            
            //Acceleration
            if (currentSpeed < speed)
                currentSpeed += acceleration * Time.fixedDeltaTime;
            else
                currentSpeed = speed;

            //Calculate direction to target and store it in coords.
            Vector2 dir = targetPosition - coords.position;
            if (Vector2.Angle(coords.direction, dir) > Time.fixedDeltaTime * rotateSpeed)
            {
                currentRotateSpeed += acceleration * Time.fixedDeltaTime;
                int turnSide = Vector2.SignedAngle(coords.direction, dir) > 0 ? 1 : -1;
                float currentAngle = Vector2.SignedAngle(Vector2.right, coords.direction) + turnSide * Time.fixedDeltaTime * currentRotateSpeed;
                coords.direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                _transform.rotation = Quaternion.Euler(transform.rotation.x, -currentAngle + 90, transform.rotation.z);
            }
            else
            {
                currentRotateSpeed = rotateSpeed;
                coords.direction = dir;
            }

            //Update the plane's position.
            coords.position += coords.direction.normalized * currentSpeed * Time.fixedDeltaTime;

            //Store the new position in the coords.
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

            //Deceleration
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
            Debug.DrawRay(Coordinates.ConvertVector2ToWorld(waitingTarget), Vector3.up, Color.red);
            
            Debug.DrawRay(Coordinates.ConvertVector2ToWorld(waitingPoint), Vector3.up, Color.green);

            Debug.DrawRay(Coordinates.ConvertVector2ToWorld(tempTarget), Vector3.up, Color.blue);

            //Place center point of routine if not placed aswell as new routine targets.
            if (waitingPoint == nullVector)
            {
                waitingPoint = coords.position;

                waitingTarget = Coordinates.ConvertWorldToVector2(_transform.position) + new Vector2(_transform.forward.x, _transform.forward.z) * radius;
                tempTarget = Coordinates.ConvertWorldToVector2(_transform.position) - new Vector2(_transform.forward.x, _transform.forward.z) * radius;
                waitingRoutineRadius = 0;
            }
            else
            {
                planeToCenter = coords.position - waitingPoint;
                Vector2 dotReference = waitingPoint + waitingTarget;

                float dot = Vector3.Dot(Coordinates.ConvertVector2ToWorld(coords.direction).normalized, Coordinates.ConvertVector2ToWorld(dotReference).normalized);

                if (dot < -0.9f || canWait)
                {
                    canWait = true;
                    
                    if (waitingRoutineRadius == 0)
                    {
                        waitingRoutineRadius = planeToCenter.magnitude;
                    }
;
                    Vector2 dir = new Vector2(planeToCenter.y, -planeToCenter.x);
                    
                    if(Vector2.SignedAngle(coords.direction, planeToCenter) < 0)
                    {
                        dir = -dir;
                    }

                    if (Vector2.Angle(coords.direction, dir) > Time.fixedDeltaTime * rotateSpeed)
                    {
                        currentRotateSpeed += acceleration * Time.fixedDeltaTime;
                        int turnSide = Vector2.SignedAngle(coords.direction, dir) > 0 ? 1 : -1;
                        float currentAngle = Vector2.SignedAngle(Vector2.right, coords.direction) + turnSide * Time.fixedDeltaTime * rotateSpeed;
                        coords.direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                    }
                    else
                    {
                        currentRotateSpeed = rotateSpeed;
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

                    coords.position += (planeToCenter.normalized * waitingRoutineRadius) - planeToCenter;

                    _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
                    _transform.forward = new Vector3(coords.direction.x, 0, coords.direction.y);
                }
                else
                {
                    if(waitingTarget != nullVector)
                    {
                        Move(tempTarget);
                        currentSpeed = minimumSpeed;
                    }
                }
            }
        }

        public void UseActiveObject()
        {
            if (activeEquipement.readyToUse && activeEquipement.chargeCount > 0)
                activeEquipement.UseEquipement(coords);
        }
    }
}
