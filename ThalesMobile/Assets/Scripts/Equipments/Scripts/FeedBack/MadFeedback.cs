using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
 

namespace PlayerEquipement
{
    

    public class MadFeedback : EquipementFeedback
    {
        Mad sourceMad;
        [SerializeField]
        Renderer madRenderer;

        [Header("Sound - Reveal")]
        [SerializeField]
        AudioMixerGroup targetGroup;
        [SerializeField ]
        AudioClip revealSound;
        [SerializeField ]
        float revealSoundVolume;
        [SerializeField]
        AudioSource revealSoundSource;
        

        float scaleFactor
        {
            get
            {
                if (sourceMad != null && sourceMad.GetType() == typeof(Mad) && madRenderer != null)
                {
                    return (sourceMad.range * 2) / madRenderer.bounds.size.x;
                }
                else return 0;
            }
        }

        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);


            if (_source.GetType() == typeof(Mad)) sourceMad = _source as Mad;
            else
            {
                print("FeedBack specified to equipement: " + _source.GetType().ToString() + " is not the right feedback");
                return;
            }

            madRenderer.transform.localScale = Vector3.one;
            madRenderer.transform.localScale *= scaleFactor;
        }

        public void RevealDetection()
        {
            revealSoundSource.volume = Mathf.Clamp(revealSoundVolume, 0, 1);
            GameManager.Instance.soundHandler.PlaySound(revealSound, revealSoundSource, targetGroup);
        }
    }
}