using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;

namespace OceanEntities
{
    public class Helicopter : PlayerOceanEntity
    {
        private Transform _transform;
        private float currentSpeed = 0;
        private float currentRotateSpeed =0;

        [Header("Helicopter Flight")]
        public float preparationDuration;
        public float alertDuration;
        public float flightDuration;

        [HideInInspector]public bool inAlert;
        [HideInInspector]public bool inFlight;
        private bool onShip = true;

        //linkedShip must be linked when the helicopter is instanciated
        public Ship linkedShip;

        [Header("Equipement")]
        public Equipement activeEquipment;
        private float time;
        public float flashPreparationTime;

        private void Start()
        {
            _transform = transform;
            movementType = MovementType.air;
        }

        void Update()
        {
            if(currentTargetPoint != nullVector && inFlight)
            {
                Move(currentTargetPoint);
            }

            //If flight ended then go back to the ship
            if(!inFlight && !onShip)
            {
                Move(linkedShip.coords.position);
            }

            //Implement use of Sonic Flash
            if(Input.touchCount > 0 && inFlight && currentTargetPoint == nullVector)
            {
                Vector2 touchPos = GameManager.Instance.inputManager.GetSeaPosition();
                if((touchPos -  coords.position).magnitude < 0.5f)
                {
                    time += Time.deltaTime;
                    if (time >= flashPreparationTime && activeEquipment.readyToUse && activeEquipment.chargeCount > 0)
                    {
                        print("launch flash");
                        activeEquipment.UseEquipement(coords);
                    }
                }
                else
                {
                    time = 0;
                }
                
            }
            else
            {
                time = 0;
            }


        }

        public override void Move(Vector2 targetPosition)
        {

            if (currentSpeed < speed)
                currentSpeed += acceleration * Time.deltaTime;
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
            coords.position += coords.direction.normalized * currentSpeed * Time.deltaTime;

            //Store the new position in the coords.
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

            if ((targetPosition - coords.position).magnitude < 0.1f)
            {
                currentTargetPoint = nullVector;
                currentSpeed = 0;
            
                if (!inFlight)
                {
                    onShip = true;
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

        public void TakeOff()
        {
            inFlight = true;
            onShip = false;
            StartCoroutine(FlightCoroutine());
        }

        public void LauchButton()
        {
            StartCoroutine(PreparationCoroutine());
        }

        #region Coroutines

        private IEnumerator PreparationCoroutine()
        {
            yield return new WaitForSeconds(preparationDuration);
            StartCoroutine(AlertCoroutine());
        }

        private IEnumerator AlertCoroutine()
        {
            inAlert = true;
            StartCoroutine(AlertTimer());

            //Wait until not in alert anymore or current selected entity is this one
            yield return new WaitUntil(() => !inAlert || GameManager.Instance.playerController.currentSelectedEntity == this);
            if(inAlert)
            {
                TakeOff();
                inAlert = false;
            }
        }

        private IEnumerator AlertTimer()
        {
            yield return new WaitForSeconds(alertDuration);
            inAlert = false;
        }

        private IEnumerator FlightCoroutine()
        {
            yield return new WaitForSeconds(flightDuration);

            inFlight = false;
        }

        #endregion  
    }
}
