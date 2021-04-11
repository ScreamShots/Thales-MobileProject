﻿using System.Collections.Generic;
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
        float deployerCooldown;

        [Header("Pool Params")]

        [SerializeField]
        GameObject sonobuoyPrefab;
        [SerializeField, Min(0)]
        int poolSize;

        List<SonobuoyInstance> availaibleSonobuoys;
        List<SonobuoyInstance> usedSonobuoys;

        public override void Init()
        {
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
            base.UseEquipement(user);

            GameManager.Instance.ExternalStartCoroutine(DeploySonoBuy(user));
        }

        IEnumerator DeploySonoBuy(PlayerOceanEntity user)
        {
            Debug.Log("Start");
            readyToUse = false;
            Vector2 userCurrentTarget = user.currentTargetPoint;

            //Set input manager to get a new target;
            GameManager.Instance.inputManager.getEntityTarget = true;
            yield return new WaitUntil(()=> user.currentTargetPoint != userCurrentTarget);

            //set the sonobuy new target
            Vector2 targetPos = user.currentTargetPoint;

            //Wait until the ship has arrived or target has changed
            yield return new WaitUntil(() => user.currentTargetPoint == user.nullVector || targetPos != user.currentTargetPoint);

            //then drop sonobuoy
            if (user.currentTargetPoint == user.nullVector)
            {
                Debug.Log("DROP");
                DropSonobuoy(targetPos);
                yield return new WaitForSeconds(deployerCooldown);
                readyToUse = true;
            }
            else
            {
                readyToUse = true;
            }
        }


        void DropSonobuoy(Vector2 targetPos)
        {
            availaibleSonobuoys[0].EnableSonobuoy(targetPos); //Add life time parameters to the methods
            usedSonobuoys.Add(availaibleSonobuoys[0]);
            availaibleSonobuoys.RemoveAt(0);
        }
    }
}