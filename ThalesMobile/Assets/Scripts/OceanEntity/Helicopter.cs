using System.Collections;
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
        private float currentRotateSpeed;

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
        [HideInInspector] public bool isDroppingFlash;
        private bool onShip = true;

        [HideInInspector] public Button launchButton;

        //linkedShip must be linked when the helicopter is instanciated
        public Ship linkedShip;

        [Header("Equipement")]
        public Equipement activeEquipement;
        private float time;
        public float flashPreparationTime;

        private void Start()
        {
            _transform = transform;
            movementType = MovementType.air;

            currentRotateSpeed = rotateSpeed;
            coords.direction = Coordinates.ConvertWorldToVector2(transform.forward);

            activeEquipement.Init(this);
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
                }
            }

            else if(currentTargetPoint != nullVector && inFlight && !isDroppingFlash)
            {
                Move(currentTargetPoint);
            }

            //If flight ended then go back to the ship
            else if(!inFlight && !onShip)
            {
                if(!isDroppingFlash)
                Move(linkedShip.coords.position);
            }

            //Implement use of Sonic Flash
            if(Input.touchCount > 0 && inFlight && currentTargetPoint == nullVector)
            {
                Vector2 touchPos = GameManager.Instance.inputManager.GetSeaPosition();
                if ((touchPos -  coords.position).magnitude < 0.5f)
                {
                    time += Time.deltaTime;
                    if (time >= flashPreparationTime && activeEquipement.readyToUse && activeEquipement.chargeCount > 0)
                    {
                        activeEquipement.UseEquipement(this);
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

                if (!inFlight)
                {
                    //Update the plane's position.
                    coords.position += coords.direction.normalized * currentSpeed * Time.deltaTime;
                }
            }

            if (inFlight)
            {
                //Update the plane's position.
                coords.position += coords.direction.normalized * currentSpeed * Time.deltaTime;
            }

            //Store the new position in the coords.
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);

            _transform.forward = Coordinates.ConvertVector2ToWorld(coords.direction);

            if ((targetPosition - coords.position).magnitude < 0.1f)
            {
                currentTargetPoint = nullVector;
                currentSpeed = 0;
            
                //Land on Ship
                if (!inFlight)
                {
                    onShip = true;
                    LandFeedback();
                    operating = false;
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

        public void LandFeedback()
        {
            deckUI.UpdateSecondaryButton(HelicopterButtonState.Start);
            deckUI.DeactivateButton();

            deckUI.UpdateStatusText("Prepare Launch");
            deckUI.fillBar.fillAmount = 0;
            deckUI.percentageText.text = "0 %";
        }

        #region Coroutines

        private IEnumerator PreparationCoroutine()
        {
            deckUI.UpdateStatusText("Preparing launch...");
            StartCoroutine(deckUI.FillBar(preparationDuration, 1));
            yield return new WaitForSeconds(preparationDuration);

            deckUI.UpdateSecondaryButton(HelicopterButtonState.Launch);

            StartCoroutine(AlertCoroutine());
        }

        private IEnumerator AlertCoroutine()
        {
            deckUI.UpdateStatusText("Launch !");
            inAlert = true;
            StartCoroutine(AlertTimer());

            //Wait until not in alert anymore or current selected entity is this one
            yield return new WaitUntil(() => !inAlert || launch);
            if(inAlert)
            {
                deckUI.UpdateStatusText("Drop Flash !");
                deckUI.ActivateButton();

                TakeOff();

                inAlert = false;
                launch = false;

                GameManager.Instance.inputManager.getEntityTarget = true;
                GameManager.Instance.uiHandler.entitiesSelectionUI.UpdateButtons(false);
            }
            else
            {
                LandFeedback();
            }
        }

        private IEnumerator AlertTimer()
        {
            yield return new WaitForSeconds(alertDuration);
            inAlert = false;
        }

        private IEnumerator FlightTimer()
        {
            yield return new WaitForSeconds(flightDuration);
            inFlight = false;
        }

        private IEnumerator FlightCoroutine()
        {
            Coroutine timer = StartCoroutine(FlightTimer());

            StartCoroutine(deckUI.FillBar(flightDuration, 0));
            deckUI.UpdateSecondaryButton(HelicopterButtonState.Return);

            yield return new WaitUntil(() => !inFlight);

            StopCoroutine(timer);

            GameManager.Instance.inputManager.getEntityTarget = false;
        }

        #endregion  
    }
}
