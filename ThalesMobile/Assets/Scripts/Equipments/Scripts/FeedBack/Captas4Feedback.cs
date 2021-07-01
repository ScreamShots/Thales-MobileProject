using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
 

namespace PlayerEquipement
{
     
    public class Captas4Feedback : EquipementFeedback
    {
        [SerializeField]
        MeshRenderer captasWaveRenderer;
        [SerializeField]
        MeshFilter captasMeshFiler;
        [SerializeField]
        float progressionMin;
        [SerializeField]
        float progressionMax;

        [Header("Sound - Wave Start")]
        [SerializeField]
        AudioMixerGroup waveTargetGroup;
        [SerializeField ]
        AudioClip waveSound;
        [SerializeField ]
        float waveSoundVolume;
        [SerializeField]
        AudioSource waveSoundSource;

        [Header("Sound - Deep Sea")]
        [SerializeField]
        AudioMixerGroup deepSeaTargetGroup;
        [SerializeField ]
        AudioClip deepSea;
        [SerializeField ]
        float deepSeaSoundVolume;
        [SerializeField]
        AudioSource deepSeaSoundSource;


        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);
            captasWaveRenderer.material.SetFloat("CaptasProgression", progressionMin);
            captasWaveRenderer.transform.localScale = Vector3.zero;
        }

        public void StartWave(float range, float duration)
        {
            captasWaveRenderer.transform.localScale = Vector3.one;
            float scaleFactor = (2 * range) / captasMeshFiler.sharedMesh.bounds.size.x;
            captasWaveRenderer.transform.localScale *= scaleFactor;
            captasWaveRenderer.transform.rotation = Quaternion.identity;

            waveSoundSource.volume = Mathf.Clamp(waveSoundVolume, 0, 1);
            GameManager.Instance.soundHandler.PlaySound(waveSound, waveSoundSource, waveTargetGroup);

            deepSeaSoundSource.volume = Mathf.Clamp(deepSeaSoundVolume, 0, 1);
            GameManager.Instance.soundHandler.PlaySound(deepSea, deepSeaSoundSource, deepSeaTargetGroup);

            StartCoroutine(WaveProgression(duration));
        }

        public void EndWave()
        {
            captasWaveRenderer.material.SetFloat("CaptasProgression", progressionMin);
            captasWaveRenderer.transform.localScale = Vector3.zero;
            GameManager.Instance.soundHandler.CrossFade(deepSeaSoundSource, null, 1f) ;
        }

        IEnumerator WaveProgression(float duration)
        {
            float timer = 0;
            
            while(timer < duration)
            {
                captasWaveRenderer.material.SetFloat("CaptasProgression", Mathf.Lerp(progressionMin, progressionMax, timer/duration));
                yield return null;
                timer += Time.deltaTime;
            }

            EndWave();
        }
    }
}
