using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class HullSonarFeedback : EquipementFeedback
    {
        HullSonar sourceHullSonar;
        MeshRenderer waveRenderer;

        [Header("Visual Params")]
        [SerializeField]
        Material waveMaterial;
        
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
                    return (sourceHullSonar.range * 2) / waveRenderer.bounds.size.x;
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

            waveRenderer = GetComponent<MeshRenderer>();

            waveRenderer.material = waveMaterial;
            waveRenderer.sharedMaterial.SetFloat("VitesseOnde", waveSpeed);
            transform.localScale *= scaleFactor;
        }

        public void SetWaveSpeed(float _speed)
        {
            waveRenderer.sharedMaterial.SetFloat("VitesseOnde", (100f * expansionFactor) / _speed);
        }
    }
}

