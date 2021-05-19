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
    public GameObject textContainer_14a;
    public TextMeshProUGUI screenText_14a;
    public GameObject textContainer_14b;
    public TextMeshProUGUI screenText_14b;
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

        uiHandler.entitiesSelectionUI.gameObject.SetActive(false);
        uiHandler.entityDeckUI.gameObject.SetActive(false);
        uiHandler.submarineUI.gameObject.SetActive(false);
        uiHandler.entitiesSelectionUI.helicopterSelectionParent.SetActive(false);
        uiHandler.entitiesSelectionUI.buttons[0].gameObject.SetActive(false);

        tutorialPlane.gameObject.SetActive(false);
        tutorialShip.gameObject.SetActive(false);

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

        #region Screen 5 : Ship UI Apparition
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

        #region Screen 11 : Transition Screen
        textContainer_11.SetActive(true);
        screenText_11.text = "Votre objectif est de <b>détécter</b> les objets immergés, puis de les <b>identifier</b> pour trouver le sous-marin. Pour enfin le <b>faire fuir</b> grâce aux équipements <b>Thalès.</b>";
        opaquePanel.SetActive(true);
        hand_11.SetActive(true);

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        opaquePanel.SetActive(false);
        textContainer_11.SetActive(false);
        #endregion

        #region Screen 12 : Transition Screen 2
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

        #region Screen 15 : Hull Sonar 

        #endregion


    }




}
 