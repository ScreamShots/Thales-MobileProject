using UnityEngine;
using UnityEngine.Audio;
using Tweek.FlagAttributes;

[TweekClass]
public class PassiveCard : MonoBehaviour
{
    public InteractableUI card;
    private InputManager inputManager;
    private UIHandler uiHandler;

    [Header("Description")]
    public string descriptionHeader;
    public string descriptionText;

    [Header("Audio")]
    private SoundHandler soundHandler;
    public AudioSource audioSource;
    public AudioMixerGroup targetGroup;
    [TweekFlag(FieldUsage.Sound)]
    public AudioClip descriptionAppearSound;
    [TweekFlag(FieldUsage.Sound)]
    public float descriptionAppearSoundVolume;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
        uiHandler = GameManager.Instance.uiHandler;
        soundHandler = GameManager.Instance.soundHandler;

        card.holdHandler += UpdateDescriptionText;
    }

    private void UpdateDescriptionText()
    {
        audioSource.volume = Mathf.Clamp01(descriptionAppearSoundVolume);
        soundHandler.PlaySound(descriptionAppearSound, audioSource, targetGroup);
        uiHandler.entityDeckUI.descriptionHeaderText.text = descriptionHeader;//Expose string
        uiHandler.entityDeckUI.descriptionText.text = descriptionText;//Expose string
    }

}
