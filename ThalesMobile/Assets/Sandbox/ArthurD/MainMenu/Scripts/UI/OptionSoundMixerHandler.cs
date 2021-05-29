using UnityEngine.UI;
using UnityEngine;

public class OptionSoundMixerHandler : MonoBehaviour
{
    [Header("Component")]
    public SoundHandler soundHandler;
    [Space(5)]
    public Slider soundSliderMaster;
    public Slider soundSliderUI;
    public Slider soundSliderMusic;
    public Slider soundSliderEffect;

    void Start()
    {
        //Place rightly the slider
        soundSliderMaster.value = PlayerPrefs.GetFloat("masterVolume", 0.5f);
        soundSliderUI.value = PlayerPrefs.GetFloat("uiVolume", 0.5f);
        soundSliderMusic.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        soundSliderEffect.value = PlayerPrefs.GetFloat("effectsVolume", 0.5f);
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
