﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;
using UnityEngine.UI;

namespace OceanEntities
{
    public class Helicopter : PlayerOceanEntity
    {
        private Transform _transform;
        private float currentSpeed = 0;
        private float currentRotateSpeed =0;

        [Header("Renderer")]
        public GameObject helicopterRenderer;
        public HelicopterFeedback helicopterFeedback;
        [HideInInspector] public HelicopterDeckUI deckUI;

        [Header("Helicopter Flight")]
        public float preparationDuration;
        public float alertDuration;
        public float flightDuration;
        public float cooldownDuration;

        [HideInInspector] public bool operating;  
        [HideInInspector] public bool inAlert;
        [HideInInspector] public bool inFlight;
        [HideInInspector] public bool launch;
        private bool onShip = true;

        [HideInInspector] public Button launchButton;

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

            if(onShip)
            {
                _transform.position = linkedShip.transform.position;

                if (helicopterRenderer.activeSelf)
                {
                    //Play landing particles
                    helicopterRenderer.SetActive(false);
                    GameManager.Instance.inputManager.getEntityTarget = false;
                    GameManager.Instance.uiHandler.entitiesSelectionUI.UpdateButtons(true);
                    if(deckUI != null)
                    {
                        StartCoroutine(Cooldown());
                    }
                }
            }

            else if(currentTargetPoint != nullVector && inFlight)
            {
                Move(currentTargetPoint);
            }

            //If flight ended then go back to the ship
            else if(!inFlight && !onShip)
            {
                Move(linkedShip.coords.position);
            }

            //Implement use of Sonic Flash
            if(Input.touchCount > 0 && inFlight && currentTargetPoint == nullVector)
            {
                Vector2 touchPos = GameManager.Instance.inputManager.GetSeaPosition();
                if ((touchPos -  coords.position).magnitude < 0.5f)
                {
                    time += Time.deltaTime;
                    if (time >= flashPreparationTime && activeEquipment.readyToUse && activeEquipment.chargeCount > 0)
                    {
                        print("launch flash");
                        activeEquipment.UseEquipement(this);
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
            
                //Land on Ship
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

            _transform.position = linkedShip.transform.position;
            coords.position = Coordinates.ConvertWorldToVector2(_transform.position);

            helicopterRenderer.SetActive(true);
            StartCoroutine(FlightCoroutine());
        }

        public void LaunchButton()
        {
            operating = true;
            StartCoroutine(PreparationCoroutine());
        }

        #region Coroutines

        private IEnumerator PreparationCoroutine()
        {
            deckUI.UpdateStatusText("Preparing launch...");
            StartCoroutine(deckUI.FillBar(preparationDuration, 1));
            StartCoroutine(deckUI.FillIcon(preparationDuration, 1));
            yield return new WaitForSeconds(preparationDuration);
            StartCoroutine(AlertCoroutine());
        }

        private IEnumerator AlertCoroutine()
        {
            deckUI.UpdateStatusText("Launch !");
            StartCoroutine(deckUI.FillIcon(alertDuration, 0));
            inAlert = true;
            StartCoroutine(AlertTimer());

            //Wait until not in alert anymore or current selected entity is this one
            yield return new WaitUntil(() => !inAlert || launch);
            if(inAlert)
            {
                deckUI.UpdateStatusText("Drop Flash !");
                TakeOff();
                inAlert = false;
                launch = false;
                GameManager.Instance.inputManager.getEntityTarget = true;
                GameManager.Instance.uiHandler.entitiesSelectionUI.UpdateButtons(false);
            }
            else
            {
                StartCoroutine(Cooldown());
            }
        }

        private IEnumerator AlertTimer()
        {
            yield return new WaitForSeconds(alertDuration);
            inAlert = false;
        }

        private IEnumerator FlightCoroutine()
        {
            StartCoroutine(deckUI.FillBar(flightDuration, 0));
            yield return new WaitForSeconds(flightDuration);
            GameManager.Instance.inputManager.getEntityTarget = false;
            inFlight = false;
        }

        private IEnumerator Cooldown()
        {
            StartCoroutine(deckUI.FillBar(cooldownDuration, 0));
            deckUI.UpdateStatusText("Cooling down ...");
            yield return new WaitForSeconds(cooldownDuration);
            deckUI.UpdateStatusText("Prepare Launch");
            operating = false;
        }

        #endregion  
    }
}
