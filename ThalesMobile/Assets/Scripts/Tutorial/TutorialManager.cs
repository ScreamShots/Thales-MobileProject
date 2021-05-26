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
    [Space]
    public GameObject textContainer_13bis;
    public TextMeshProUGUI screenText_13bis;
    public DetectableOceanEntity pointToZoom;

    [Space]
    public GameObject textContainer_14a;
    public TextMeshProUGUI screenText_14a;
    public GameObject textContainer_14b;
    public TextMeshProUGUI screenText_14b;

    [Space]
    public GameObject textContainer_15;
    public TextMeshProUGUI screenText_15;
    public DetectableOceanEntity pointToDetect;

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

    [Space]
    public GameObject textContainer_22;
    public TextMeshProUGUI screenText_22;

    [Space]
    public GameObject textContainer_23;
    public TextMeshProUGUI screenText_23;

    [Space]
    public GameObject textContainer_24;
    public TextMeshProUGUI screenText_24;

    [Space]
    public GameObject textContainer_25;
    public TextMeshProUGUI screenText_25;

    [Space]
    public GameObject textContainer_26;
    public TextMeshProUGUI screenText_26;

    [Space]
    public GameObject textContainer_27;
    public TextMeshProUGUI screenText_27;

    [Space]
    public GameObject textContainer_29;
    public TextMeshProUGUI screenText_29;

    [Space]
    public GameObject textContainer_30;
    public TextMeshProUGUI screenText_30;

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
        screenText_2.text = "Déplacez la camera jusqu'à la zone d'entrainement";

        yield return new WaitUntil(() => (cameraController.focusPoint.position - cameraTargetScreen_2.transform.position).magnitude < 2f);

        textContainer_2.SetActive(false);
        hand_2.SetActive(false);

        #endregion

        #region Screen 3 : Camera Zoom
        inputManager.canUseCam = false;
        inputManager.canZoomCam = true;

        textContainer_3.SetActive(true);
        screenText_3.text = "Zoomez et dézoomez sur la zone d'entrainement";
        hand_3.SetActive(true);

        yield return new WaitUntil(() => cameraController.zoomIntensity < 0.2);
        yield return new WaitUntil(() => cameraController.zoomIntensity > 0.8);

        inputManager.canZoomCam = false;
        cameraController.SetZoom(1, 5);

        textContainer_3.SetActive(false);
        hand_3.SetActive(false);
        #endregion

        #region Screen 4 : Ship Apparition
        textContainer_4a.SetActive(true);
        textContainer_4b.SetActive(true);

        screenText_4a.text = "Voici la frégate, un navire équipée pour la détéction des sous-marins.";
        screenText_4b.text = "Appuyez sur la frégate pour la sélectionner.";
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
        screenText_5.text = "Vous pouvez également <b> séléctionner </b> la frégate dans la <b> menu des batiments</b>.";

        uiHandler.entitiesSelectionUI.gameObject.SetActive(true);
        uiHandler.entitiesSelectionUI.currentButton = null;
        playerController.currentSelectedEntity = null;
        hand_5.SetActive(true);
        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialShip);

        textContainer_5.SetActive(false);
        hand_5.SetActive(false);
        #endregion

        #region Screen 5 bis : Double click on UI
        textContainer_5bis.SetActive(true);
        screenText_5bis.text = "<b>Centrez la caméra</b> sur le bâtiment en le sélectionnant une seconde fois dans le menu des bâtiments";   
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
        screenText_6a.text = "Tout les batiments ont des <b>équipements</b>";
        screenText_6b.text = "<b>Appuyez</b> et faites <b>glisser</b> la <b>carte déplacement</b> à l'endroit désigné";

        uiHandler.entityDeckUI.gameObject.SetActive(true);
        uiHandler.entityDeckUI.currentDeck = null;
        uiHandler.entityDeckUI.UpdateCurrentDeck(uiHandler.entitiesSelectionUI.currentButton.linkedEntity.entityDeck);
        hand_6.SetActive(true);

        yield return new WaitForEndOfFrame();


        shipMovementCard.canClick = false;
        shipPassiveCard.canHold = false;

        targetScreen_6.SetActive(true);
        
        Vector2 currentTarget = Coordinates.ConvertWorldToVector2(targetScreen_6.transform.position);
        yield return new WaitUntil(() => (playerController.currentSelectedEntity.currentTargetPoint  - currentTarget).magnitude < 1);

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
        screenText_7.text = "Vous pouvez aussi <b>appuyer</b> sur la carte de déplacement pour que celle-ci soit <b>séléctionnée</b>";
        hand_7.SetActive(true);
        shipMovementCard.canClick = true;

        yield return new WaitUntil(() => shipMovementCard.isSelected);

        shipMovementCard.canClick = false;
        textContainer_7.SetActive(false);
        hand_7.SetActive(false);
        #endregion

        #region Screen 8 : Click on screen to move
        textContainer_8.SetActive(true);
        screenText_8.text = "Puis <b>appuyez</b> sur l'endroit de la zone où vous voulez <b>déplacer</b> votre bâtiment";
        hand_8.SetActive(true);
        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint != playerController.currentSelectedEntity.nullVector);

        textContainer_8.SetActive(false);
        hand_8.SetActive(false);
        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint == playerController.currentSelectedEntity.nullVector);
        #endregion

        #region Screen 9 : Plane Apparition
        textContainer_9.SetActive(true);
        screenText_9.text = " Votre second bâtiment est un <b>avion de patrouille maritime</b> dit <b>PATMAR</b>. Il est bien plus <b>rapide</b> que la frégate. Vous pouvez le sélectionner dans le menu des bâtiment comme la frégate.";

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
        screenText_10.text = "<b>Déplacez</b> les deux batiments dans la <b>zone indiquée</b> pour chaque bâtiment";
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
        screenText_11.text = "Votre objectif est de <b>détécter</b> les objets immergés, puis de les <b>identifier</b> pour trouver le sous-marin. Pour enfin le <b>faire fuir</b> grâce aux équipements <b>Thalès.</b>";
        opaquePanel.SetActive(true);
        hand_11.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        opaquePanel.SetActive(false);
        textContainer_11.SetActive(false);
        #endregion

        #region Transition Screen 2
        textContainer_12.SetActive(true);
        screenText_12.text = "Il est possible de <b>détécter</b> des <b>objets immergés</b> de <b>plusieurs façons</b>.";

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_12.SetActive(false);
        hand_11.SetActive(false);
        #endregion

        #region Screen 13 : Captas Card Appartition
        textContainer_13.SetActive(true);
        screenText_13.text = "Le CAPTAS détecte la position de tout les objets immergés sur la zone.";

        shipEquipementCard.gameObject.SetActive(true);
        shipMovementCard.canClick = false;
        shipMovementCard.canDrag = false;

        tutorialShip.linkedButton.SelectEntity();

        yield return new WaitUntil(() => tutorialShip.activeEquipement.chargeCount == 0);

        textContainer_13.SetActive(false);
        shipEquipementCard.canClick = false;
        shipEquipementCard.canDrag = false;

        yield return new WaitForSeconds(1);
        #endregion

        #region Screen 13 bis : Information Peremption
        textContainer_13bis.SetActive(true);
        screenText_13bis.text = "Les objets immergés détectés peuvent se déplacer, l'information de leur position n'est donc valable que pendant un temps limité";

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
        screenText_14a.text = "La bouée Sonobuy détècte tout les objets immergés se trouvant dans sa portée.";
        screenText_14b.text = "Appuyez et faites glisser la carte bouée jusqu'a l'endroit où vous souhaitez la larguer.";

        planeEquipementCard.gameObject.SetActive(true);
        tutorialPlane.linkedButton.SelectEntity();
        planeMovementCard.canClick = false;
        planeMovementCard.canDrag = false;

        planeEquipementCard.canClick = true;
        planeEquipementCard.canDrag = true;

        yield return new WaitUntil(() => tutorialPlane.activeEquipement.chargeCount == tutorialPlane.activeEquipement.chargeMax - 1);

        planeEquipementCard.canClick = false;
        planeEquipementCard.canDrag = false;

        textContainer_14a.SetActive(false);
        textContainer_14b.SetActive(false);
        #endregion

        #region Screen 15 : Hull Sonar / BlueMaster
        textContainer_15.SetActive(true);
        screenText_15.text = "Le Bluemaster détecte tous les objets immergés dans son rayon d'action.";
        tutorialShip.linkedButton.SelectEntity();

        yield return new WaitForSeconds(5f);

        Vector3 tempPostition = pointToDetect.transform.position;
        pointToDetect.transform.position = tutorialShip.transform.position + new Vector3(0, 0, 1);

        yield return new WaitUntil(() => pointToDetect.linkedGlobalDetectionPoint.detectionState == DetectionState.unknownDetection);

        pointToDetect.transform.position = tempPostition;
        textContainer_15.SetActive(false);
        #endregion

        #region Screen 16 : SearchMaster 
        textContainer_16.SetActive(true);
        screenText_16.text = "Le Searchmaster identifie la nature d'un objet immergé et détecté dans son rayon d'action.";
        tutorialShip.linkedButton.SelectEntity();

        yield return new WaitForSeconds(5f);

        Vector3 tempPostitionReveal = pointToDetect.transform.position;
        pointToReveal.transform.position = tutorialPlane.transform.position + new Vector3(0, 0, 1);

        yield return new WaitUntil(() => pointToReveal.linkedGlobalDetectionPoint.detectionState == DetectionState.revealedDetection);

        pointToReveal.transform.position = tempPostitionReveal;
        textContainer_16.SetActive(false);
        #endregion

        #region Screen 17 : Wait
        textContainer_17.SetActive(true);
        screenText_17.text = "Un objet immergé qui a été identifié le reste pendant toute la partie.";

        yield return new WaitForSeconds(5);

        textContainer_17.SetActive(false);
        #endregion

        #region Screen 18 : Hold For Information
        textContainer_18.SetActive(true);
        screenText_18.text = "Des informations complémentaires sont disponibles en restant appuyer sur un élément de l'écran";

        yield return new WaitUntil(() => descriptionCanvasGroup.alpha == 1);

        textContainer_18.SetActive(false);

        yield return new WaitUntil(() => descriptionCanvasGroup.alpha == 0);
        #endregion

        #region Screen 19 : Submarine Apparition
        textContainer_19.SetActive(true);
        screenText_19.text = "Des informations complémentaires sont disponibles en restant appuyer sur un élément de l'écran";
        tutorialSubmarine.gameObject.SetActive(true);
        tutorialSubmarine.transform.position += new Vector3(0, 1, 0) * 0.5f;
        cameraController.SetTarget(tutorialSubmarine.transform);
        cameraController.SetZoom(0,1);

        yield return new WaitForSeconds(1);

        while(tutorialSubmarine.transform.position.y > -0.5f)
        {
            tutorialSubmarine.transform.position -= new Vector3(0, 1, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        textContainer_19.SetActive(false);
        #endregion

        #region Screen 20 : Submarine Objective
        textContainer_20.SetActive(true);
        screenText_20.text = "L'objectif du sous-marin est de pirater des points d'intérêts. Pour cela, il doit rester proche du point qu'il souhaite pirater pendant quelques secondes.";

        cameraController.SetTarget(interestPointToFocus.transform);
        cameraController.SetZoom(0.3f, 1);


        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_20.SetActive(false);
        #endregion

        #region Screen 21 : Submarine Objective
        textContainer_21.SetActive(true);
        screenText_21.text = "Il y a plusieurs points d'intérêts sur la zone, le sous-marin doit en pirater un certain nombre pour gagner";

        cameraController.SetZoom(1f, 1);


        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_21.SetActive(false);
        #endregion

        #region Screen 22 : Submarine UI Apparition
        textContainer_22.SetActive(true);
        screenText_22.text = "L'interface en haut de l'écran concerne les informations que vous détenez sur le sous-marin";

        uiHandler.submarineUI.gameObject.SetActive(true);


        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_22.SetActive(false);
        #endregion

        #region Screen 23 : Submarine UI Gauge
        textContainer_23.SetActive(true);
        screenText_23.text = "La jauge représente l'avancement du sous-marin dans le piratage des points d'intérêt, ainsi que le nombre de point d'intérêt qu'il doit pirater. Si cette jauge est pleine, le sous-marin gagne la partie.";

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_23.SetActive(false);
        #endregion

        #region Screen 24 : Submarine UI Alert
        textContainer_24.SetActive(true);
        screenText_24.text = "Le cercle représente l'état du sous-marin, quand le cercle devient rouge, c'est que le sous-marin vous a détecté et essaye de fuir. Si le cercle reste rouge pendant trop longtemps, le sous - marin effectuera une manoeuvre spéciale pour vous semer.";

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_24.SetActive(false);
        #endregion

        #region Screen 25 : Submarine Detection
        textContainer_25.SetActive(true);
        screenText_25.text = "Il est temps pour vous de mettre en pratique vos acquis, trouvez le sous-marin ! ";

        tutorialSubmarine.maxSpeed = 0;
        tutorialSubmarine.currentSpeed = 0;

        yield return new WaitForSeconds(5f);

        textContainer_25.SetActive(false);

        yield return new WaitUntil(()=> tutorialSubmarine.linkedGlobalDetectionPoint.detectionState == DetectionState.revealedDetection);
        #endregion

        #region Screen 26 : Helo Selection
        textContainer_26.SetActive(true);
        screenText_26.text = "Vous avez détecté et identifié le sous-marin  Pour le faire fuir, vous devez utiliser l'hélicoptère : HELO. \nSélectionnez l'HELO";

        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialHelicopter);

        textContainer_26.SetActive(false);
        #endregion

        #region Screen 27 : Helo Launch
        textContainer_27.SetActive(true);
        screenText_27.text = "L'HELO a un temps de préparation";

        yield return new WaitUntil(() => tutorialHelicopter.inAlert);

        screenText_27.text = "L'HELO ne reste actif que pendant un temps limité une fois qu'il a été activé";

        yield return new WaitUntil(() => tutorialHelicopter.inFlight);

        textContainer_27.SetActive(false);
        #endregion

        #region Screen 29 : Helo Control
        textContainer_29.SetActive(true);
        screenText_29.text = "L'HELO se contrôle différemment des bâtiments, il n'est plus possible de zoomer ou de vous déplacer sur la zone. \nDéplacez l'HELO en appuyant sur l'écran";

        yield return new WaitUntil(()=> tutorialHelicopter.currentTargetPoint != tutorialHelicopter.nullVector);


        yield return new WaitForSeconds(3f);
        textContainer_29.SetActive(false);
        #endregion

        #region Screen 30 : Flash Display
        textContainer_30.SetActive(true);
        screenText_30.text = "Le sonar Flash est le seul équipement de l'HELO. Il permet de détecter les objets immergés proches et de faire fuir le sous-marin si vous parvenez à toucher avec.";

        yield return new WaitForSeconds(5);

        textContainer_30.SetActive(false);
        #endregion

        #region Screen 31 : Flash Usage
        textContainer_31.SetActive(true);
        screenText_31.text = "Utilisez le flash au dessus du sous-marin pour le faire fuir.";

        yield return new WaitUntil(() => victoryCanvasGroup.alpha == 1);

        textContainer_31.SetActive(true);
        #endregion

        #region Screen 32 : Victory Screen
        textContainer_32.SetActive(true);
        screenText_32.text = "Félicitations vous avez réussi la mission CASEX : session d'entraînement à la lutte anti sous-marine. Vous êtes prêt à partir sur le terrain.";
        #endregion
    }
}
 