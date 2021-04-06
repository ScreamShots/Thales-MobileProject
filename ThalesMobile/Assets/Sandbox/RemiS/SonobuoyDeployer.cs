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

        List<Sonobuoy> availaibleSonobuoy;
        List<Sonobuoy> usedSonobuoy;

        private void Awake()
        {
            equipementType = EquipementType.active;

            availaibleSonobuoy = new List<Sonobuoy>();
            usedSonobuoy = new List<Sonobuoy>();

            GameObject tempSonobuoy;

            for (int i = 0; i < poolSize; i++)
            {
                tempSonobuoy = Instantiate(sonobuoyPrefab, GameManager.Instance.levelManager.transform);
                availaibleSonobuoy.Add(tempSonobuoy.GetComponent<Sonobuoy>());
            }
        }

    }

}