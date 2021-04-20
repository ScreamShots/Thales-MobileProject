using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class MadFeedback : EquipementFeedback
    {
        Mad sourceMad;
        Renderer madRenderer;

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

            madRenderer = GetComponent<Renderer>();

            transform.localScale *= scaleFactor;
        }
    }
}