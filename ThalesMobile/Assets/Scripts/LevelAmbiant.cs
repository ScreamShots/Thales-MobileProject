using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using NaughtyAttributes;
 

 
public class LevelAmbiant : MonoBehaviour
{
    SoundHandler currentSoundHandler;

    [Header("Sea Ambient")]
    public AudioSource oceanSource;
     
    public AudioClip oceanClip;
    public AudioMixerGroup oceanTargetMixer;
     
    public float oceanVolume;
    [CurveRange(0, 0, 1, 1)]
    public AnimationCurve ocean3DVolume;

    CameraSettings currentCamSet;

    private void Start()
    {
        currentSoundHandler = GameManager.Instance.soundHandler;
        currentCamSet = GameManager.Instance.cameraController.camSett;
        oceanSource.maxDistance = currentCamSet.maxHeight;
        oceanSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, SoundHandler.Get3DVolumeCurve(ocean3DVolume));

        oceanSource.volume = Mathf.Clamp01(oceanVolume);
        currentSoundHandler.PlaySound(oceanClip, oceanSource, oceanTargetMixer);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, currentCamSet.minHeight, transform.position.z);
    }
}
