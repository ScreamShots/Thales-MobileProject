using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    /// <summary>
    /// Rémi Sécher - 06/04/2021 - Base for enum that determine the type of an equipement's action 
    /// </summary>

    public enum EquipementType { passive, active }

    /// <summary>
    /// Rémi Sécher - 06/04/2021 - Base for equipement creation 
    /// </summary>

    public abstract class Equipement : ScriptableObject
    {
        public EquipementType equipementType { get; protected set; }

        [HideInInspector]
        public bool readyToUse { get; protected set; }

        [SerializeField]
        int chargeMax;
        [SerializeField]
        float loadingTime;

        protected int chargeCount
        {
            get { return chargeCount; }
            set
            {
                if (value < chargeCount) GameManager.Instance.ExternalStartCoroutine(LoadCharge());
                chargeCount = value;
            }
        }

        IEnumerator LoadCharge()
        {
            yield return new WaitForSeconds(loadingTime);
            chargeCount++;
            if (chargeCount < chargeMax) GameManager.Instance.ExternalStartCoroutine(LoadCharge());
        }

        public virtual void UseEquipement(Transform userPos)
        {
            if (equipementType == EquipementType.active)
            {
                chargeCount--;
            }
        }

        protected virtual void OnDestroy()
        {
            GameManager.Instance.ExternalStopCoroutine(LoadCharge());
        }
    }

}

