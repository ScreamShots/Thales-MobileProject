using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        public bool readyToUse { get; protected set; } = true;
        
        [Header("Equipement Params")]
        public int chargeMax;
        [SerializeField]
        protected int chargeStart;
        private int _chargeCount;

        [SerializeField]
        protected float loadingTime;
        public float loadPercent { get; protected set;}
        public bool isLoading { get; protected set; }

        protected List<Coroutine> allCoroutines = new List<Coroutine>();

        public int chargeCount
        {
            get { return _chargeCount; }
            protected set
            {
                if (value < _chargeCount) allCoroutines.Add(GameManager.Instance.ExternalStartCoroutine(LoadCharge()));
                if (value > chargeMax) _chargeCount = chargeMax;
                else _chargeCount = value;
            }
        }

        public virtual void Awake()
        {
            chargeCount = chargeStart;
        }

        IEnumerator LoadCharge()
        {
            float timer = 0;
            loadPercent = 0;
            isLoading = true;

            while (timer < loadingTime)
            {
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
                loadPercent = timer / loadingTime;
            }

            chargeCount++;
            isLoading = false;

            if (chargeCount < chargeMax) allCoroutines.Add(GameManager.Instance.ExternalStartCoroutine(LoadCharge()));
        }

        public virtual void UseEquipement(Coordinates userCoords)
        {
            if (equipementType == EquipementType.active)
            {
                chargeCount--;
            }
        }

        public virtual void UseEquipement(Coordinates userCoords, Vector2 targetPos)
        {
            if (equipementType == EquipementType.active)
            {
                chargeCount--;
            }
        }

        protected virtual void OnDestroy()
        {
            foreach(Coroutine coroutine in allCoroutines.ToList())
            {
                GameManager.Instance.ExternalStopCoroutine(coroutine);
            }            
        }
    }

}

