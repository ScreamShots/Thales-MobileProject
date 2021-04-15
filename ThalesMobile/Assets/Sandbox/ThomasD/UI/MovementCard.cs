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
        card.abortHandler += AbortMethod;
    }

    // Update is called once per frame
    void Update()
    {
        if(card.isDragged)
        {
            if (!card.isSelected)
            {
                card.Select();
            }
                inputManager.isDraggingCard = true;
                inputManager.getEntityTarget = true;
                inputManager.currentSelectedCard = card;
        }

        else if(card.isDropped)
        {
            card.Deselect();
            inputManager.isDraggingCard = false;
        }

        else if (card.isClicked) 
        {
            if (!card.isSelected)
            {
                if (inputManager.currentSelectedCard != null)
                {
                    inputManager.currentSelectedCard.Deselect();
                    inputManager.currentSelectedCard.abortHandler();
                }

                card.Select();
                inputManager.getEntityTarget = true;
                inputManager.currentSelectedCard = card;
            }
            else
            {
                card.Deselect();
                inputManager.getEntityTarget = false;
                inputManager.currentSelectedCard = null;
            }
        }
        
    }

    public void AbortMethod()
    {
        print("AbortMove");
        card.Deselect();
        inputManager.getEntityTarget = false;
    }
}
