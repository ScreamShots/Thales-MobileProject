using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using PlayerEquipement;

public class SonobuyDeployerCard : MonoBehaviour
{
    [Header("Elements")]
    public InteractableUI card;
    public SonobuoyDeployer sonobuyDeployer;

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        card.abortHandler += AbortMethod;
    }

    // Update is called once per frame
    void Update()
    {
        if (card.isClicked)
        {
            print("isClicked");
            if (!card.isSelected)
            {
                if (inputManager.currentSelectedCard != null)
                {
                    inputManager.currentSelectedCard.Deselect();
                    inputManager.currentSelectedCard.abortHandler();
                }

                if (sonobuyDeployer.readyToUse)
                {
                    sonobuyDeployer.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
                    card.Select();
                    inputManager.currentSelectedCard = card;
                }
            }
            else
            {
                card.Deselect();
                inputManager.currentSelectedCard = null;
                sonobuyDeployer.Abort();
            }
        }

        if (card.isDragged)
        {
            if (sonobuyDeployer.readyToUse)
                sonobuyDeployer.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);

            inputManager.isDraggingCard = true;
            inputManager.currentSelectedCard = card;
            inputManager.canUseCam = false;
            print("StopdraggingCaptas");
        }
        else
        {
            if(inputManager.isDraggingCard && inputManager.currentSelectedCard == card)
            {
                inputManager.isDraggingCard = false;
                inputManager.canUseCam = true;
                print("StopdraggingCaptas");
            }
        }
    }

    public void AbortMethod()
    {
        card.Deselect();
        sonobuyDeployer.Abort();
    }
}
