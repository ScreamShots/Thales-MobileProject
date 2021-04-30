using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class Captas4Feedback : EquipementFeedback
    {
        [SerializeField]
        MeshRenderer captasWaveRenderer;
        [SerializeField]
        float progressionMin;
        [SerializeField]
        float progressionMax;

        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);
            captasWaveRenderer.material.SetFloat("CaptasProgression", progressionMin);
            captasWaveRenderer.transform.localScale = Vector3.zero;
        }

        public void StartWave(float range, float duration)
        {
            captasWaveRenderer.transform.localScale = Vector3.one;
            float scaleFactor = (2 * range) / captasWaveRenderer.bounds.size.x;
            captasWaveRenderer.transform.localScale *= scaleFactor;
            StartCoroutine(WaveProgression(duration));
        }

        public void EndWave()
        {
            captasWaveRenderer.material.SetFloat("CaptasProgression", progressionMin);
            captasWaveRenderer.transform.localScale = Vector3.zero;
        }

        IEnumerator WaveProgression(float duration)
        {
            float timer = 0;
            
            while(timer < duration)
            {
                captasWaveRenderer.material.SetFloat("CaptasProgression", Mathf.Lerp(progressionMin, progressionMax, timer/duration));
                yield return new WaitForFixedUpdate();
                timer += Time.deltaTime;
            }

            EndWave();
        }
    }
}
