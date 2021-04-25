using UnityEngine;

public class PassiveCard : MonoBehaviour
{
    public InteractableUI card;
    private InputManager inputManager;
    private UIHandler uiHandler;

    [Header("Description")]
    public string descriptionHeader;
    public string descriptionText;


    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        uiHandler = GameManager.Instance.uiHandler;

        card.holdHandler += UpdateDescriptionText;
    }

    private void UpdateDescriptionText()
    {
        uiHandler.entityDeckUI.descriptionHeaderText.text = descriptionHeader;//Expose string
        uiHandler.entityDeckUI.descriptionText.text = descriptionText;//Expose stringS
    }

}
