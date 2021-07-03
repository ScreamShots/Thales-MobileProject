using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using PlayerEquipement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
 

 
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
     
    public AudioClip descriptionAppearSound;
     
    public float descriptionAppearSoundVolume;
     
    public AudioClip cardSelectionSound;
     
    public float cardSelectionSoundVolume;
     
    public AudioClip outOfChargeSound;
     
    public float outOfChargeSoundVolume;

    OceanEntities.Plane temp;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        uiHandler = GameManager.Instance.uiHandler;
        soundHandler = GameManager.Instance.soundHandler;



        for (int i = 0; i < GameManager.Instance.levelManager.playerOceanEntities.Count; i++)
        {
            if(GameManager.Instance.levelManager.playerOceanEntities[i].GetType() == typeof(OceanEntities.Plane))
            {
                temp = (OceanEntities.Plane) GameManager.Instance.levelManager.playerOceanEntities[i];

                sonobuyDeployer = (SonobuoyDeployer) temp.activeEquipement;
            }
        }       

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

        if(GameManager.Instance.playerController.currentSelectedEntity != temp)
            sonobuyDeployer.Abort();

        inputManager.currentSelectedCard = null;
    }

    public void OnClickEvent()
    {
        if (!card.isSelected)
        {
            if(inputManager.currentSelectedCard != null)
            {
                inputManager.currentSelectedCard.abortHandler();
            }

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
        if(inputManager.currentSelectedCard != null)
        {
            inputManager.currentSelectedCard.abortHandler();
        }

        if(sonobuyDeployer.chargeCount<=0)
        {
            audioSource.volume = Mathf.Clamp01(outOfChargeSoundVolume);
            soundHandler.PlaySound(outOfChargeSound, audioSource, targetGroup);
        }

        inputManager.isDraggingCard = true;
        inputManager.currentSelectedCard = card;
        inputManager.canUseCam = false;
    }

    public void OnEndDragEvent()
    {
        if (sonobuyDeployer.chargeCount > 0)
        {
            sonobuyDeployer.UseEquipement(GameManager.Instance.playerController.currentSelectedEntity);

            if (!isCharging)
                StartCoroutine(RechargeFeedback());
        }

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

        uiHandler.entityDeckUI.descriptionHeaderText.text = "Bouées Sonoflash";//Expose string
        uiHandler.entityDeckUI.descriptionText.text = "Les bouée SONOFLASH <b>détectent</b> les objets immergés dans une <b>zone</b> autour de la bouée sans donner la position précise de l'objet détecté.";//Expose stringS
    }

}
