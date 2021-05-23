using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundMixerGroup { Master, UI, Music, Effect }

public class SoundHandler : MonoBehaviour
{
    [Header("Main Parameters")]
    public AudioMixer mainAudioMixer;
    public AudioMixerGroup masterGroup;

    [Range(-80f, 20f)]
    public float masterVolume;

    [Header("AudioGroups")]
    public AudioMixerGroup uiGroup;
    public AudioMixerGroup effectsGroup;
    public AudioMixerGroup musicGroup;

    [Header("StartVolumes")]
    public bool changeOnStart = false;
    [Range(-80f, 20f)] public float uiVolume;
    [Range(-80f, 20f)] public float effectsVolume;
    [Range(-80f, 20f)] public float musicVolume;

    void Start()
    {
        if (changeOnStart)
        {
            //Set groups volume.
            ChangeVolume(masterVolume, "masterVolume");
            ChangeVolume(uiVolume, "uiVolume");
            ChangeVolume(musicVolume,"musicVolume");
            ChangeVolume(effectsVolume, "effectsVolume");
        }
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

    //Change a volume
    public void ChangeVolume(float value, SoundMixerGroup targetGroup)
    {
        switch (targetGroup)
        {
            case SoundMixerGroup.Master:
                ChangeVolume(value, "masterVolume");
                break;

            case SoundMixerGroup.UI:
                ChangeVolume(value, "uiVolume");
                break;

            case SoundMixerGroup.Music:
                ChangeVolume(value, "musicVolume");
                break;

            case SoundMixerGroup.Effect:
                ChangeVolume(value, "effectsVolume");
                break;

            default:
                Debug.LogError("Can't find the SoundMixer Group", this);
                break;
        }
    }
    public void ChangeVolume(float value, string targetGroup)
    {
        mainAudioMixer.SetFloat(targetGroup, value!=0? Mathf.Log10(value) * 20 : -80);
        PlayerPrefs.SetFloat(targetGroup, value);
    }
}
