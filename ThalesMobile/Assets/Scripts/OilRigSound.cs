using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using NaughtyAttributes;
using Tweek.FlagAttributes;

[TweekClass]
public class OilRigSound : MonoBehaviour
{
    SoundHandler currentSoundHandler;
    CameraSettings currentCamSet;

    [Header("Ambient")]
    public AudioSource oilRigAmbiantSource;
    [TweekFlag(FieldUsage.Sound)]
    public AudioClip oilRigAmbiantClip;
    public AudioMixerGroup oilRigAmbiantTargetMixer;
    [TweekFlag(FieldUsage.Sound)]
    public float oilRigAmbiantVolume;
    [CurveRange(0, 0, 1, 1)]
    public AnimationCurve oilRigAmbiant3DVolume;

    private void Start()
    {
        currentSoundHandler = GameManager.Instance.soundHandler;
        currentCamSet = GameManager.Instance.cameraController.camSett;

        oilRigAmbiantSource.maxDistance = currentCamSet.maxHeight;
        oilRigAmbiantSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, SoundHandler.Get3DVolumeCurve(oilRigAmbiant3DVolume));

        oilRigAmbiantSource.volume = Mathf.Clamp01(oilRigAmbiantVolume);
        currentSoundHandler.PlaySound(oilRigAmbiantClip, oilRigAmbiantSource, oilRigAmbiantTargetMixer);

        transform.position = new Vector3(transform.position.x, currentCamSet.minHeight, transform.position.z);
    }
}
