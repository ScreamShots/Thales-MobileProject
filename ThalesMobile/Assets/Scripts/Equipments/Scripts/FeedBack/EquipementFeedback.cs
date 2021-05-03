using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public abstract class EquipementFeedback : MonoBehaviour
    {
        private Equipement source;

        public virtual void EquipementFeedbackInit(Equipement _source)
        {
            source = _source;
        }
    }
}