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

        List<SonobuoyInstance> availaibleSonobuoy;
        List<SonobuoyInstance> usedSonobuoy;

        private void Awake()
        {
            equipementType = EquipementType.active;

            availaibleSonobuoy = new List<SonobuoyInstance>();
            usedSonobuoy = new List<SonobuoyInstance>();

            GameObject tempSonobuoy;

            for (int i = 0; i < poolSize; i++)
            {
                tempSonobuoy = Instantiate(sonobuoyPrefab, GameManager.Instance.levelManager.transform);
                availaibleSonobuoy.Add(tempSonobuoy.GetComponent<SonobuoyInstance>());
            }
        }

    }

}