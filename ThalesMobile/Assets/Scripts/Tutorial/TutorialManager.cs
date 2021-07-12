using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NaughtyAttributes;
using OceanEntities;

public class TutorialManager : MonoBehaviour
{
    private GameManager gameManager;
    private InputManager inputManager;
    private UIHandler uiHandler;
    private CameraController cameraController;
    private LevelManager levelManager;
    private PlayerController playerController;


    //Entities
    private Ship tutorialShip;
    private OceanEntities.Plane tutorialPlane;
    private Helicopter tutorialHelicopter;
    private Submarine tutorialSubmarine;

    public CameraSettings cameraSettings;

    [Header("UI")]
    public GameObject opaquePanel;
    [Space]
    public GameObject textContainer_1;
    public TextMeshProUGUI screenText_1;
    public GameObject hand_1;
    [Space]
    public GameObject cameraTargetScreen_2;
    public GameObject textContainer_2;
    public TextMeshProUGUI screenText_2;
    public GameObject hand_2;
    [Space]
    public GameObject textContainer_3;
    public TextMeshProUGUI screenText_3;
    public GameObject hand_3;
    [Space]
    public GameObject textContainer_4a;
    public TextMeshProUGUI screenText_4a;
    public GameObject textContainer_4b;
    public TextMeshProUGUI screenText_4b;
    public GameObject hand_4;
    [Space]
    public GameObject textContainer_5;
    public TextMeshProUGUI screenText_5;
    public GameObject hand_5;
    [Space]
    public GameObject textContainer_5bis;
    public TextMeshProUGUI screenText_5bis;
    public GameObject hand_5bis;
    [Space]
    public GameObject targetScreen_6;
    public GameObject textContainer_6a;
    public TextMeshProUGUI screenText_6a;
    public GameObject textContainer_6b;
    public TextMeshProUGUI screenText_6b;
    public GameObject hand_6;
    [Space]
    public GameObject textContainer_7;
    public TextMeshProUGUI screenText_7;
    public GameObject hand_7;
    [Space]
    public GameObject textContainer_8;
    public TextMeshProUGUI screenText_8;
    public GameObject hand_8;
    [Space]
    public GameObject textContainer_9;
    public TextMeshProUGUI screenText_9;
    public GameObject hand_9;
    [Space]
    public GameObject textContainer_10;
    public TextMeshProUGUI screenText_10;
    public GameObject shipTargetScreen_10;
    public GameObject planeTargetScreen_10;
    [Space]
    public GameObject textContainer_11;
    public TextMeshProUGUI screenText_11;
    public GameObject hand_11;
    [Space]
    public GameObject textContainer_12;
    public TextMeshProUGUI screenText_12;
    [Space]
    public GameObject textContainer_13;
    public TextMeshProUGUI screenText_13;
    public GameObject hand_13;
    [Space]
    public GameObject textContainer_13bis;
    public TextMeshProUGUI screenText_13bis;
    public DetectableOceanEntity pointToZoom;

    [Space]
    public GameObject textContainer_14a;
    public TextMeshProUGUI screenText_14a;
    public GameObject textContainer_14b;
    public TextMeshProUGUI screenText_14b;
    public GameObject hand_14;

    [Space]
    public GameObject textContainer_15;
    public TextMeshProUGUI screenText_15;
    public DetectableOceanEntity pointToDetect;
    public GameObject hand_15;

    [Space]
    public GameObject textContainer_16;
    public TextMeshProUGUI screenText_16;
    public DetectableOceanEntity pointToReveal;

    [Space]
    public GameObject textContainer_17;
    public TextMeshProUGUI screenText_17;

    [Space]
    public GameObject textContainer_18;
    public TextMeshProUGUI screenText_18;
    public CanvasGroup descriptionCanvasGroup;
    public GameObject hand_18;

    [Space]
    public GameObject textContainer_19;
    public TextMeshProUGUI screenText_19;

    [Space]
    public GameObject textContainer_20;
    public TextMeshProUGUI screenText_20;
    public InterestPoint interestPointToFocus;

    [Space]
    public GameObject textContainer_21;
    public TextMeshProUGUI screenText_21;
    public GameObject hand_21;

    [Space]
    public GameObject textContainer_22;
    public TextMeshProUGUI screenText_22;
    public GameObject hand_22;

