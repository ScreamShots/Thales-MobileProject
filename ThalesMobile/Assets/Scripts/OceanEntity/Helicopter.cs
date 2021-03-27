using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class Helicopter : PlayerOceanEntity
    {
        private Transform _transform;
        [HideInInspector] public Transform currentTargetPoint;

        public float preparationDuration;
        public float alertDuration;
        public float flightDuration;

        [HideInInspector]public bool inAlert;
        [HideInInspector]public bool inFlight;
        private bool onShip;

        //linkedShip must be linked when the helicopter is instanciated
        [HideInInspector] public Ship linkedShip;

        void Start()
        {
            _transform = transform;
            movementType = MovementType.air;
        }

        void Update()
        {
            if(currentTargetPoint != null && inFlight)
            {
                Move(currentTargetPoint.position);
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
            //Calculate direction to target and store it in coords.
            coords.direction = targetPosition - (Vector2)_transform.position;

            //Update the plane's position.
            _transform.position += (Vector3)coords.direction.normalized * speed * Time.deltaTime;

            //Store the new position in the coords.
            coords.position = _transform.position;

            if(coords.position == targetPosition)
            {
                currentTargetPoint = null;

                if(!inFlight)
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
            yield return new WaitUntil(() => !inAlert);
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
