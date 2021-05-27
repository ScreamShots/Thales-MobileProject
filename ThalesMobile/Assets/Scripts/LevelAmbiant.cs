using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LevelAmbiant : MonoBehaviour
{
    SoundHandler currentSoundHandler;

    [Header("Sea Ambient")]
    public AudioSource oceanSource;
    public AudioClip oceanClip;
    public AudioMixerGroup oceanTargetMixer;
    public float oceanVolume;
    public AnimationCurve ocean3DVolume;

    CameraSettings currentCamSet;

    private void Start()
    {
        currentSoundHandler = GameManager.Instance.soundHandler;
        currentCamSet = GameManager.Instance.cameraController.camSett;
        oceanSource.maxDistance = currentCamSet.maxHeight;
        oceanSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, ocean3DVolume);

        oceanSource.volume = Mathf.Clamp(0, 1, oceanVolume);
        currentSoundHandler.PlaySound(oceanClip, oceanSource, oceanTargetMixer);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, currentCamSet.minHeight, transform.position.z);
    }
}
