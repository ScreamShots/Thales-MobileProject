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

        [SerializeField]
        float waveDuration;
        [Min(0)]
        public float winningRange;
        [Min(0)]
        public float extendedRange;
        [SerializeField, Min(0)]
        public float dropDuration;
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

            int temp = currentEnviro.ZoneIn(user.coords.position);

            Zone testedZone = currentEnviro.zones[currentEnviro.ZoneIn(user.coords.position) - 1];

            FlashFeedback flashFeedback = (FlashFeedback)feedbackBehavior;
            flashFeedback.DropFlash(dropDuration, new Vector3(currentUser.transform.position.x, currentUser.transform.position.y + heightOffset, currentUser.transform.position.z));

            Helicopter helicopter = (Helicopter)user;
            helicopter.isDroppingFlash = true;

            //Wait the drop duration before apply effect
            //Insert Somehow feedback of drop here
            while(timer < dropDuration)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            helicopter.isDroppingFlash = false;

            //Test all submarine in the scene
            //If they are in winning range Win the game (need method implementation to win In GameManager probably)
            //If they are in extended range show submarine trail for the specified time (need method impletation for that in Submarine)

            float waveTimer = 0;
            float waveProgress = 0; 
            float wavePadding = 0;
            bool detectionTestEntity = false;

            while (waveTimer < waveDuration)
            {

                (feedbackBehavior as FlashFeedback).UpdateWaveProgression(waveProgress/extendedRange, Mathf.Clamp01(waveProgress/winningRange));

                foreach (DetectableOceanEntity detectable in levelManager.submarineEntitiesInScene)
                {
                    distance = Mathf.Abs(Vector2.Distance(detectable.coords.position, user.coords.position));

                    detectionTestEntity = distance <= waveProgress && distance >= waveProgress + wavePadding;

                    if(detectionTestEntity && detectable.currentDetectableState != DetectableState.cantBeDetected)
                    {
                        if (distance <= winningRange)
                        {
                            if(detectable.GetType() == typeof(Submarine)) GameManager.Instance.uiHandler.victoryScreenManager.Victory(true);
                            else if(detectionTestEntity && detectable.currentDetectableState != DetectableState.cantBeDetected)
                            {
                                if (detectable.linkedGlobalDetectionPoint.activated) detectable.linkedGlobalDetectionPoint.UpdatePoint();
                                else detectable.linkedGlobalDetectionPoint.InitPoint();
                            }
                        }
                        else if (distance >= winningRange)
                        {
                            if (detectable.GetType() == typeof(Submarine) && testedZone.state != ZoneState.SeaTurbulent)
                            {
                                (detectable as Submarine).MaterialChangedByFlash(revealDuration);
                            }

                            if (detectionTestEntity && detectable.currentDetectableState != DetectableState.cantBeDetected)
                            {
                                if (detectable.linkedGlobalDetectionPoint.activated) detectable.linkedGlobalDetectionPoint.UpdatePoint();
                                else detectable.linkedGlobalDetectionPoint.InitPoint();
                            }
                        }
                    }                    
                }

                yield return null;
                waveTimer += Time.deltaTime;
                wavePadding = waveProgress - (extendedRange * (waveTimer / waveDuration));
                waveProgress = extendedRange * (waveTimer / waveDuration);
            }

            (feedbackBehavior as FlashFeedback).UpdateWaveProgression(0, 0, true);
            readyToUse = true;
        }
    }
}
