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
    public void CrossFade(AudioSource source, AudioClip clip, float fadeDuration, float newVolume = 0)
    {
        Coroutine fade = StartCoroutine(CrossfadeRoutine(source, clip, fadeDuration));
    }

    public IEnumerator CrossfadeRoutine(AudioSource source, AudioClip clip, float fadeDuration, float newVolume = 0)
    {
        float time = 0;
        float currentVolume = source.volume;

        while(time < fadeDuration)
        {
            source.volume = Mathf.Lerp(currentVolume, 0, time/ fadeDuration);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
        source.volume = 0;
        source.clip = clip;
        time = 0;

        while (time < fadeDuration)
        {
            if(newVolume == 0) source.volume = Mathf.Lerp(0,currentVolume, time / fadeDuration);
            else source.volume = Mathf.Lerp(0, newVolume, time / fadeDuration);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }

        source.volume = currentVolume;

        if (!source.isPlaying)
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
