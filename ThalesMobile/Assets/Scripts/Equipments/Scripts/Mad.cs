using UnityEngine;
using OceanEntities;

namespace PlayerEquipement
{
    /// <summary>
    ///  Rémi Sécher - 08/04/21 - Class that handle M.A.D. Equipement Behaviour
    /// </summary>
    
    [CreateAssetMenu(menuName = "Equipement/MAD")]
    public class Mad : Equipement
    {
        [Header("M.A.D. Params")]

        [Min(0)]
        public float range;

        MadFeedback madFeedback;
        LevelManager levelManager;

        public override void Init(PlayerOceanEntity user)
        {
            base.Init(user);

            levelManager = GameManager.Instance.levelManager;
            equipementType = EquipementType.passive;
            madFeedback = feedbackBehavior as MadFeedback;
        }

        float distance = 0f;
        public override void UseEquipement(PlayerOceanEntity user)
        {
            base.UseEquipement(user);
            
            //Loop through all detection object (sonobuoy or detectionPoint for exemple)
            //If they stand within the range and they detected something, allow them to display infos
            //If they leave range while in revealed state, cancel reveal autorization
            foreach(DetectionObject obj in levelManager.activatedDetectionObjects)
            {
                distance = Mathf.Abs(Vector2.Distance(obj.coords.position, user.coords.position));
                if (distance <= range && !obj.inMadRange)
                {
                    obj.inMadRange = true;
                    if(obj.detectionState == DetectionState.unknownDetection) madFeedback.RevealDetection();
                }
                else if(distance > range && obj.inMadRange) obj.inMadRange = false;
            }
        }
    }
}
