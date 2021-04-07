using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class SonobuoyDeployer : Equipement
    {
        GameObject sonobuoyPrefab;
        [SerializeField]
        int poolSize;

        List<Sonobuoy> availaibleSonobuoys;
        List<Sonobuoy> usedSonobuoys;

        public override void Awake()
        {
            equipementType = EquipementType.active;

            availaibleSonobuoys = new List<Sonobuoy>();
            usedSonobuoys = new List<Sonobuoy>();

            GameObject tempSonobuoy;

            for (int i = 0; i < poolSize; i++)
            {
                tempSonobuoy = Instantiate(sonobuoyPrefab, GameManager.Instance.levelManager.transform);
                availaibleSonobuoys.Add(tempSonobuoy.GetComponent<Sonobuoy>());
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
            availaibleSonobuoys[0].EnableSonobuoy(targetPos);
            usedSonobuoys.Add(availaibleSonobuoys[0]);
            availaibleSonobuoys.RemoveAt(0);
        }

    }
}