    [Space]
    public GameObject textContainer_23;
    public TextMeshProUGUI screenText_23;
    public GameObject hand_23;

    [Space]
    public GameObject textContainer_24;
    public TextMeshProUGUI screenText_24;
    public GameObject hand_24;

    [Space]
    public GameObject textContainer_25;
    public TextMeshProUGUI screenText_25;
    public GameObject newSubPos;

    [Space]
    public GameObject textContainer_26;
    public TextMeshProUGUI screenText_26;
    public GameObject hand_26;

    [Space]
    public GameObject textContainer_27;
    public TextMeshProUGUI screenText_27;
    public GameObject hand_27;

    [Space]
    public GameObject textContainer_29;
    public TextMeshProUGUI screenText_29;

    [Space]
    public GameObject textContainer_30;
    public TextMeshProUGUI screenText_30;
    public GameObject hand_30;

    [Space]
    public GameObject textContainer_31;
    public TextMeshProUGUI screenText_31;
    public CanvasGroup victoryCanvasGroup;

    [Space]
    public GameObject textContainer_32;
    public TextMeshProUGUI screenText_32;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        inputManager = gameManager.inputManager;
        uiHandler = gameManager.uiHandler;
        cameraController = gameManager.cameraController;
        levelManager = gameManager.levelManager;
        playerController = gameManager.playerController;

        for (int i = 0; i < levelManager.playerOceanEntities.Count; i++)
        {
            if(levelManager.playerOceanEntities[i].GetType() == typeof(Ship))
            {
                tutorialShip = (Ship)levelManager.playerOceanEntities[i];
            }
            else if(levelManager.playerOceanEntities[i].GetType() == typeof(OceanEntities.Plane))
            {
                tutorialPlane = (OceanEntities.Plane)levelManager.playerOceanEntities[i];
            }
            else if(levelManager.playerOceanEntities[i].GetType() == typeof(Helicopter))
            {
                tutorialHelicopter = (Helicopter)levelManager.playerOceanEntities[i];
            }
        }

        tutorialSubmarine = levelManager.submarine;

