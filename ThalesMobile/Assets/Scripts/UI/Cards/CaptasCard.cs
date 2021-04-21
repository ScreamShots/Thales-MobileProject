using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;

public class CaptasCard : MonoBehaviour
{

    public InteractableUI card;
    public CaptasFour captas;

    private InputManager inputManager;
    Coroutine captasUse;



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
            if(!card.isSelected)
            {
                if (inputManager.currentSelectedCard != null)
                {
                    inputManager.currentSelectedCard.Deselect();
                    inputManager.currentSelectedCard.abortHandler();
                }
                card.Select();

                if (captas.readyToUse && captas.chargeCount > 0)
                {
                    captasUse = StartCoroutine(UseCaptas());
                }

                inputManager.currentSelectedCard = card;
            }
            else
            {
                AbortMethod();
                inputManager.currentSelectedCard = null;
            }
        }

        if (card.isDragged)
        {
            inputManager.isDraggingCard = true;
            inputManager.currentSelectedCard = card;
            inputManager.canUseCam = false;
        }
        else
        {
            if (inputManager.isDraggingCard && inputManager.currentSelectedCard == card)
            {
                inputManager.isDraggingCard = false;
                inputManager.canUseCam = true;

                if (captas.readyToUse && captas.chargeCount > 0)
                {
                    captas.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
                }

            }
        }
    }

    public void AbortMethod()
    {
        card.Deselect();
        if (captasUse != null)
            StopCoroutine(captasUse);
    }


    private IEnumerator UseCaptas()
    {
        print("StartCoroutine");
        yield return new WaitUntil(() => inputManager.touchingGame);
        captas.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
        card.Deselect();
    }
}
