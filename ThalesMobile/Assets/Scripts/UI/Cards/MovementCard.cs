using UnityEngine;

public class MovementCard : MonoBehaviour
{
    public InteractableUI card;
    private InputManager inputManager;
    private UIHandler uiHandler;

    

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        uiHandler = GameManager.Instance.uiHandler;

        card.clickHandler     += OnClickEvent;
        card.beginDragHandler += OnBeginDragEvent;
        card.endDragHandler   += OnEndDragEvent;
        card.abortHandler     += AbortMethod;
        card.holdHandler      += UpdateDescriptionText;
    }

   

    public void AbortMethod()
    {
        card.Deselect();
        inputManager.getEntityTarget = false;
        inputManager.currentSelectedCard = null;
    }

    public void OnClickEvent()
    {
        if(card.isSelected)
        {
            card.abortHandler();
        }
        else
        {
            //Deselect and abort current selected card.
            if(inputManager.currentSelectedCard != null)
            {
                inputManager.currentSelectedCard.abortHandler();
            }

            //Select new card and link to input manager.
            card.Select();
            inputManager.getEntityTarget = true;
            inputManager.currentSelectedCard = card;
        }
    }

    public void OnBeginDragEvent()
    {
        //Deselect and abort current selected card.
        if (inputManager.currentSelectedCard != null)
        {
            inputManager.currentSelectedCard.abortHandler();
        }

        inputManager.isDraggingCard = true;
        inputManager.getEntityTarget = true;
        inputManager.currentSelectedCard = card;
    }

    public void OnEndDragEvent()
    {
        inputManager.isDraggingCard = false;
        inputManager.currentSelectedCard = null;
    }

    private void UpdateDescriptionText()
    {
        uiHandler.entityDeckUI.descriptionHeaderText.text = "Movement Card";//Expose string
        uiHandler.entityDeckUI.descriptionText.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";//Expose stringS
    }

}
