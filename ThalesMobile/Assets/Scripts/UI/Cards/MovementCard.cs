using System.Collections;
using System.Collections.Generic;
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



}
