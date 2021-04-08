using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    /// <summary>
    ///  Rémi Sécher - 08/04/21 - Class that handle M.A.D. Equipement Behaviour
    /// </summary>
    
    public class Mad : Equipement
    {
        [Header("M.A.D. Params")]

        [SerializeField, Min(0)]
        float range;

        LevelManager levelManager;

        public override void Init()
        {
            base.Init();

            levelManager = GameManager.Instance.levelManager;
            equipementType = EquipementType.passive;
        }

        float distance = 0f;
        public override void UseEquipement(Coordinates userCoords)
        {
            base.UseEquipement(userCoords);
            
            //Loop through all detection object (sonobuoy or detectionPoint for exemple)
            //If they stand within the range and they detected something, allow them to display infos
            //If they leave range while in revealed state, cancel reveal autorization
            foreach(DetectionObject obj in levelManager.activatedDetectionObjects)
            {
                distance = Mathf.Abs(Vector2.Distance(obj.coords.position, userCoords.position));
                if (distance <= range && obj.detectionState != DetectionState.noDetection) obj.detectionState = DetectionState.revealedDetection;
                else if (obj.detectionState == DetectionState.revealedDetection) obj.detectionState = DetectionState.unknownDetection;
            }
        }
    }
}