        StartCoroutine(TutorialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TutorialCoroutine()
    {
        //Initialize elements to be disabled
        inputManager.canUseCam = false;
        inputManager.canZoomCam = false;

        cameraController.camSett = cameraSettings;
        cameraController.SetZoom(0.5f, 1);

        uiHandler.entitiesSelectionUI.gameObject.SetActive(false);
        uiHandler.entityDeckUI.gameObject.SetActive(false);
        uiHandler.submarineUI.gameObject.SetActive(false);
        uiHandler.entitiesSelectionUI.helicopterSelectionParent.SetActive(false);
        uiHandler.entitiesSelectionUI.buttons[0].gameObject.SetActive(false);

        tutorialPlane.gameObject.SetActive(false);
        tutorialShip.gameObject.SetActive(false);
        tutorialSubmarine.gameObject.SetActive(false);

        List<InterestPoint> interestPoints = levelManager.interestPointsToHack;

        for (int i = 0; i < interestPoints.Count; i++)
        {
            interestPoints[i].gameObject.SetActive(false);
        }

        InteractableUI shipEquipementCard = tutorialShip.entityDeck.GetComponent<Deck>().equipementCard;
        InteractableUI shipMovementCard = tutorialShip.entityDeck.GetComponent<Deck>().movementCard;
        InteractableUI shipPassiveCard = tutorialShip.entityDeck.GetComponent<Deck>().passiveCard;

        InteractableUI planeEquipementCard = tutorialPlane.entityDeck.GetComponent<Deck>().equipementCard;
        InteractableUI planeMovementCard = tutorialPlane.entityDeck.GetComponent<Deck>().movementCard;
        InteractableUI planePassiveCard = tutorialPlane.entityDeck.GetComponent<Deck>().passiveCard;

        shipEquipementCard.gameObject.SetActive(false);
        planeEquipementCard.gameObject.SetActive(false);

        #region Screen 1 : Tutorial Introduction
        opaquePanel.SetActive(true);
        textContainer_1.SetActive(true);
        hand_1.SetActive(true);
        screenText_1.text = "Welcome to this training session.";
        inputManager.canMoveCam = false;

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_1.SetActive(false);
        textContainer_1.SetActive(false);
        opaquePanel.SetActive(false);
        #endregion

        #region Screen 2 : Camera Movement

        inputManager.canUseCam = true;
        inputManager.canMoveCam = true;
        textContainer_2.SetActive(true);
        hand_2.SetActive(true);
        screenText_2.text = "Move the camera by sliding your finger across the screen.";

        cameraTargetScreen_2.transform.position = cameraController.focusPoint.position;

        yield return new WaitUntil(() => (cameraController.focusPoint.position - cameraTargetScreen_2.transform.position).magnitude > 5f);

        textContainer_2.SetActive(false);
        hand_2.SetActive(false);

        #endregion

        #region Screen 3 : Camera Zoom
        inputManager.canUseCam = false;
        inputManager.canZoomCam = true;

        textContainer_3.SetActive(true);
        screenText_3.text = "Zoom in and out by pinching the screen.";
        hand_3.SetActive(true);

        yield return new WaitUntil(() => cameraController.zoomIntensity < 0.4);
        yield return new WaitUntil(() => cameraController.zoomIntensity > 0.6);

        inputManager.canZoomCam = false;
        cameraController.SetZoom(1, 1);

        yield return new WaitForSeconds(1f);

        textContainer_3.SetActive(false);
        hand_3.SetActive(false);
        #endregion

        #region Screen 4 : Ship Apparition
        textContainer_4a.SetActive(true);
        textContainer_4b.SetActive(true);

        screenText_4a.text = "This is the <b>FREMM</b>, a frigate dedicated to anti submarine warfare <b>(ASW)</b>.";
        screenText_4b.text = "<b>Press</b> the FREMM to select it.";
        hand_4.SetActive(true);

        tutorialShip.gameObject.SetActive(true);

        inputManager.canUseCam = true;
        inputManager.canMoveCam = false;

        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialShip);

        inputManager.canUseCam = false;
        inputManager.canMoveCam = true;

        textContainer_4a.SetActive(false);
        textContainer_4b.SetActive(false);

        hand_4.SetActive(false);
        #endregion

        #region Screen 5 : Ship UI Apparition
        textContainer_5.SetActive(true);
        screenText_5.text = "You can also <b>select</b> it from the <b> Assets Menu</b>.";

        uiHandler.entitiesSelectionUI.gameObject.SetActive(true);
        uiHandler.entitiesSelectionUI.currentButton = null;
        playerController.currentSelectedEntity = null;
        hand_5.SetActive(true);
        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialShip);

        cameraController.SetZoom(0.5f,1f);
        textContainer_5.SetActive(false);
        hand_5.SetActive(false);

        yield return new WaitForSeconds(1f);

        #endregion

