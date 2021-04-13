using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCard : MonoBehaviour
{

    public InteractableUI card;
    private InputManager inputManager;


    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
    }

    // Update is called once per frame
    void Update()
    {
        if(card.isDragged)
        { 
            if(!card.isSelected)
                card.Select();


            inputManager.isDraggingCard = true;
            inputManager.getEntityTarget = true;
            inputManager.currentSelectedCard = card;
        }
        else
        {
            if (inputManager.isDraggingCard == true)
            {
                inputManager.isDraggingCard = false;
                card.Deselect();
            }
        }

        if(card.isClicked)
        {
            card.Select();
            inputManager.getEntityTarget = true;
            inputManager.currentSelectedCard = card;
        }
    }
}
