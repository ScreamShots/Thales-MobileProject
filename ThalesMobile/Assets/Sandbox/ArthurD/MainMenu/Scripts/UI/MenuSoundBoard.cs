using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class MenuSoundBoard : MonoBehaviour
{
    [Header("Component")]
    public SoundHandler soundHandler;

    [Header("UI component")]
    public Slider soundSliderMaster;
    public Slider soundSliderUI;
    public Slider soundSliderMusic;
    public Slider soundSliderEffect;

    [Header("UI sound")]
    public AudioMixerGroup uiGroup;
    [Space(5)]
    public AudioClip closeWindow;
    public AudioClip lauchMission;
    public AudioClip error;
    public AudioClip screenReturn;
    public AudioClip selection;
    public AudioClip selectMission;
    public AudioClip optionSound;

    [Header("Music")]
    public AudioMixerGroup musicGroup;
    [Space(5)]
    public AudioClip thalesTheme;
    [Space(25)]
    public AudioSource themePlayer;

    void Start()
    {
        soundHandler = GameManager.Instance.soundHandler;

        PlayThalesTheme();

        //Place rightly the slider
        soundSliderMaster.value = PlayerPrefs.GetFloat("masterVolume", 0.5f);
        soundSliderUI.value = PlayerPrefs.GetFloat("uiVolume", 0.5f);
        soundSliderMusic.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        soundSliderEffect.value = PlayerPrefs.GetFloat("effectsVolume", 0.5f);
    }

    //UI sound
    public void PlayCloseWindow(AudioSource audioSource)
    {
        soundHandler.PlaySound(closeWindow, audioSource, uiGroup);
    }
    public void PlayLauchMission(AudioSource audioSource)
    {
        soundHandler.PlaySound(lauchMission, audioSource, uiGroup);
    }
    public void PlayScreenReturn(AudioSource audioSource)
    {
        soundHandler.PlaySound(screenReturn, audioSource, uiGroup);
    }
    public void PlaySelectionMission(AudioSource audioSource)
    {
        soundHandler.PlaySound(selectMission, audioSource, uiGroup);
    }
    public void PlaySelection(AudioSource audioSource)
    {
        soundHandler.PlaySound(selection, audioSource, uiGroup);
    }
    public void PlayOptionSound(AudioSource audioSource)
    {
        soundHandler.PlaySound(optionSound, audioSource, uiGroup);
    }
    public void PlayErrorSound(AudioSource audioSource)
    {
        soundHandler.PlaySound(error, audioSource, uiGroup);
    }

    //Music
    public void PlayThalesTheme()
    {
        soundHandler.PlaySound(thalesTheme, themePlayer, musicGroup);
    }

    //Change a specific volume
    public void ChangeMasterVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.Master);
        PlayerPrefs.SetFloat("masterVolume", value);
    }
    public void ChangeUIVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.UI);
        PlayerPrefs.SetFloat("uiVolume", value);
    }
    public void ChangeMusicVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.Music);
        PlayerPrefs.SetFloat("musicVolume", value);
    }
    public void ChangeEffectVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.Effect);
        PlayerPrefs.SetFloat("effectsVolume", value);
    }
}
