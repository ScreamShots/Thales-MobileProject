using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using System.Collections;

namespace PlayerEquipement
{
    /// <summary>
    /// Rémi Sécher - 08/04/21 - Class that handle Sonobuoy Deployer Equipement behaviour
    /// </summary>
    
    [CreateAssetMenu(menuName = "Equipement/SonobuyDeployer")]
    public class SonobuoyDeployer : Equipement
    {
        [Header("Sonobuoy Deployer Params")]

        [SerializeField, Min(0)]
        float sonobuoyLifeTime;
        [SerializeField, Min(0)]
        float sonobuoyRange;

        [Header("Pool Params")]

        [SerializeField]
        GameObject sonobuoyPrefab;
        [SerializeField, Min(0)]
        int poolSize;

        [HideInInspector]
        public List<SonobuoyInstance> availaibleSonobuoys;
        [HideInInspector]
        public List<SonobuoyInstance> usedSonobuoys;

        [Header("Feedback")]
        public GameObject sonobuyTargetFeedbackPrefab;
        private GameObject sonobuyTargetFeedback;
        private GameObject temp;

        private Coroutine coroutine;

        public override void Init(PlayerOceanEntity user)
        {
            base.Init(user);
            equipementType = EquipementType.active;
            availaibleSonobuoys = new List<SonobuoyInstance>();
            usedSonobuoys = new List<SonobuoyInstance>();
            readyToUse = true;

            //Pool Init
            GameObject tempSonobuoy;

            for (int i = 0; i < poolSize; i++)
            {
                tempSonobuoy = Instantiate(sonobuoyPrefab, GameManager.Instance.levelManager.transform);
                availaibleSonobuoys.Add(tempSonobuoy.GetComponent<SonobuoyInstance>());
            }
        }

        public override void UseEquipement(PlayerOceanEntity user)
        {
           coroutine =  GameManager.Instance.ExternalStartCoroutine(DeploySonoBuy(user));
        }

        IEnumerator DeploySonoBuy(PlayerOceanEntity user)
        {
            temp = user.enitityFeedback.targetPoint.transform.GetChild(0).transform.GetChild(0).gameObject;

            if(user.enitityFeedback.targetPoint.transform.GetChild(0).transform.childCount < 2)
            {
                sonobuyTargetFeedback = Instantiate(sonobuyTargetFeedbackPrefab, user.enitityFeedback.targetPoint.transform.GetChild(0).transform);
                sonobuyTargetFeedback.transform.SetAsLastSibling();
            }
            temp.SetActive(false);
            sonobuyTargetFeedback.SetActive(true);


            readyToUse = false;
            Vector2 userCurrentTarget = user.currentTargetPoint;

            //Set input manager to get a new target;
            GameManager.Instance.inputManager.getEntityTarget = true;
            yield return new WaitUntil(()=> user.currentTargetPoint != userCurrentTarget && Input.touchCount == 0);

            //set the sonobuy new target
            Vector2 targetPos = user.currentTargetPoint;

            //Wait until the ship has arrived or target has changed
            yield return new WaitUntil(() => user.currentTargetPoint == user.nullVector || targetPos != user.currentTargetPoint);

            //then drop sonobuoy
            if (user.currentTargetPoint == user.nullVector)
            {
                Debug.Log("DROP");
                temp.SetActive(true);
                sonobuyTargetFeedback.SetActive(false);

                DropSonobuoy(targetPos);
                readyToUse = true;
            }
            else
            {
                temp.SetActive(true);
                sonobuyTargetFeedback.SetActive(false);

                readyToUse = true;
            }
        }

        void DropSonobuoy(Vector2 targetPos)
        {
            availaibleSonobuoys[0].EnableSonobuoy(targetPos, sonobuoyRange, sonobuoyLifeTime, this);
            usedSonobuoys.Add(availaibleSonobuoys[0]);
            availaibleSonobuoys.RemoveAt(0);
            chargeCount--;
        }

        public void Abort()
        {
            GameManager.Instance.ExternalStopCoroutine(coroutine);
            readyToUse = true;
            GameManager.Instance.inputManager.getEntityTarget = false;

            temp.SetActive(true);
            if(sonobuyTargetFeedback!= null)
                sonobuyTargetFeedback.SetActive(false);
        }
    }
}