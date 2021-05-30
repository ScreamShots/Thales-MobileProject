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
        screenText_1.text = "Bienvenue dans ce CASEX.";
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
        screenText_2.text = "Déplacez la caméra en faisant <b>glisser</b> votre doigt sur l'écran.";

        cameraTargetScreen_2.transform.position = cameraController.focusPoint.position;

        yield return new WaitUntil(() => (cameraController.focusPoint.position - cameraTargetScreen_2.transform.position).magnitude > 5f);

        textContainer_2.SetActive(false);
        hand_2.SetActive(false);

        #endregion

        #region Screen 3 : Camera Zoom
        inputManager.canUseCam = false;
        inputManager.canZoomCam = true;

        textContainer_3.SetActive(true);
        screenText_3.text = "Zoomez et dézoomez sur la zone d'entrainement.";
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

        screenText_4a.text = "Voici la <b>frégate</b>, un navire équipé pour la <b>détéction des sous-marins</b>.";
        screenText_4b.text = "<b>Appuyez</b> sur la frégate pour la sélectionner.";
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
        screenText_5.text = "Vous pouvez également <b>sélectionner</b> la frégate dans le <b> menu des bâtiments</b>.";

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
        screenText_5bis.text = "Si le bâtiment sélectionné est hors-champ, vous pouvez <b>centrer la caméra</b> dessus en le sélectionnant à nouveau.";   
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
        screenText_6a.text = "Tous les bâtiments ont des <b>équipements</b> pour effectuer des actions.";
        screenText_6b.text = "<b>Appuyez</b> et faites <b>glisser</b> la <b>carte déplacement</b> jusqu'à la cible pour déplacer la frégate.";

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
        screenText_7.text = "Vous pouvez aussi <b>appuyer</b> sur la carte de déplacement pour que celle-ci soit <b>sélectionnée</b>.";
        hand_7.SetActive(true);
        shipMovementCard.canClick = true;

        yield return new WaitUntil(() => shipMovementCard.isSelected);

        shipMovementCard.canClick = false;
        textContainer_7.SetActive(false);
        hand_7.SetActive(false);
        #endregion

        #region Screen 8 : Click on screen to move
        textContainer_8.SetActive(true);
        screenText_8.text = "Puis <b>appuyez</b> sur l’endroit de la zone où vous voulez déplacer votre bâtiment.";
        hand_8.SetActive(true);
        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint != playerController.currentSelectedEntity.nullVector);

        textContainer_8.SetActive(false);
        hand_8.SetActive(false);
        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint == playerController.currentSelectedEntity.nullVector);
        #endregion

        #region Screen 9 : Plane Apparition
        textContainer_9.SetActive(true);
        screenText_9.text = "Votre second bâtiment est un <b>avion de patrouille maritime MPA ATL2</b>. Il est bien plus <b>rapide</b> que la frégate. Vous pouvez le sélectionner dans le menu des bâtiments comme la frégate.";

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
        screenText_10.text = "<b>Déplacez</b> les deux bâtiments dans la <b>zone indiquée</b> pour chaque bâtiment.";
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
        screenText_11.text = "Votre objectif est de <b>détecter</b> les objets immergés, puis de les <b>identifier</b> pour trouver le sous-marin. Pour enfin le <b>faire fuir</b> grâce aux équipements <b>Thales</b>.";
        opaquePanel.SetActive(true);
        hand_11.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        opaquePanel.SetActive(false);
        textContainer_11.SetActive(false);
        #endregion

        #region Transition Screen 2
        textContainer_12.SetActive(true);
        screenText_12.text = "Il est possible de <b>détecter</b> des <b>objets immergés</b> de <b>plusieurs façons</b>.";

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_12.SetActive(false);
        hand_11.SetActive(false);
        #endregion

        #region Screen 13 : Captas Card Appartition
        textContainer_13.SetActive(true);
        screenText_13.text = "Le <b>CAPTAS 4</b> de la frégate détecte la position des tous les objets immergés sur la zone.";

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
        screenText_13bis.text = "Les objets immergés détectés <b>peuvent se déplacer</b>, l'information de leur position n'est donc valable que pendant un <b>temps limité</b>.";

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
        screenText_14a.text = "La <b>bouée SONOFLASH</b> détecte tous les objets immergés se trouvant dans sa portée</b>.";
        screenText_14b.text = "Faites glisser <b>la carte bouée</b> jusqu'a l'endroit où vous souhaitez <b>la larguer</b>.";
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
        screenText_15.text = "Le <b>BLUEMASTER</b> détecte tous les objets immergés dans son rayon d’action.";
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
        screenText_16.text = "Le <b>SEARCHMASTER</b> identifie la nature d’un objet immergé et détecté dans son rayon d’action. Seuls les points <b>détéctés récemment</b> (vert ou orange) peuvent être identifiés.\n<b>Déplacez l’avion</b> pour <b>identifier</b> le point détecté par la frégate.";
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
        screenText_17.text = "Un objet immergé qui a été identifié le reste pendant toute la partie. Il suffit de le détecter à nouveau pour connaître sa nature.";

        yield return new WaitForSeconds(5);

        textContainer_17.SetActive(false);
        #endregion

        #region Screen 18 : Hold For Information
        textContainer_18.SetActive(true);
        screenText_18.text = "Des <b>informations complémentaires</b> sont disponibles en <b>restant appuyé</b> sur un élément de l'écran.";
        hand_18.SetActive(true);

        yield return new WaitUntil(() => descriptionCanvasGroup.alpha == 1);

        textContainer_18.SetActive(false);
        hand_18.SetActive(false);

        yield return new WaitUntil(() => descriptionCanvasGroup.alpha == 0);
        #endregion

        #region Screen 19 : Submarine Apparition
        textContainer_19.SetActive(true);
        screenText_19.text = "Un sous-marin vient d'entrer dans la zone d'entraînement !";

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
        screenText_20.text = "L’objectif du sous-marin est <b>d’espionner des points d’intérêt</b>. Pour cela, il doit rester proche du point qu’il souhaite espionner pendant <b>quelques secondes</b>.";

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
        screenText_21.text = "Il y a plusieurs <b>points d’intérêt</b> dans la zone, le sous-marin doit en <b>espionner un certain nombre</b> pour gagner.";

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
        screenText_22.text = "L’interface en haut de l’écran concerne les <b>informations</b> que vous détenez sur le <b>sous-marin</b>.";

        uiHandler.submarineUI.gameObject.SetActive(true);

        hand_22.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_22.SetActive(false);

        textContainer_22.SetActive(false);
        #endregion

        #region Screen 23 : Submarine UI Gauge
        textContainer_23.SetActive(true);
        screenText_23.text = "Les points représentent <b>l'avancement du sous-marin</b> dans l'espionnage des points d'intérêt, ainsi que le nombre de point d'intérêt qu'il doit espionner. Si <b>tous les points</b> sont remplis, le sous-marin <b>gagne la partie</b>.";

        hand_23.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_23.SetActive(false);

        textContainer_23.SetActive(false);
        #endregion

        #region Screen 24 : Submarine UI Alert
        textContainer_24.SetActive(true);
        screenText_24.text = "L'oscilloscope représente <b>l'état du sous-marin</b>, quand il devient rouge, c'est que le sous-marin vous a détecté et <b>essaye de fuir</b>. \nSi l'oscilloscope reste rouge pendant <b>trop longtemps</b>, le sous-marin effectuera une <b>manoeuvre spéciale</b> pour vous semer.";

        hand_24.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        hand_24.SetActive(false);

        textContainer_24.SetActive(false);
        #endregion

        #region Screen 25 : Submarine Detection
        textContainer_25.SetActive(true);
        screenText_25.text = "Il est temps pour vous de mettre en pratique vos acquis, trouvez le sous-marin ! ";

        tutorialSubmarine.maxSpeed = 0;
        tutorialSubmarine.currentSpeed = 0;

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

        inputManager.canMoveCam = false;
        inputManager.canZoomCam = false;
        inputManager.canUseCam = false;

        yield return new WaitUntil(()=> tutorialSubmarine.linkedGlobalDetectionPoint.detectionState == DetectionState.revealedDetection);
        #endregion

        #region Screen 26 : Helo Selection
        textContainer_26.SetActive(true);
        screenText_26.text = "Vous avez <b>détecté et identifié</b> le sous-marin  Pour le faire fuir, vous devez utiliser <b>l'hélicoptère HELO</b>. \nSélectionnez l'HELO";

        uiHandler.entitiesSelectionUI.helicopterSelectionParent.SetActive(true);
        hand_26.SetActive(true);

        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialHelicopter);

        hand_26.SetActive(false);
        textContainer_26.SetActive(false);
        #endregion

        #region Screen 27 : Helo Launch
        textContainer_27.SetActive(true);
        screenText_27.text = "L'HELO doit être <b>préparé</b> avant qu'il puisse décoller";

        hand_27.SetActive(true);

        yield return new WaitUntil(() => tutorialHelicopter.operating);

        hand_27.SetActive(false);

        yield return new WaitUntil(() => tutorialHelicopter.inAlert);

        hand_27.SetActive(true);
        screenText_27.text = "L'HELO ne reste actif que pendant un <b>temps limité</b> une fois qu'il a été activé";

        yield return new WaitUntil(() => tutorialHelicopter.inFlight);
        hand_27.SetActive(false);

        textContainer_27.SetActive(false);
        #endregion

        #region Screen 29 : Helo Control
        textContainer_29.SetActive(true);
        screenText_29.text = "L'HELO se <b>contrôle différemment</b> des bâtiments, il n'est <b>plus possible de zoomer ou de vous déplacer</b> sur la zone. \nDéplacez l'HELO en appuyant sur l'écran.";

        yield return new WaitUntil(()=> tutorialHelicopter.currentTargetPoint != tutorialHelicopter.nullVector);


        yield return new WaitForSeconds(5f);
        textContainer_29.SetActive(false);
        #endregion

        #region Screen 30 : Flash Display
        textContainer_30.SetActive(true);
        screenText_30.text = "Le sonar Flash est le seul équipement de l'HELO. Il permet de <b>détecter les objets immergés proches</b> et de <b>faire fuir le sous-marin</b> si vous parvenez à le larguer suffisament près.";
        hand_30.SetActive(true);

        yield return new WaitForSeconds(10f);

        hand_30.SetActive(false);
        textContainer_30.SetActive(false);
        #endregion

        #region Screen 31 : Flash Usage
        textContainer_31.SetActive(true);
        screenText_31.text = "Déplacez vous au dessus du sous-marin et utilisez le Flash pour le faire fuir.";

        yield return new WaitUntil(() => victoryCanvasGroup.alpha == 1);

        textContainer_31.SetActive(false);
        #endregion

        #region Screen 32 : Victory Screen
        textContainer_32.SetActive(true);
        screenText_32.text = "Félicitations ! Vous êtes maintenant prêt à partir sur le terrain.";
        #endregion
    }
}
 