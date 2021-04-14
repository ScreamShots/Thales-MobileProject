using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;

public class CaptasCard : MonoBehaviour
{

    public InteractableUI card;
    public CaptasFour captas;

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (card.isClicked)
        {
            if (inputManager.currentSelectedCard != null)
                inputManager.currentSelectedCard.Deselect();

            card.Select();

            if (captas.readyToUse && captas.chargeCount > 0)
                captas.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);

            inputManager.currentSelectedCard = card;
        }

        if (card.isDragged)
        {
            if (!card.isSelected)
                card.Select();

            inputManager.isDraggingCard = true;
            inputManager.currentSelectedCard = card;
        }
        else
        {
            if (inputManager.isDraggingCard)
            {
                inputManager.isDraggingCard = false;

                if (captas.readyToUse && captas.chargeCount >0)
                    captas.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
                card.Deselect();
            }
        }
    }
}
