using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class Flash : Equipement
    {
        [SerializeField]
        float winningRange;
        [SerializeField]
        float extendedRange;
        [SerializeField]
        float dropDuration;
        [SerializeField]
        float revealDuration;

        LevelManager levelManager;

        public override void Awake()
        {
            equipementType = EquipementType.passive;

            levelManager = GameManager.Instance.levelManager;
        }

        public override void UseEquipement(Coordinates userCoords)
        {
            base.UseEquipement(userCoords);
            readyToUse = false;
            allCoroutines.Add(GameManager.Instance.ExternalStartCoroutine(DropFlash(userCoords)));
        }

        IEnumerator DropFlash(Coordinates userCoords)
        {
            float timer = 0;
            float distance = 0;

            while(timer < dropDuration)
            {
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }
            
            foreach(Submarine submarine in levelManager.enemyEntitiesInScene)
            {
                distance = Mathf.Abs(Vector2.Distance(submarine.coords.position, userCoords.position));
                if (distance <= winningRange) Debug.Log("Win"); /*Win the Game*/
                else if (distance <= extendedRange) Debug.Log("Near"); /*enable trail (SubMarine Methode with duration param)*/
            }
        }
    }
}
