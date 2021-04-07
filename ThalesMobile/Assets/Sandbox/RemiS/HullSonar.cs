using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PlayerEquipement
{
    [CreateAssetMenu(menuName = "Equipement/HullSonar")]
    public class HullSonar : Equipement
    {
        [Header("Hull Sonar Params")]
        [SerializeField]
        float range;
        [SerializeField]
        float waveDuration;
        [SerializeField]
        float pointFadeDuration;

        [Header("Pooling Params")]
        [SerializeField]
        GameObject detectionPointPrefab;
        [SerializeField]
        int poolSize;
        List<HullSonarDetectionPoint> availableDetectionPoints = new List<HullSonarDetectionPoint>();
        List<HullSonarDetectionPoint> usedDetectionPoints = new List<HullSonarDetectionPoint>();

        LevelManager levelManager;

        public override void Awake()
        {
            base.Awake();

            levelManager = GameManager.Instance.levelManager;

            equipementType = EquipementType.passive;

            GameObject tempDetectionPoint;

            for (int i = 0; i < poolSize; i++)
            {
                tempDetectionPoint = Instantiate(detectionPointPrefab, GameManager.Instance.levelManager.transform);
                availableDetectionPoints.Add(tempDetectionPoint.GetComponent<HullSonarDetectionPoint>());
            }
        }

        public override void UseEquipement(Coordinates userCoords)
        {
            base.UseEquipement(userCoords);

            readyToUse = false;
            GameManager.Instance.ExternalStartCoroutine(SonarWave(userCoords));
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
                foreach (DetectableOceanEntity detectable in levelManager.submarineEntitiesInScene)
                {
                    detectableCoords = new Coordinates(detectable.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, detectableCoords.position));

                    if (distance >= waveRange && distance <= waveRange + padding)
                    {
                        if (availableDetectionPoints.Count > 0)
                        {
                            availableDetectionPoints[0].ActivatePoint(detectable, pointFadeDuration);
                            usedDetectionPoints.Add(availableDetectionPoints[0]);
                            availableDetectionPoints.RemoveAt(0);
                        }
                        else Debug.Log("Not Enough object in pool");
                        
                    }
                }

                foreach (HullSonarDetectionPoint detectionPoint in usedDetectionPoints.ToList())
                {
                    pointCoords = new Coordinates(detectionPoint.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, pointCoords.position));

                    if (distance >= waveRange && distance <= waveRange + padding)
                    {
                        detectionPoint.DesactivatePoint();
                        availableDetectionPoints.Add(detectionPoint);
                        usedDetectionPoints.Remove(detectionPoint);
                    }
                }

                waveTime += Time.deltaTime;
                padding = waveRange - (range * 1 - (waveTime / waveDuration));
                waveRange = range * 1 - (waveTime / waveDuration);

                yield return new WaitForEndOfFrame();
            }

            readyToUse = true;
        }
    }
}

