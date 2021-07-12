using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
 

 
public class CaptasCard : MonoBehaviour
{

    public InteractableUI card;
    public CaptasFour captas;

    [Header("Charge Feedback")]
    public TextMeshProUGUI chargeCountText;
    public Image fillBar;
    private bool isCharging;

    private InputManager inputManager;
    private UIHandler uiHandler;
    Coroutine captasUse;

    [Header("Audio")]
    private SoundHandler soundHandler;
    public AudioSource audioSource;
    public AudioMixerGroup targetGroup;
     
    public AudioClip descriptionAppearSound;
     
    public float descriptionAppearSoundVolume;
     
    public AudioClip cardSelectionSound;
     
    public float cardSelectionSoundVolume;
     
    public AudioClip outOfChargeSound;
     
    public float outOfChargeSoundVolume;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        uiHandler = GameManager.Instance.uiHandler;
        soundHandler = GameManager.Instance.soundHandler;

        chargeCountText.text = captas.chargeCount.ToString();

        for (int i = 0; i < GameManager.Instance.levelManager.playerOceanEntities.Count; i++)
        {
            if (GameManager.Instance.levelManager.playerOceanEntities[i].GetType() == typeof(OceanEntities.Ship))
            {
                OceanEntities.Ship temp = (OceanEntities.Ship)GameManager.Instance.levelManager.playerOceanEntities[i];

                captas = (CaptasFour)temp.activeEquipement;
            }
        }



        card.abortHandler     += AbortMethod;
        card.clickHandler     += OnClickEvent;
        card.beginDragHandler += OnBeginDragEvent;
        card.endDragHandler   += OnEndDragEvent;
        card.holdHandler      += UpdateDescriptionText;
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
            if (inputManager.currentSelectedCard != null)
            {
                inputManager.currentSelectedCard.abortHandler();
            }

            if (captas.readyToUse && captas.chargeCount > 0)
            {
                card.Select();
                captasUse = StartCoroutine(UseCaptas());
                inputManager.currentSelectedCard = card;
                audioSource.volume = Mathf.Clamp01(cardSelectionSoundVolume);
                soundHandler.PlaySound(cardSelectionSound, audioSource, targetGroup);
            }
            else
            {
                print("Unavailable feedback click");
                audioSource.volume = Mathf.Clamp01(outOfChargeSoundVolume);
                soundHandler.PlaySound(outOfChargeSound, audioSource, targetGroup);

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
            print("Unavailable feedback drag");
            audioSource.volume = Mathf.Clamp01(outOfChargeSoundVolume);
            soundHandler.PlaySound(outOfChargeSound, audioSource, targetGroup);
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
        yield return new WaitUntil(() => captas.isLoading);

        fillBar.fillAmount = 0;
        chargeCountText.text = captas.chargeCount.ToString();

        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount = captas.loadPercent;
            yield return new WaitForEndOfFrame();

            chargeCountText.text = captas.chargeCount.ToString();
        }
        fillBar.fillAmount = 1;

        isCharging = false;  
    }

    private void UpdateDescriptionText()
    {
        audioSource.volume = Mathf.Clamp01(descriptionAppearSoundVolume);
        soundHandler.PlaySound(descriptionAppearSound, audioSource, targetGroup);

        uiHandler.entityDeckUI.descriptionHeaderText.text = "CAPTAS-4";//Expose string
        uiHandler.entityDeckUI.descriptionText.text = "The CAPTAS-4 projects a wave that travels <b> the entire </b> board, <b> detecting </b> submerged objects in its path.";//Expose stringS
    }
}
