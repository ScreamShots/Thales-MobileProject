using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;

namespace PlayerEquipement
{
    /// <summary>
    /// Rémi Sécher - 08/04/2021 - Class that handle behaviour of Flash Equipement
    /// </summary>
    
    [CreateAssetMenu(menuName = "Equipement/Flash")]
    public class Flash : Equipement
    {
        [Header("Flash Params")]

        [SerializeField, Min(0)]
        float winningRange;
        [SerializeField, Min(0)]
        float extendedRange;
        [SerializeField, Min(0)]
        float dropDuration;
        [SerializeField, Min(0)]
        float revealDuration;

        [Header("Flash Feedback Params")]
        [SerializeField]
        float heightOffset;
        
        LevelManager levelManager;

        public override void Init(PlayerOceanEntity user)
        {
            base.Init(user);
            equipementType = EquipementType.passive;
            levelManager = GameManager.Instance.levelManager;
        }

        public override void UseEquipement(PlayerOceanEntity user)
        {
            base.UseEquipement(user);
            readyToUse = false;
            allCoroutines.Add(GameManager.Instance.ExternalStartCoroutine(DropFlash(user.coords)));
        }

        IEnumerator DropFlash(Coordinates userCoords)
        {
            float timer = 0;
            float distance = 0;

            FlashFeedback flashFeedback = (FlashFeedback)feedbackBehavior;
            flashFeedback.DropFlash(dropDuration, new Vector3(currentUser.transform.position.x, currentUser.transform.position.y + heightOffset, currentUser.transform.position.z));

            //Wait the drop duration before apply effect
            //Insert Somehow feedback of drop here
            while(timer < dropDuration)
            {
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }
            
            //Test all submarine in the scene
            //If they are in winning range Win the game (need method implementation to win In GameManager probably)
            //If they are in extended range show submarine trail for the specified time (need method impletation for that in Submarine)
            foreach(Submarine submarine in levelManager.enemyEntitiesInScene)
            {
                distance = Mathf.Abs(Vector2.Distance(submarine.coords.position, userCoords.position));
                if (distance <= winningRange) Debug.Log("Win"); /*Win the Game*/
                else if (distance <= extendedRange) Debug.Log("Near"); /*enable trail (SubMarine Methode with duration param)*/
            }
        }
    }
}
