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

        card.abortHandler     += AbortMethod;
        card.clickHandler     += OnClickEvent;
        card.beginDragHandler += OnBeginDragEvent;
        card.endDragHandler   += OnEndDragEvent;
    }

    public void AbortMethod()
    {
        card.Deselect();
        sonobuyDeployer.Abort();
        inputManager.currentSelectedCard = null;
    }

    public void OnClickEvent()
    {
        if (!card.isSelected)
        {
            //Abort and deselect current selected card;
            if(inputManager.currentSelectedCard != null)
            {
                inputManager.currentSelectedCard.abortHandler();
            }

            //If possible use captas and select card.
            if (sonobuyDeployer.readyToUse && sonobuyDeployer.chargeCount > 0)
            {
                card.Select();
                inputManager.currentSelectedCard = card;
                sonobuyDeployer.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
            }
            else
            {
                //Unavailable feedback;
                print("Unavailable feedback click");
            }
        }
        else
        {
            card.abortHandler();
        }
    }

    public void OnBeginDragEvent()
    {
        //Abort and deselect current selected card.
        if(inputManager.currentSelectedCard != null)
        {
            inputManager.currentSelectedCard.abortHandler();
        }


        if (sonobuyDeployer.readyToUse && sonobuyDeployer.chargeCount > 0)
        {
            sonobuyDeployer.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
        }
        else
        {
            //Unavailable feedbaack
            print("Unavailable feedback drag");
        }

        inputManager.isDraggingCard = true;
        inputManager.currentSelectedCard = card;
        inputManager.canUseCam = false;
    }

    public void OnEndDragEvent()
    {
        inputManager.isDraggingCard = false;
        inputManager.currentSelectedCard = null;
        inputManager.canUseCam = true;
    }


}