        #region Screen 5 bis : Double click on UI
        textContainer_5bis.SetActive(true);
        screenText_5bis.text = "If the selected asset is out of range, you can centre the camera <b>centre the camera</b> on it by selecting it again from the Assets Menu. ";   
        hand_5bis.SetActive(true);
        playerController.currentSelectedEntity = null;

        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialShip);
        cameraController.SetTarget(tutorialShip.transform);

        textContainer_5bis.SetActive(false);
        hand_5bis.SetActive(false);

        yield return new WaitForSeconds(1);

        #endregion

        #region Screen 6 : DeckUI Apparition
        textContainer_6a.SetActive(true);
        textContainer_6b.SetActive(true);
        screenText_6a.text = "Every asset uses <b>equipment</b> to perform their actions";
        screenText_6b.text = "<b>Press</b> and <b>drag</b> the <b>movement Card</b> on the target to move the FREMM accordingly.";

        uiHandler.entityDeckUI.gameObject.SetActive(true);
        uiHandler.entityDeckUI.currentDeck = null;
        uiHandler.entityDeckUI.UpdateCurrentDeck(uiHandler.entitiesSelectionUI.currentButton.linkedEntity.entityDeck);
        hand_6.SetActive(true);

        yield return new WaitForEndOfFrame();


        shipMovementCard.canClick = false;
        shipPassiveCard.canHold = false;

        targetScreen_6.SetActive(true);
        
        Vector2 currentTarget = Coordinates.ConvertWorldToVector2(targetScreen_6.transform.position);
        yield return new WaitUntil(() => (playerController.currentSelectedEntity.currentTargetPoint  - currentTarget).magnitude < 3);

        hand_6.SetActive(false);
        shipMovementCard.canDrag = false;
        shipMovementCard.canClick = false;

        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint == playerController.currentSelectedEntity.nullVector);

        targetScreen_6.SetActive(false);
        textContainer_6a.SetActive(false);
        textContainer_6b.SetActive(false);
        #endregion

        #region Screen 7 : Click on card
        textContainer_7.SetActive(true);
        screenText_7.text = "You can also <b>press</b> on the card to <b>select</b> it.";
        hand_7.SetActive(true);
        shipMovementCard.canClick = true;

        yield return new WaitUntil(() => shipMovementCard.isSelected);

        shipMovementCard.canClick = false;
        textContainer_7.SetActive(false);
        hand_7.SetActive(false);
        #endregion

        #region Screen 8 : Click on screen to move
        textContainer_8.SetActive(true);
        screenText_8.text = "Then <b>press</b> on the map where you want your asset to go.";
        hand_8.SetActive(true);
        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint != playerController.currentSelectedEntity.nullVector);

        textContainer_8.SetActive(false);
        hand_8.SetActive(false);
        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint == playerController.currentSelectedEntity.nullVector);
        #endregion

        #region Screen 9 : Plane Apparition
        textContainer_9.SetActive(true);
        screenText_9.text = "Your next asset is an <b>MPA Aircraft</b>. It is much <b>faster</b> than the FREMM. You can select it from the Assets menu just like the FREMM.";

        tutorialPlane.gameObject.SetActive(true);
        uiHandler.entitiesSelectionUI.buttons[0].gameObject.SetActive(true);

        inputManager.canUseCam = true;
        inputManager.canMoveCam = false;
        hand_9.SetActive(true);

        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialPlane);
        tutorialPlane.entityDeck.SetActive(true);
        uiHandler.entityDeckUI.UpdateCurrentDeck(tutorialPlane.entityDeck);

        textContainer_9.SetActive(false);
        hand_9.SetActive(false);
        #endregion

        #region Screen 10 : Move Plane and Ship to target
        textContainer_10.SetActive(true);
        screenText_10.text = "<b>Move</b> both assets to the area <b>indicated</b>.";
        planeTargetScreen_10.SetActive(true);
        shipTargetScreen_10.SetActive(true);

        Vector2 planeTarget = Coordinates.ConvertWorldToVector2(planeTargetScreen_10.transform.position);
        Vector2 shipTarget = Coordinates.ConvertWorldToVector2(shipTargetScreen_10.transform.position);

        planeEquipementCard.gameObject.SetActive(false);

        shipMovementCard.canClick = true;
        shipMovementCard.canDrag = true;

        inputManager.canUseCam = true;
        inputManager.canMoveCam = true;

        yield return new WaitUntil(() => (tutorialPlane.coords.position - planeTarget).magnitude < 2 && (tutorialShip.coords.position - shipTarget).magnitude < 1);

        inputManager.canUseCam = false;
        inputManager.canMoveCam = false;

        planeTargetScreen_10.SetActive(false);
        shipTargetScreen_10.SetActive(false);
        textContainer_10.SetActive(false);
        #endregion

        #region Transition Screen
        textContainer_11.SetActive(true);
        screenText_11.text = "Your objective is to <b>detect</b> submerged objects, then <b>identify</b> them to find the submarine. Finally, you'll have to oblige the submarine to <b>abort</b>her mission.";
        opaquePanel.SetActive(true);
        hand_11.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        opaquePanel.SetActive(false);
        textContainer_11.SetActive(false);
        #endregion

        #region Transition Screen 2
        textContainer_12.SetActive(true);
        screenText_12.text = "You can <b>detect submerged objects</b> by <b>several ways</b>.";

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_12.SetActive(false);
        hand_11.SetActive(false);
        #endregion

        #region Screen 13 : Captas Card Appartition
        textContainer_13.SetActive(true);
        screenText_13.text = "The FREMM's <b>CAPTAS 4</b> detects the position of all submerged objects in the area.";

        shipEquipementCard.gameObject.SetActive(true);
        shipMovementCard.canClick = false;
        shipMovementCard.canDrag = false;

        tutorialShip.linkedButton.SelectEntity();
        tutorialShip.activeEquipement.Init(tutorialShip);
        hand_13.SetActive(true);

        yield return new WaitUntil(() => tutorialShip.activeEquipement.chargeCount == 0);

        
        textContainer_13.SetActive(false);
        shipEquipementCard.canClick = false;
        shipEquipementCard.canDrag = false;
        hand_13.SetActive(false);

        yield return new WaitForSeconds(1);
        #endregion

        #region Screen 13 bis : Information Peremption
        textContainer_13bis.SetActive(true);
        screenText_13bis.text = "The submerged objects previously detected <b>can move</b>, so the points will only stay relevant for a <b>period of time</b>.";

        float tempDuration = pointToZoom.linkedGlobalDetectionPoint.expirationDuration;
        pointToZoom.linkedGlobalDetectionPoint.expirationDuration = 5;

        yield return new WaitUntil(() => pointToZoom.currentDetectableState == DetectableState.detected);

        cameraController.SetTarget(pointToZoom.linkedGlobalDetectionPoint.transform);
        cameraController.SetZoom(0.1f, 1f);

        yield return new WaitForSeconds(5);

        textContainer_13bis.SetActive(false);
        pointToZoom.linkedGlobalDetectionPoint.expirationDuration = tempDuration;
        cameraController.SetZoom(1f, 1f);

        yield return new WaitForSeconds(1);

        #endregion

        #region Screen 14 : Sonobuy Card Appartion
        textContainer_14a.SetActive(true);
        textContainer_14b.SetActive(true);
        screenText_14a.text = "The <b>SONOFLASH buoy</b> detects all the submerged objects inside its range.";
        screenText_14b.text = "Drag the <b>SONOFLASH card</b> on the map to <b>drop one</b>.";
        hand_14.SetActive(true);


        planeEquipementCard.gameObject.SetActive(true);
        tutorialPlane.linkedButton.SelectEntity();
        tutorialPlane.activeEquipement.Init(tutorialPlane);

        planeMovementCard.canClick = false;
        planeMovementCard.canDrag = false;

        planeEquipementCard.canClick = true;
        planeEquipementCard.canDrag = true;


        yield return new WaitUntil(() => tutorialPlane.activeEquipement.chargeCount == tutorialPlane.activeEquipement.chargeMax - 1);

        planeEquipementCard.canClick = false;
        planeEquipementCard.canDrag = false;

        textContainer_14a.SetActive(false);
        textContainer_14b.SetActive(false);
        hand_14.SetActive(false);
        #endregion

        #region Screen 15 : Hull Sonar / BlueMaster
        textContainer_15.SetActive(true);
        screenText_15.text = "The <b>BLUEMASTER</b> detects all submerged objects surrounding the FREMM.";
        tutorialShip.linkedButton.SelectEntity();

        Vector3 tempPostition = pointToDetect.transform.position;
        pointToDetect.transform.position = tutorialShip.transform.position + new Vector3(0, 0, 3);
        pointToDetect.coords.position = Coordinates.ConvertWorldToVector2(pointToDetect.transform.position);

        hand_15.SetActive(true);

        yield return new WaitForSeconds(3f);


        hand_15.SetActive(false);
        tutorialShip.passiveEquipement.Init(tutorialShip);
        
        yield return new WaitUntil(() => pointToDetect.linkedGlobalDetectionPoint.detectionState == DetectionState.unknownDetection);

        cameraController.SetTarget(pointToDetect.transform);
        cameraController.SetZoom(0,1f);
        hand_15.SetActive(false);

        yield return new WaitForSeconds(5f);

        cameraController.SetZoom(1, 1);
        
        yield return new WaitForSeconds(1);


        textContainer_15.SetActive(false);
        #endregion

        #region Screen 16 : SearchMaster 
        textContainer_16.SetActive(true);
        screenText_16.text = "The <b>SEARCHMASTER</b> identifies the submerged objects detected by other equipments. Only <b>recently detected</b> points (white or orange) can be identified.\n<b>Move the MPA</b> to <b>identify</b> the point detected by the BLUEMASTER.";
        tutorialPlane.linkedButton.SelectEntity();

        yield return new WaitForSeconds(3f);

        tutorialPlane.passiveEquipement.Init(tutorialPlane);
        planeMovementCard.canClick = true;
        planeMovementCard.canDrag = true;
        yield return new WaitUntil(() => pointToReveal.linkedGlobalDetectionPoint.detectionState == DetectionState.revealedDetection);

        cameraController.SetTarget(pointToReveal.linkedGlobalDetectionPoint.transform);
        cameraController.SetZoom(0, 1f);

        yield return new WaitForSeconds(5f);

        cameraController.SetZoom(1, 1);

        yield return new WaitForSeconds(1);

        planeMovementCard.canClick = false;
        planeMovementCard.canDrag = false;
        pointToDetect.transform.position = tempPostition;
        pointToDetect.coords.position = Coordinates.ConvertWorldToVector2(pointToDetect.transform.position);
        textContainer_16.SetActive(false);
        #endregion

        #region Screen 17 : Wait
        textContainer_17.SetActive(true);
        screenText_17.text = "An identified object remains so throughout the mission. Detecting an object previously identified will automatically display it's identity.";

        yield return new WaitForSeconds(5);

        textContainer_17.SetActive(false);
        #endregion

        #region Screen 18 : Hold For Information
        textContainer_18.SetActive(true);
        screenText_18.text = "<b>Additional information</b> can be displayed by <b>pressing and holding</b> any element from the interface.";
        hand_18.SetActive(true);

        yield return new WaitUntil(() => descriptionCanvasGroup.alpha == 1);

        textContainer_18.SetActive(false);
        hand_18.SetActive(false);

        yield return new WaitUntil(() => descriptionCanvasGroup.alpha == 0);
        #endregion

        #region Screen 19 : Submarine Apparition
        textContainer_19.SetActive(true);
        screenText_19.text = "A submarine just entered the training area!";

        tutorialSubmarine.gameObject.SetActive(true);
        tutorialSubmarine.currentSpeed = 0;
        tutorialSubmarine.maxSpeed = 0;

        cameraController.SetTarget(tutorialSubmarine.transform);
        cameraController.SetZoom(0,1);
        yield return new WaitForSeconds(1);

        while (tutorialSubmarine.submarineRenderer.transform.position.y < 0.5f)
        {
            tutorialSubmarine.submarineRenderer.transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2);

        while(tutorialSubmarine.submarineRenderer.transform.position.y > -0.5f)
        {
            tutorialSubmarine.submarineRenderer.transform.position -= new Vector3(0, 1, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);
        textContainer_19.SetActive(false);
        #endregion

        #region Screen 20 : Submarine Objective
        textContainer_20.SetActive(true);
        screenText_20.text = "The submarine's goal is to <b>spy on various points of interest</b>. To do this, he must stay close to the point he wishes to spy <b>for a few second</b>.";

        for (int i = 0; i < interestPoints.Count; i++)
        {
            interestPoints[i].gameObject.SetActive(true);
        }

        cameraController.SetTarget(interestPointToFocus.transform);
        cameraController.SetZoom(0.3f, 1);


        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_20.SetActive(false);
        #endregion

        #region Screen 21 : Submarine Objective
        textContainer_21.SetActive(true);
        screenText_21.text = "There are several <b>points of interest</b> in the area. The submarine must <b>spy on a number of them</b> in order to win.";

        cameraController.SetZoom(1f, 1);
        yield return new WaitForSeconds(1);
        hand_21.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_21.SetActive(false);

        textContainer_21.SetActive(false);
        #endregion

        #region Screen 22 : Submarine UI Apparition
        textContainer_22.SetActive(true);
        screenText_22.text = "The interface at the top of the screen is <b>everything you know about the submarine</b>.";

        uiHandler.submarineUI.gameObject.SetActive(true);

        hand_22.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_22.SetActive(false);

        textContainer_22.SetActive(false);
        #endregion

        #region Screen 23 : Submarine UI Gauge
        textContainer_23.SetActive(true);
        screenText_23.text = "The dots represent the <b>submarine's progression</b> in its mission. If <b>every points</b> turns red, you'll <b>fail the mission</b>.";

        hand_23.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_23.SetActive(false);

        textContainer_23.SetActive(false);
        #endregion

        #region Screen 24 : Submarine UI Alert
        textContainer_24.SetActive(true);
        screenText_24.text = "The oscilloscope represents the <b>state of the submarine</b>. It turns red when the submarine detects one of your asset and <b>he will try to escape</b>. \nIf the oscilloscope stays red for <b>too long</b>, the submarine will perform a <b>special manœuvre</b> to escape.";

        hand_24.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_24.SetActive(false);

        textContainer_24.SetActive(false);
        #endregion

        #region Screen 25 : Submarine Detection
        textContainer_25.SetActive(true);
        screenText_25.text = "It's time for you to put your knowledge into practice, find the submarine! ";

        tutorialSubmarine.maxSpeed = 0;
        tutorialSubmarine.currentSpeed = 0;

        tutorialSubmarine.transform.position = newSubPos.transform.position;
        tutorialSubmarine.coords.position = Coordinates.ConvertWorldToVector2(tutorialSubmarine.transform.position);

        planeEquipementCard.canClick = true;
        planeEquipementCard.canDrag = true;
        planeMovementCard.canClick = true;
        planeMovementCard.canDrag = true;

        shipEquipementCard.canClick = true;
        shipEquipementCard.canDrag = true;
        shipMovementCard.canClick = true;
        shipMovementCard.canDrag = true;

        inputManager.canMoveCam = true;
        inputManager.canZoomCam = true;
        inputManager.canUseCam = true;

        yield return new WaitForSeconds(5f);

        textContainer_25.SetActive(false);


        yield return new WaitUntil(()=> tutorialSubmarine.linkedGlobalDetectionPoint.detectionState == DetectionState.revealedDetection);

        inputManager.canMoveCam = false;
        inputManager.canZoomCam = false;
        inputManager.canUseCam = false;

        #endregion

        #region Screen 26 : Helo Selection
        textContainer_26.SetActive(true);
        screenText_26.text = "You have <b>detected and identified</b> the submarine. To make it run away, you must use the <b>HELO helicopter</b>. \nSelect the HELO.";

        uiHandler.entitiesSelectionUI.helicopterSelectionParent.SetActive(true);
        hand_26.SetActive(true);

        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialHelicopter);

        hand_26.SetActive(false);
        textContainer_26.SetActive(false);
        #endregion

        #region Screen 27 : Helo Launch
        textContainer_27.SetActive(true);
        screenText_27.text = "The HELO must be <b>prepared</b> before it can take off.";

        hand_27.SetActive(true);

        yield return new WaitUntil(() => tutorialHelicopter.operating);

        hand_27.SetActive(false);

        yield return new WaitUntil(() => tutorialHelicopter.inAlert);

        hand_27.SetActive(true);
        screenText_27.text = "He only remains active for a <b>limited time</b> once it has been activated.";

        yield return new WaitUntil(() => tutorialHelicopter.inFlight);
        hand_27.SetActive(false);

        textContainer_27.SetActive(false);
        #endregion

        #region Screen 29 : Helo Control
        textContainer_29.SetActive(true);
        screenText_29.text = "The HELO is <b>controlled differently</b> from the other assets as <b>it is no longer possible to zoom in or out</b>. \nYou can move the HELO by pressing on the screen.";

        yield return new WaitUntil(()=> tutorialHelicopter.currentTargetPoint != tutorialHelicopter.nullVector);


        yield return new WaitForSeconds(5f);
        textContainer_29.SetActive(false);
        #endregion

        #region Screen 30 : Flash Display
        textContainer_30.SetActive(true);
        screenText_30.text = "The FLASH dipping sonar is the key sensor on the HELO. It allows you to <b>detect nearby submerged objects</b> but above all to <b>win the mission</b> if you manage to dip it close enough to the submarine.";
        hand_30.SetActive(true);

        yield return new WaitForSeconds(10f);

        hand_30.SetActive(false);
        textContainer_30.SetActive(false);
        #endregion

        #region Screen 31 : Flash Usage
        textContainer_31.SetActive(true);
        screenText_31.text = "Position the HELO above the submarine and use the FLASH to end the mission.";

        yield return new WaitUntil(() => victoryCanvasGroup.alpha == 1);

        textContainer_31.SetActive(false);
        #endregion

        #region Screen 32 : Victory Screen
        textContainer_32.SetActive(true);
        screenText_32.text = "Congratulations, you are now ready to prove yourself on the field.";
        #endregion
    }
}
 