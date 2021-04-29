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
            allCoroutines.Add(GameManager.Instance.ExternalStartCoroutine(DropFlash(user)));
        }

        IEnumerator DropFlash(PlayerOceanEntity user)
        {
            float timer = 0;
            float distance = 0;

            Environnement currentEnviro = GameManager.Instance.levelManager.environnement;
            Zone testedZone = currentEnviro.zones[currentEnviro.ZoneIn(userCoords.position) - 1];

            FlashFeedback flashFeedback = (FlashFeedback)feedbackBehavior;
            flashFeedback.DropFlash(dropDuration, new Vector3(currentUser.transform.position.x, currentUser.transform.position.y + heightOffset, currentUser.transform.position.z));

            Helicopter helicopter = (Helicopter)user;
            helicopter.isDroppingFlash = true;

            //Wait the drop duration before apply effect
            //Insert Somehow feedback of drop here
            while(timer < dropDuration)
            {
                yield return new WaitForFixedUpdate();
                timer += Time.deltaTime;
            }

            helicopter.isDroppingFlash = false;

            //Test all submarine in the scene
            //If they are in winning range Win the game (need method implementation to win In GameManager probably)
            //If they are in extended range show submarine trail for the specified time (need method impletation for that in Submarine)
            foreach (Submarine submarine in levelManager.enemyEntitiesInScene)
            {
                distance = Mathf.Abs(Vector2.Distance(submarine.coords.position, user.coords.position));
                if (distance <= winningRange)
                {
                    GameManager.Instance.uiHandler.victoryScreenManager.Victory(true);
                }
                else if (distance <= extendedRange && testedZone.state != ZoneState.SeaTurbulent) submarine.MaterialChangedByFlash(revealDuration);
            }
        }
    }
}
