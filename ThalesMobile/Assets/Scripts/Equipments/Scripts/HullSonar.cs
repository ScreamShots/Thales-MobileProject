using System.Collections;
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
        [SerializeField, Range(0, 1)]
        float turbulentSeaSpeedReductionFactor;
        [SerializeField, Min(0)]
        float pointFadeDuration;
        public bool expand;

        [Header("Pooling Params")]
        [SerializeField]
        GameObject detectionPointPrefab;
        [SerializeField, Min(0)]
        int poolSize;        

        [HideInInspector]
        public List<HullSonarDetectionPoint> availableDetectionPoints;
        [HideInInspector]
        public Dictionary<DetectableOceanEntity ,HullSonarDetectionPoint> usedDetectionPoints;

        ////Debug
        [SerializeField]
        GameObject testPrefab;
        GameObject test;

        LevelManager levelManager;

        public override void Init(PlayerOceanEntity user)
        {
            base.Init(user);

            levelManager = GameManager.Instance.levelManager;
            equipementType = EquipementType.passive;

            //Pool initialization
            GameObject tempDetectionPoint;

            availableDetectionPoints = new List<HullSonarDetectionPoint>();
            usedDetectionPoints = new Dictionary<DetectableOceanEntity, HullSonarDetectionPoint>();

            for (int i = 0; i < poolSize; i++)
            {
                tempDetectionPoint = Instantiate(detectionPointPrefab, GameManager.Instance.levelManager.transform);
                availableDetectionPoints.Add(tempDetectionPoint.GetComponent<HullSonarDetectionPoint>());
            }

            test = Instantiate(testPrefab);
            test.transform.position = Coordinates.ConvertVector2ToWorld(user.coords.position);

            readyToUse = true;
        }

        public override void UseEquipement(PlayerOceanEntity user)
        {
            base.UseEquipement(user);

            readyToUse = false;
            GameManager.Instance.ExternalStartCoroutine(SonarWave());
        }

        IEnumerator SonarWave()
        {
            HullSonarFeedback feedback = feedbackBehavior as HullSonarFeedback;

            Coordinates userCoords;
            Coordinates detectableCoords;
            Coordinates pointCoords;

            float distance;
            float waveRange;

            float waveTime = 0;
            float padding = 0;
            float waveMaxDuration;

            bool detectionTestPoint;
            bool detectionTestEntity;

            if (levelManager.environnement.zones[levelManager.environnement.ZoneIn(currentUser.coords.position) - 1].state == ZoneState.SeaTurbulent)
                waveMaxDuration = waveDuration / turbulentSeaSpeedReductionFactor;
            else waveMaxDuration = waveDuration;

            feedback.SetWaveSpeed(waveMaxDuration);

            if (expand) waveRange = 0;
            else waveRange = range;

            while (waveTime < waveMaxDuration)
            {
                userCoords = currentUser.coords;
                test.transform.position = new Vector3(waveRange + userCoords.position.x, 0, userCoords.position.y);

                //loop through every detection point link to this Equipement
                //test if they are at the current tested distance to the player entity
                //test if their linked detectable pos is different from the one it is on and that the detectable is still open to detection
                //if so remove them
                //else restart the fade timer
                foreach (KeyValuePair<DetectableOceanEntity, HullSonarDetectionPoint> usedPair in usedDetectionPoints.ToDictionary(x => x.Key, x => x.Value))
                {
                    pointCoords = usedPair.Value.coords;
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, pointCoords.position));

                    if (expand) detectionTestPoint = distance <= waveRange && distance >= waveRange + padding;
                    else detectionTestPoint = distance >= waveRange && distance <= waveRange + padding;

                    if (detectionTestPoint)
                    {
                        if(usedPair.Key.coords.position != usedPair.Value.coords.position || usedPair.Key.currentDetectableState == DetectableState.cantBeDetected)
                        {
                            usedPair.Value.StartCoroutine(usedPair.Value.DesactivatePoint());
                        }
                        else
                        {
                            usedPair.Value.ResetFade(pointFadeDuration);
                        }
                    }
                }

                //loop through every detectable entities in the scene
                //test if they are at the current tested distante to the player entity
                //test if there is not already a point at their position
                //if so place a point on their position 
                foreach (DetectableOceanEntity detectable in levelManager.submarineEntitiesInScene)
                {
                    detectableCoords = new Coordinates(detectable.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, detectableCoords.position));

                    if (expand) detectionTestEntity = distance <= waveRange && distance >= waveRange + padding;
                    else detectionTestEntity = distance >= waveRange && distance <= waveRange + padding;

                    if (detectionTestEntity && detectable.currentDetectableState != DetectableState.cantBeDetected)
                    {
                        if (!usedDetectionPoints.ContainsKey(detectable))
                        {
                            if (availableDetectionPoints.Count > 0)
                            {
                                ActivateNewPoint(detectable);
                            }
                            else Debug.Log("Not Enough object in pool");
                        }
                    }
                }

                //Make the wave progress, depend of the progression in the duration
                //Security added to not pass through entities that will be beetween two precise wave position check (padding)
                //First if is when wave goes from center to edge and else is for wave going from edge to center
                if (expand)
                {
                    waveTime += Time.deltaTime;
                    padding = waveRange - (range * (waveTime / waveMaxDuration));
                    waveRange = range * (waveTime / waveMaxDuration);                    
                }
                else
                {
                    waveTime += Time.deltaTime;
                    padding = waveRange - (range * (1 - (waveTime / waveMaxDuration)));
                    waveRange = range * (1 -(waveTime / waveMaxDuration));
                }
                feedback.UpdateWaveProgression(waveTime / waveMaxDuration);
                yield return new WaitForFixedUpdate();
            }

            readyToUse = true;
        }

        public void ActivateNewPoint(DetectableOceanEntity entity)
        {
            GameManager.Instance.ExternalStartCoroutine(availableDetectionPoints[0].ActivatePoint(entity, pointFadeDuration, this));
            usedDetectionPoints.Add(entity, availableDetectionPoints[0]);
            availableDetectionPoints.RemoveAt(0);
        }

        public void DesactivatePoint(HullSonarDetectionPoint point, DetectableOceanEntity entity)
        {
            levelManager.activatedDetectionObjects.Remove(point);
            availableDetectionPoints.Add(point);
            usedDetectionPoints.Remove(entity);
        }
    }
}

