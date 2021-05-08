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
    [Space]
    public GameObject cameraTargetScreen_2;
    public GameObject textContainer_2;
    public TextMeshProUGUI screenText_2;
    [Space]
    public GameObject textContainer_3;
    public TextMeshProUGUI screenText_3;
    [Space]
    public GameObject textContainer_4a;
    public TextMeshProUGUI screenText_4a;
    public GameObject textContainer_4b;
    public TextMeshProUGUI screenText_4b;
    [Space]
    public GameObject textContainer_5;
    public TextMeshProUGUI screenText_5;
    [Space]
    public GameObject targetScreen_6;
    public GameObject textContainer_6a;
    public TextMeshProUGUI screenText_6a;
    public GameObject textContainer_6b;
    public TextMeshProUGUI screenText_6b;

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



        #region Screen 1 : Tutorial Introduction
        opaquePanel.SetActive(true);
        textContainer_1.SetActive(true);
        screenText_1.text = "Bienvenue dans ce CASEX.";

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        textContainer_1.SetActive(false);
        opaquePanel.SetActive(false);
        #endregion

        #region Screen 2 : Camera Movement

        inputManager.canUseCam = true;
        textContainer_2.SetActive(true);
        screenText_2.text = "Déplacez la camera jusqu'à la zone d'entrainement";

        yield return new WaitUntil(() => (cameraController.focusPoint.position - cameraTargetScreen_2.transform.position).magnitude < 2f);

        textContainer_2.SetActive(false);

        #endregion

        #region Screen 3 : Camera Zoom
        inputManager.canUseCam = false;
        inputManager.canZoomCam = true;

        textContainer_3.SetActive(true);
        screenText_3.text = "Zoomez et dézoomez sur la zone d'entrainement";

        yield return new WaitUntil(() => cameraController.zoomIntensity < 0.2);
        yield return new WaitUntil(() => cameraController.zoomIntensity > 0.8);

        inputManager.canZoomCam = false;
        cameraController.SetZoom(1, 5);

        textContainer_3.SetActive(false);
        #endregion

        #region Screen 4 : Ship Apparition
        textContainer_4a.SetActive(true);
        textContainer_4b.SetActive(true);

        screenText_4a.text = "Voici la frégate, un navire équipée pour la détéction des sous-marins.";
        screenText_4b.text = "Appuyez sur la frégate pour la sélectionner.";

        tutorialShip.gameObject.SetActive(true);

        inputManager.canUseCam = true;
        inputManager.canMoveCam = false;

        yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialShip);

        inputManager.canUseCam = false;
        inputManager.canMoveCam = true;

        textContainer_4a.SetActive(false);
        textContainer_4b.SetActive(false);
        #endregion

        # region Screen 5 : Ship UI Apparition
                textContainer_5.SetActive(true);
                screenText_5.text = "Vous pouvez également <b> séléctionner </b> la frégate dans la <b> menu des batiments</b>.";

                uiHandler.entitiesSelectionUI.gameObject.SetActive(true);
                uiHandler.entitiesSelectionUI.currentButton = null;
                playerController.currentSelectedEntity = null;

                yield return new WaitUntil(() => playerController.currentSelectedEntity == tutorialShip);

                textContainer_5.SetActive(false);

                yield return new WaitForSeconds(1);

        #endregion

        #region Screen 6 : DeckUI Apparition
        textContainer_6a.SetActive(true);
        textContainer_6b.SetActive(true);
        screenText_6a.text = "Tout les batiments ont des <b>équipements</b>";
        screenText_6b.text = "<b>Appuyez</b> et faites <b>glisser</b> la <b>carte déplacement</b> à l'endroit désigné";

        uiHandler.entityDeckUI.gameObject.SetActive(true);
        uiHandler.entityDeckUI.UpdateCurrentDeck(uiHandler.entitiesSelectionUI.currentButton.linkedEntity.entityDeck);

        yield return new WaitForEndOfFrame();

        InteractableUI shipEquipementCard = uiHandler.entityDeckUI.currentDeck.GetComponent<Deck>().equipementCard;
        InteractableUI shipMovementCard = uiHandler.entityDeckUI.currentDeck.GetComponent<Deck>().movementCard;
        InteractableUI shipPassiveCard = uiHandler.entityDeckUI.currentDeck.GetComponent<Deck>().passiveCard;

        shipEquipementCard.gameObject.SetActive(false);
        shipMovementCard.canClick = false;
        shipPassiveCard.canHold = false;

        targetScreen_6.SetActive(true);
        
        Vector2 currentTarget = Coordinates.ConvertWorldToVector2(targetScreen_6.transform.position);
        yield return new WaitUntil(() => (playerController.currentSelectedEntity.currentTargetPoint  - currentTarget).magnitude < 1);

        shipMovementCard.canDrag = false;
        shipMovementCard.canClick = false;

        yield return new WaitUntil(() => playerController.currentSelectedEntity.currentTargetPoint == playerController.currentSelectedEntity.nullVector);
        textContainer_6a.SetActive(false);
        textContainer_6b.SetActive(false);
        #endregion

        #region Screen 7 : Click on card

        #endregion
    }




}
