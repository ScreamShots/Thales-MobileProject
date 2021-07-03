using UnityEngine;
using UnityEngine.Audio;
 

 
public class MovementCard : MonoBehaviour
{
    public InteractableUI card;
    private InputManager inputManager;
    private UIHandler uiHandler;

    [Header("Audio")]
    private SoundHandler soundHandler;
    public AudioSource audioSource;
    public AudioMixerGroup targetGroup;
     
    public AudioClip descriptionAppearSound;
     
    public float descriptionAppearSoundVolume;
     
    public AudioClip cardSelectionSound;
     
    public float cardSelectionSoundVolume;



    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        uiHandler = GameManager.Instance.uiHandler;
        soundHandler = GameManager.Instance.soundHandler;

        card.clickHandler     += OnClickEvent;
        card.beginDragHandler += OnBeginDragEvent;
        card.endDragHandler   += OnEndDragEvent;
        card.abortHandler     += AbortMethod;
        card.holdHandler      += UpdateDescriptionText;
    }

   

    public void AbortMethod()
    {
        card.Deselect();
        inputManager.getEntityTarget = false;
        inputManager.currentSelectedCard = null;
    }

    public void OnClickEvent()
    {
            if(card.isSelected)
            {
                card.abortHandler();
            }
            else
            {
                if(inputManager.currentSelectedCard != null)
                {
                    inputManager.currentSelectedCard.abortHandler();
                }

            card.Select();
            audioSource.volume = Mathf.Clamp01(cardSelectionSoundVolume);
            soundHandler.PlaySound(cardSelectionSound, audioSource, targetGroup);
            inputManager.getEntityTarget = true;
            inputManager.currentSelectedCard = card;
        }
    }

    public void OnBeginDragEvent()
    {
        if (inputManager.currentSelectedCard != null)
        {
            inputManager.currentSelectedCard.abortHandler();
        }

        inputManager.isDraggingCard = true;
        inputManager.getEntityTarget = true;
        inputManager.currentSelectedCard = card;
    }

    public void OnEndDragEvent()
    {
        inputManager.isDraggingCard = false;
        inputManager.currentSelectedCard = null;
    }

    private void UpdateDescriptionText()
    {
        audioSource.volume = Mathf.Clamp01(descriptionAppearSoundVolume);
        soundHandler.PlaySound(descriptionAppearSound, audioSource, targetGroup);

        uiHandler.entityDeckUI.descriptionHeaderText.text = "Déplacement";//Expose string
        uiHandler.entityDeckUI.descriptionText.text = "<b>Déplace</b> le bâtiment sélectionné jusqu'à l'emplacement indiqué.";
    }

}
