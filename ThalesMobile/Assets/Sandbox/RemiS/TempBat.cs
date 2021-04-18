using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;
using OceanEntities;
using NaughtyAttributes;
using System;

namespace Remi
{
    public class TempBat : PlayerOceanEntity
    {
        public List<Equipement> equipements;
        private enum AllEquipements {Captas4, HullSonar, Mad, SonobuoyDeployer, Flash }
        [SerializeField]
        AllEquipements equipementChoice;


        [Button("Activate Equipement")]
        public void ActiveEquipements()
        {
            Type equipementType = null;

            switch (equipementChoice)
            {
                case AllEquipements.Captas4:
                    equipementType = typeof(CaptasFour);
                    break;
                case AllEquipements.Flash:
                    equipementType = typeof(Flash);
                    break;
                case AllEquipements.HullSonar:
                    equipementType = typeof(HullSonar);
                    break;
                case AllEquipements.Mad:
                    equipementType = typeof(Mad);
                    break;                    
                case AllEquipements.SonobuoyDeployer:
                    equipementType = typeof(SonobuoyDeployer);
                    break;
            }

            foreach(Equipement equip in equipements)
            {
                if(equip.GetType() == equipementType)
                {
                    if(equip.readyToUse && equip.chargeCount > 0)
                    {
                        equip.UseEquipement(this);
                    }
                }
            }
        }

        [Button("ResetCoords")]
        public void ResetCoord()
        {
            coords.position = Coordinates.ConvertWorldToVector2(transform.position);
        }


        private void Start()
        {
            coords.position = Coordinates.ConvertWorldToVector2(transform.position);

            foreach(Equipement equip in equipements)
            {
                equip.Init(this);
            }
        }

        private void Update()
        {
            foreach (Equipement equip in equipements)
            {
                if (equip.GetType() == typeof(HullSonar))
                {
                    if (equip.readyToUse)
                    {
                        equip.UseEquipement(this);
                    }
                }
            }
        }

        public override void Move(Vector2 targetPosition)
        {
            throw new System.NotImplementedException();
        }

        public override void PathFinding()
        {
            throw new System.NotImplementedException();
        }

        public override void Waiting()
        {
            throw new System.NotImplementedException();
        }                
    }
}

