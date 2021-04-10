using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class Helicopter : PlayerOceanEntity
    {
        private Transform _transform;
        private float currentSpeed = 0;

        [Header("Helicopter Flight")]
        public float preparationDuration;
        public float alertDuration;
        public float flightDuration;

        [HideInInspector]public bool inAlert;
        [HideInInspector]public bool inFlight;
        private bool onShip = true;

        //linkedShip must be linked when the helicopter is instanciated
        public Ship linkedShip;

        void Start()
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

        }

        public override void Move(Vector2 targetPosition)
        {

            if (currentSpeed < speed)
                currentSpeed += acceleration * Time.deltaTime;
            else
                currentSpeed = speed;

            //Calculate direction to target and store it in coords.
            coords.direction = targetPosition - coords.position;

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
