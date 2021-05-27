using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OilRigSound : MonoBehaviour
{
    SoundHandler currentSoundHandler;
    CameraSettings currentCamSet;

    [Header("Ambient")]
    public AudioSource oceanCalmSource;
    public AudioClip oceanCalmClip;
    public AudioMixerGroup oceanCalmTargetMixer;
    public float oceanCalmVolume;
    public AnimationCurve oceanCalm3DVolume;
    [Space]
    public AudioSource oilRigAmbiantSource;
    public AudioClip oilRigAmbiantClip;
    public AudioMixerGroup oilRigAmbiantTargetMixer;
    public float oilRigAmbiantVolume;
    public AnimationCurve oilRigAmbiant3DVolume;

    private void Start()
    {
        currentSoundHandler = GameManager.Instance.soundHandler;
        currentCamSet = GameManager.Instance.cameraController.camSett;

        oceanCalmSource.maxDistance = currentCamSet.maxHeight;
        oceanCalmSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, oceanCalm3DVolume);

        oceanCalmSource.volume = Mathf.Clamp(0, 1, oceanCalmVolume);
        currentSoundHandler.PlaySound(oceanCalmClip, oceanCalmSource, oceanCalmTargetMixer);

        oilRigAmbiantSource.maxDistance = currentCamSet.maxHeight;
        oilRigAmbiantSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, oilRigAmbiant3DVolume);

        oilRigAmbiantSource.volume = Mathf.Clamp(0, 1, oilRigAmbiantVolume);
        currentSoundHandler.PlaySound(oilRigAmbiantClip, oilRigAmbiantSource, oilRigAmbiantTargetMixer);

        transform.position = new Vector3(transform.position.x, currentCamSet.minHeight, transform.position.z);
    }
}
