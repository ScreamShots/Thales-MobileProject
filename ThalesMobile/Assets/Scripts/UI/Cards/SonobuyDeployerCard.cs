using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using PlayerEquipement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using Tweek.FlagAttributes;

[TweekClass]
public class SonobuyDeployerCard : MonoBehaviour
{
    [Header("Elements")]
    public InteractableUI card;
    public SonobuoyDeployer sonobuyDeployer;

    private InputManager inputManager;
    private UIHandler uiHandler;

    [Header("Charge Feedback")]
    public TextMeshProUGUI chargeCountText;
    public Image fillBar;
    private bool isCharging;

    [Header("Audio")]
    private SoundHandler soundHandler;
    public AudioSource audioSource;
    public AudioMixerGroup targetGroup;
    [TweekFlag(FieldUsage.Sound)]
    public AudioClip descriptionAppearSound;
    [TweekFlag(FieldUsage.Sound)]
    public float descriptionAppearSoundVolume;
    [TweekFlag(FieldUsage.Sound)]
    public AudioClip cardSelectionSound;
    [TweekFlag(FieldUsage.Sound)]
    public float cardSelectionSoundVolume;
    [TweekFlag(FieldUsage.Sound)]
    public AudioClip outOfChargeSound;
    [TweekFlag(FieldUsage.Sound)]
    public float outOfChargeSoundVolume;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        uiHandler = GameManager.Instance.uiHandler;
        soundHandler = GameManager.Instance.soundHandler;

        chargeCountText.text = sonobuyDeployer.chargeCount.ToString();

        card.abortHandler     += AbortMethod;
        card.clickHandler     += OnClickEvent;
        card.beginDragHandler += OnBeginDragEvent;
        card.endDragHandler   += OnEndDragEvent;
        card.holdHandler      += UpdateDescriptionText;
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
                audioSource.volume = Mathf.Clamp01(cardSelectionSoundVolume);
                soundHandler.PlaySound(cardSelectionSound, audioSource, targetGroup);


                if (!isCharging)
                    StartCoroutine(RechargeFeedback());
            }
            else
            {
                //Unavailable feedback;
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
        //Abort and deselect current selected card.
        if(inputManager.currentSelectedCard != null)
        {
            inputManager.currentSelectedCard.abortHandler();
        }


        if (sonobuyDeployer.chargeCount > 0)
        {
            sonobuyDeployer.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);

            if (!isCharging)
                StartCoroutine(RechargeFeedback());
        }
        else
        {
            //Unavailable feedbaack
            print("Unavailable feedback drag");
            audioSource.volume = Mathf.Clamp01(outOfChargeSoundVolume);
            soundHandler.PlaySound(outOfChargeSound, audioSource, targetGroup);
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

    public IEnumerator RechargeFeedback()
    {
        isCharging = true;
        yield return new WaitUntil(() => sonobuyDeployer.isLoading);

        fillBar.fillAmount = 0;
        chargeCountText.text = sonobuyDeployer.chargeCount.ToString();

        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount = sonobuyDeployer.loadPercent;
            yield return new WaitForEndOfFrame();

            chargeCountText.text = sonobuyDeployer.chargeCount.ToString();
        }
        fillBar.fillAmount = 1;

        isCharging = false;
    }

    private void UpdateDescriptionText()
    {
        audioSource.volume = Mathf.Clamp01(descriptionAppearSoundVolume);
        soundHandler.PlaySound(descriptionAppearSound, audioSource, targetGroup);

        uiHandler.entityDeckUI.descriptionHeaderText.text = "Sonobuy Card";//Expose string
        uiHandler.entityDeckUI.descriptionText.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";//Expose stringS
    }

}
