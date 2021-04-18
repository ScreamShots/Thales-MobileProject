﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OceanEntities;

namespace PlayerEquipement
{
    /// <summary>
    /// Rémi Sécher - 08/04/21 - Class that handle HullSonar Equipment Bahaviour
    /// </summary>
    
    [CreateAssetMenu(menuName = "Equipement/HullSonar")]
    public class HullSonar : Equipement
    {
        [Header("Hull Sonar Params")]

        [Min(0)]
        public float range;
        [Min(0)]
        public float waveDuration;
        [SerializeField, Min(0)]
        float pointFadeDuration;

        [Header("Pooling Params")]
        [SerializeField]
        GameObject detectionPointPrefab;
        [SerializeField, Min(0)]
        int poolSize;        

        [HideInInspector]
        public List<HullSonarDetectionPoint> availableDetectionPoints;
        [HideInInspector]
        public List<HullSonarDetectionPoint> usedDetectionPoints;

        ////Debug
        //[SerializeField]
        //GameObject testPrefab;
        //GameObject test;

        LevelManager levelManager;

        public override void Init(PlayerOceanEntity user)
        {
            base.Init(user);

            levelManager = GameManager.Instance.levelManager;
            equipementType = EquipementType.passive;

            //Pool initialization
            GameObject tempDetectionPoint;

            availableDetectionPoints = new List<HullSonarDetectionPoint>();
            usedDetectionPoints = new List<HullSonarDetectionPoint>();

            for (int i = 0; i < poolSize; i++)
            {
                tempDetectionPoint = Instantiate(detectionPointPrefab, GameManager.Instance.levelManager.transform);
                availableDetectionPoints.Add(tempDetectionPoint.GetComponent<HullSonarDetectionPoint>());
            }

            //test = Instantiate(testPrefab);
            //test.transform.position = Coordinates.ConvertVector2ToWorld(user.coords.position);

            readyToUse = true;
        }

        public override void UseEquipement(PlayerOceanEntity user)
        {
            base.UseEquipement(user);

            readyToUse = false;
            GameManager.Instance.ExternalStartCoroutine(SonarWave(user.coords));
        }

        IEnumerator SonarWave(Coordinates userCoords)
        {
            Coordinates detectableCoords;
            Coordinates pointCoords;
            float distance;
            float waveRange = range;
            float waveTime = 0;
            float padding = 0;

            while (waveTime < waveDuration)
            {
                //test.transform.position = new Vector3(waveRange, 0, 0);

                //loop through every detection point link to this Equipement
                //test if they are at the current tested distance to the player entity
                //if so remove them
                foreach (HullSonarDetectionPoint detectionPoint in usedDetectionPoints.ToList())
                {
                    pointCoords = new Coordinates(detectionPoint.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, pointCoords.position));

                    if (distance >= waveRange && distance <= waveRange + padding)
                    {
                        detectionPoint.DesactivatePoint();                        
                    }
                }

                //loop through every detectable entities in the scene
                //test if they are at the current tested distante to the player entity
                //if so place a point on their position 
                foreach (DetectableOceanEntity detectable in levelManager.submarineEntitiesInScene)
                {
                    detectableCoords = new Coordinates(detectable.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, detectableCoords.position));

                    if (distance >= waveRange && distance <= waveRange + padding && detectable.currentDetectableState != DetectableState.cantBeDetected)
                    {
                        if (availableDetectionPoints.Count > 0)
                        {
                            availableDetectionPoints[0].ActivatePoint(detectable, pointFadeDuration, this);
                            usedDetectionPoints.Add(availableDetectionPoints[0]);
                            availableDetectionPoints.RemoveAt(0);
                        }
                        else Debug.Log("Not Enough object in pool");
                        
                    }
                }                

                //Make the wave progress, depend of the progression in the duration
                //Security added to not pass through entities that will be beetween two precise wave position check (padding)
                waveTime += Time.deltaTime;
                padding = waveRange - (range * (1 - (waveTime / waveDuration)));
                waveRange = range * (1 - (waveTime / waveDuration));

                yield return new WaitForEndOfFrame();
            }

            readyToUse = true;
        }
    }
}

