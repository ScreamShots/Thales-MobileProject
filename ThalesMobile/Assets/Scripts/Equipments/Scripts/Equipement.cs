using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OceanEntities;

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

        [Min(0)]
        public int chargeMax;
        [SerializeField, Min(0)]
        protected int chargeStart;
        private int _chargeCount;

        [SerializeField, Min(0)]
        protected float loadingTime;
        [HideInInspector]
        public float loadPercent { get; protected set;}
        [HideInInspector]
        public bool isLoading { get; protected set; }

        protected List<Coroutine> allCoroutines = new List<Coroutine>();
        Coroutine coolDownCouroutine = null;

        //Phantom field that call the CoolDown method that add charge to the equipement if one is used 
        //(only if the value is change to a lower one → use of a charge and if the coolDown is not already running)
        [HideInInspector]
        public int chargeCount
        {
            get { return _chargeCount; }
            protected set
            {
                if (value < _chargeCount && coolDownCouroutine == null) StartCoolDown();
                if (value > chargeMax) _chargeCount = chargeMax;
                else _chargeCount = value;
            }
        }

        //Init Equipement. Call this on all PlayerEntity Start() that need to use equipement 
        public virtual void Init()
        {
            chargeCount = chargeStart;
        }

        void StartCoolDown()
        {
            coolDownCouroutine = GameManager.Instance.ExternalStartCoroutine(LoadCharge());
            allCoroutines.Add(coolDownCouroutine);
        }

        //Coroutine that handle coolDown before adding new charges to equipement after one is used
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

            //Restart CD if number of charge is not max
            coolDownCouroutine = null;
            if (chargeCount < chargeMax) StartCoolDown();
        }

        //Call this from player Entity to launch equipement Behaviour
        //Override this in inherited class to specify equipement bahaviour (keep base → handle charge use)
        public virtual void UseEquipement(PlayerOceanEntity user)
        {
            if (equipementType == EquipementType.active)
            {
                chargeCount--;
            }
        }

        //Temp alternative to launch equipement for Sonobuoy deployer. 
        //Delete this once full behaviour is developped on Sonobuoy Deployer.
        public virtual void UseEquipement(Coordinates userCoords, Vector2 targetPos)
        {
            if (equipementType == EquipementType.active)
            {
                chargeCount--;
            }
        }

        //Security to stop all linked coroutine on object destruction.
        protected virtual void OnDestroy()
        {
            foreach(Coroutine coroutine in allCoroutines.ToList())
            {
                if(coroutine != null)GameManager.Instance.ExternalStopCoroutine(coroutine);
            }            
        }
    }

}

