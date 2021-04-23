using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;
using TMPro;
using UnityEngine.UI;

public class CaptasCard : MonoBehaviour
{

    public InteractableUI card;
    public CaptasFour captas;

    [Header("Charge Feedback")]
    public TextMeshProUGUI chargeCountText;
    public Image fillBar;
    private bool isCharging;

    private InputManager inputManager;
    Coroutine captasUse;



    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;

        chargeCountText.text = captas.chargeCount.ToString();

        card.abortHandler += AbortMethod;
        card.clickHandler += OnClickEvent;
        card.beginDragHandler += OnBeginDragEvent;
        card.endDragHandler += OnEndDragEvent;
    }

    public void AbortMethod()
    {
        card.Deselect();
        inputManager.currentSelectedCard = null;
        if (captasUse != null)
            StopCoroutine(captasUse);
    }

    public void OnClickEvent()
    {
        if (!card.isSelected)
        {
            //Abort and deselect current selected card;
            if (inputManager.currentSelectedCard != null)
            {
                inputManager.currentSelectedCard.abortHandler();
            }

            //If possible use captas and select card.
            if (captas.readyToUse && captas.chargeCount > 0)
            {
                card.Select();
                captasUse = StartCoroutine(UseCaptas());
                inputManager.currentSelectedCard = card;
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
        inputManager.isDraggingCard = true;
        inputManager.currentSelectedCard = card;
        inputManager.canUseCam = false;
    }

    public void OnEndDragEvent()
    {
        inputManager.isDraggingCard = false;
        inputManager.canUseCam = true;
        inputManager.currentSelectedCard = null;

        if (captas.readyToUse && captas.chargeCount > 0)
        {
            captas.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
            StartCoroutine(RechargeFeedback());
        }
        else
        {
            //Unavailable feedback;
            print("Unavailable feedback drag");
        }
    }

    private IEnumerator UseCaptas()
    {
        yield return new WaitUntil(() => inputManager.touchingGame);
        captas.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);
        chargeCountText.text = captas.chargeCount.ToString();

        if (!isCharging)
            StartCoroutine(RechargeFeedback());

        card.Deselect();
    }

    public IEnumerator RechargeFeedback()
    {
        isCharging = true;
        print(captas.isLoading);
        yield return new WaitUntil(() => captas.isLoading);

        fillBar.fillAmount = 0;
        print(captas.chargeCount);
        chargeCountText.text = captas.chargeCount.ToString();
        while (captas.loadPercent < 1)
        {
            fillBar.fillAmount = captas.loadPercent;
            yield return new WaitForEndOfFrame();
        }
        fillBar.fillAmount = 1;

        yield return null;

        print(captas.chargeCount);

        if (captas.chargeCount < captas.chargeMax)
        {
            chargeCountText.text = captas.chargeCount.ToString();
            StartCoroutine(RechargeFeedback());
        }
        else
        {
            isCharging = false;
            chargeCountText.text = captas.chargeCount.ToString();
        }
        
    }


}
