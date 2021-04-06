using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PlayerEquipement
{
    public class HullSonar : Equipement
    {
        [SerializeField]
        float range;
        [SerializeField]
        float waveDuration;

        [SerializeField]
        GameObject detectionPointPrefab;
        [SerializeField]
        int poolSize;

        List<GameObject> availableDetectionPoints;
        List<GameObject> usedDetectionPoints;

        LevelManager levelHandler;

        private void Awake()
        {
            levelHandler = GameManager.Instance.levelManager;

            equipementType = EquipementType.passive;

            availableDetectionPoints = new List<GameObject>();
            usedDetectionPoints = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                availableDetectionPoints.Add(Instantiate(detectionPointPrefab, levelHandler.transform));
                availableDetectionPoints[i].SetActive(false);
            }
        }

        public override void UseEquipement(Transform userPos)
        {
            base.UseEquipement(userPos);
            readyToUse = false;
            GameManager.Instance.ExternalStartCoroutine(SonarWave(userPos));
        }

        IEnumerator SonarWave(Transform userPos)
        {
            Coordinates userCoords;
            Coordinates detectableCoords;
            Coordinates pointCoords;
            float distance;
            float waveRange = range;
            float waveTime = 0;
            float padding = 0;

            while (waveRange < waveDuration)
            {
                userCoords = new Coordinates(userPos.position, Vector2.zero, 0f);

                foreach (DetectableOceanEntity detectable in levelHandler.submarineEntitiesInScene)
                {
                    detectableCoords = new Coordinates(detectable.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, detectableCoords.position));

                    if (distance >= waveRange && distance <= waveRange + padding)
                    {
                        //Placer un point
                    }
                }

                foreach (GameObject detectionPoint in usedDetectionPoints.ToList())
                {
                    pointCoords = new Coordinates(detectionPoint.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, pointCoords.position));

                    if (distance >= waveRange && distance <= waveRange + padding)
                    {
                        //Retirer un point
                    }
                }

                waveTime += Time.deltaTime;
                padding = waveRange - (range * 1 - (waveTime / waveDuration));
                waveRange = range * 1 - (waveTime / waveDuration);

                yield return new WaitForEndOfFrame();
            }

            readyToUse = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.Instance.ExternalStopCoroutine(SonarWave(null));
        }
    }
}

