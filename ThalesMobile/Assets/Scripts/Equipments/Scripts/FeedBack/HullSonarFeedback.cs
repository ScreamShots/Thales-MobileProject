using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class HullSonarFeedback : EquipementFeedback
    {
        HullSonar sourceHullSonar;        

        [Header("Visual Params")]
        [SerializeField]
        MeshRenderer waveRenderer;
        [SerializeField]
        Material waveMaterial;
        [SerializeField]
        float minProgression, maxProgression;
        
        int expansionFactor
        {
            get
            {
                if (sourceHullSonar != null && sourceHullSonar.GetType() == typeof(HullSonar))
                {
                    if (sourceHullSonar.expand) return -1;
                    else return 1;
                }
                else return 1;
            }
        }

        float waveSpeed
        {
            get
            {
                if (sourceHullSonar != null && sourceHullSonar.GetType() == typeof(HullSonar))
                {
                    return (100f * expansionFactor) / sourceHullSonar.waveDuration;
                }                    
                else return 0;
            }
        }

        float scaleFactor
        {
            get
            {
                if (sourceHullSonar != null && sourceHullSonar.GetType() == typeof(HullSonar) && waveRenderer != null)
                {
                    return (sourceHullSonar.range * 2);
                }
                else return 0;
            }
        }
        

        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);            

            if (_source.GetType() == typeof(HullSonar)) sourceHullSonar = _source as HullSonar;
            else
            {
                print("FeedBack specified to equipement: " + _source.GetType().ToString() + " is not the right feedback");
                return;
            }

            waveRenderer.material = waveMaterial;
            waveRenderer.sharedMaterial.SetFloat("VitesseOnde", waveSpeed);
            waveRenderer.transform.localScale = Vector3.one * scaleFactor;
        }

        public void SetWaveSpeed(float _speed)
        {
            waveRenderer.sharedMaterial.SetFloat("VitesseOnde", (100f * expansionFactor) / _speed);
        }

        public void UpdateWaveProgression(float progressionRatio)
        {
            float progressionFactor = sourceHullSonar.expand ? 1 :-1;
            
            waveRenderer.material.SetFloat("ProgressionOnde", Mathf.Lerp(minProgression * progressionFactor, maxProgression * progressionFactor, progressionRatio));
        }
    }
}

