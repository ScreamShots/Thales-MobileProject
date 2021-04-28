using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundHandler : MonoBehaviour
{
    private UIHandler uiHandler;



    [Header("Main Parameters")]
    public AudioMixer mainAudioMixer;
    public AudioMixerGroup masterGroup;

    [Range(-80f, 20f)]
    public float masterVolume;

    [Header("AudioGroups")]
    public AudioMixerGroup uiGroup;
    public AudioMixerGroup effectsGroup;
    public AudioMixerGroup musicGroup;

    [Header("Volumes")]

    [Range(-80f,20f)]
    public float uiVolume;

    [Range(-80f, 20f)]
    public float effectsVolume;

    [Range(-80f, 20f)]
    public float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        //Set groups volume.
        mainAudioMixer.SetFloat("masterVolume", masterVolume);
        mainAudioMixer.SetFloat("uiVolume", uiVolume);
        mainAudioMixer.SetFloat("musicVolume", musicVolume);
        mainAudioMixer.SetFloat("effectsVolume", effectsVolume);

        uiHandler = GameManager.Instance.uiHandler;
    }

    public void PlaySound(AudioClip clip, AudioSource source, AudioMixerGroup targetGroup)
    {
        source.outputAudioMixerGroup = targetGroup;
        source.clip = clip;

        source.Play();
    }

    public void StopSound(AudioSource source, bool pause)
    {
        if(source.isPlaying)
        {
            if(pause)
                source.Pause();
            else
                source.Stop();
        }
    }

    public void UnPause(AudioSource source)
    {
        if (source.clip != null)
            source.UnPause();
    }

    public void ChangeVolume(float value, string targetGroup)
    {
        mainAudioMixer.SetFloat(targetGroup, (value * 100) - 20);
    }

}
