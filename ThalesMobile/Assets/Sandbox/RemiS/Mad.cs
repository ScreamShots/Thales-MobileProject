using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class Mad : Equipement
    {
        [SerializeField]
        float range;

        LevelManager levelManager;

        public override void Awake()
        {
            base.Awake();

            levelManager = GameManager.Instance.levelManager;
            equipementType = EquipementType.passive;
        }

        float distance = 0f;
        public override void UseEquipement(Coordinates userCoords)
        {
            base.UseEquipement(userCoords);

            foreach(DetectionObject obj in levelManager.activatedDetectionObjects)
            {
                distance = Mathf.Abs(Vector2.Distance(obj.coords.position, userCoords.position));
                if (distance <= range && obj.detectionState != DetectionState.noDetection) obj.detectionState = DetectionState.revealedDetection;
                else if (obj.detectionState == DetectionState.revealedDetection) obj.detectionState = DetectionState.unknownDetection;
            }
        }
    }
}
