using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;
using UnityEngine.Audio;

namespace OceanEntities
{
    public class Plane : PlayerOceanEntity
    {
        private Transform _transform;
        private float currentSpeed = 0;

        //Waiting routine variables
        [Header("Waiting routine")]
        public float windSpeedModulo;
        public float minimumSpeed;
        private float defaultMaxSpeed;
        private Vector2 waitingPoint = new Vector2(-9999,-9999);
        bool isWaiting;

        [Header("Audio")]
        private SoundHandler soundHandler;
        public AudioSource audioSource;
        public AudioMixerGroup targetGroup;
        public AudioClip movementSound;

        /*private Vector2 waitingTarget = new Vector2(-9999, -9999);

        private Vector2 tempTarget;
        private Vector2 planeToCenter;

        private float radius = 1;
        private float waitingRoutineRadius = 0;


        private bool canWait;
        */

        [Header("Equipment")]
        public Equipement passiveEquipement;
        public Equipement activeEquipement;

        void Start()
        {
            movementType = MovementType.air;
            _transform = transform;

            coords.position = Coordinates.ConvertWorldToVector2(_transform.position);
            coords.direction = Coordinates.ConvertWorldToVector2(transform.forward);
            currentTargetPoint = coords.position + Coordinates.ConvertWorldToVector2(_transform.forward * 2);

            environment = GameManager.Instance.levelManager.environnement;
            soundHandler = GameManager.Instance.soundHandler;
            defaultMaxSpeed = speed;
            //Equipment initialization.
            passiveEquipement.Init(this);
            activeEquipement.Init(this);

            soundHandler.PlaySound(movementSound, audioSource, targetGroup);
        }


        private void Update()
        {
           if (passiveEquipement.readyToUse)
                passiveEquipement.UseEquipement(this);
        }

        void FixedUpdate()
        {
            if(currentTargetPoint != nullVector)
            {
                Move(currentTargetPoint);
                if (waitingPoint != nullVector)
                {
                    waitingPoint = nullVector;
                    isWaiting = false;
                }
            }
            else
            {
                Waiting();
            }
        }

        public override void Move(Vector2 targetPosition)
        {
            //canWait = false;
            
            //Acceleration
            if(!isWaiting)
            {
                if (currentSpeed < speed)
                    currentSpeed += acceleration * Time.fixedDeltaTime;
                else
                    currentSpeed = speed;
            }

            //Calculate direction to target and store it in coords.
            Vector2 dir = targetPosition - coords.position;
            if (Vector2.Angle(coords.direction, dir) > Time.fixedDeltaTime * rotateSpeed)
            {
                int turnSide = Vector2.SignedAngle(coords.direction, dir) > 0 ? 1 : -1;
                if (isWaiting)
                    turnSide = -1;

                float currentAngle = Vector2.SignedAngle(Vector2.right, coords.direction) + turnSide * Time.fixedDeltaTime * rotateSpeed;
                coords.direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));
                _transform.rotation = Quaternion.Euler(transform.rotation.x, -currentAngle + 90, transform.rotation.z);
            }
            else
            {
                if (dir == Vector2.zero)
                    dir = Coordinates.ConvertWorldToVector2(transform.forward);

                coords.direction = dir;
            }

            if(!isWaiting)
            {
                int temp = environment.ZoneIn(coords.position) - 1;
                if(temp >0)
                {
                    currentZone = environment.zones[temp];
                    currentZoneState = currentZone.state;
                }
                if (currentZoneState == ZoneState.WindyZone)
                {
                    Vector3 windDirection = Quaternion.Euler(0, Mathf.Rad2Deg * currentZone.windDir, 0) * Vector3.down;

                    float dot = Vector2.Dot(new Vector2(windDirection.x, windDirection.y).normalized, coords.direction.normalized);
                    if (dot > 0.3f)
                    {
                        //speed Up plane
                        speed = defaultMaxSpeed + windSpeedModulo;
                    }
                    else
                    {
                        //slow down Plane
                        speed = defaultMaxSpeed - windSpeedModulo;
                    }
                }
                else
                {
                    speed = defaultMaxSpeed;
                }
            }

            //Update the plane's position.
            coords.position += coords.direction.normalized * currentSpeed * Time.fixedDeltaTime;

            //Store the new position in the coords.
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

            _transform.forward = Coordinates.ConvertVector2ToWorld(coords.direction);

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
        
        public override void Waiting()
        {
            isWaiting = true;

            //Place center point of routine if not placed aswell as new routine targets.
            if (waitingPoint == nullVector)
            {
                waitingPoint = coords.position;

                //waitingTarget = Coordinates.ConvertWorldToVector2(_transform.position) + new Vector2(_transform.forward.x, _transform.forward.z) * radius;
                //tempTarget = Coordinates.ConvertWorldToVector2(_transform.position) - new Vector2(_transform.forward.x, _transform.forward.z) * radius;
                //waitingRoutineRadius = 0;
            }
            
            Move(waitingPoint);

            /*else
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

                    if(new Vector3(coords.direction.x, 0, coords.direction.y) == Vector3.zero)
                        _transform.forward = new Vector3(coords.direction.x, 0, coords.direction.y);
                    else
                    {
                        _transform.forward = new Vector3(coords.direction.x + 0.001f, 0, coords.direction.y + 0.001f);
                    }
                }
                else
                {
                    if(waitingTarget != nullVector)
                    {
                        Move(tempTarget);
                        currentSpeed = minimumSpeed;
                    }
                }
            }*/
        }

        public void UseActiveObject()
        {
            if (activeEquipement.readyToUse && activeEquipement.chargeCount > 0)
                activeEquipement.UseEquipement(this);
        }
    }
}
