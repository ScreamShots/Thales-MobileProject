using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            //Pool Init
            GameObject tempSonobuoy;

            for (int i = 0; i < poolSize; i++)
            {
                tempSonobuoy = Instantiate(sonobuoyPrefab, GameManager.Instance.levelManager.transform);
                availaibleSonobuoys.Add(tempSonobuoy.GetComponent<SonobuoyInstance>());
            }
        }

        public override void UseEquipement(Coordinates userCoords)
        {
            Vector2 targetPos = Vector2.zero;

            base.UseEquipement(userCoords);

            //Input to selec a target pos where to drop the sonobuoy
            //Move Entity to the point
            
            //then drop sonobuoy ↓
            DropSonobuoy(targetPos);
        }

        public override void UseEquipement(Coordinates userCoords, Vector2 targetPos)
        {
            base.UseEquipement(userCoords, targetPos);
            DropSonobuoy(targetPos);
        }

        void DropSonobuoy(Vector2 targetPos)
        {
            availaibleSonobuoys[0].EnableSonobuoy(targetPos); //Add life time parameters to the methods
            usedSonobuoys.Add(availaibleSonobuoys[0]);
            availaibleSonobuoys.RemoveAt(0);
        }

    }
}