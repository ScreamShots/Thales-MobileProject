using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PlayerEquipement
{
    [CreateAssetMenu(menuName = "Equipement/Captas 4")]
    public class CaptasFour : Equipement
    {
        [Header("Captas 4 Params")]
        [SerializeField]
        bool partialMode;
        [SerializeField, ShowIf("partialMode")]
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
        [HideInInspector]
        public List<CaptasFourDetectionPoint> availableDetectionPoints = new List<CaptasFourDetectionPoint>();
        [HideInInspector]
        public List<CaptasFourDetectionPoint> usedDetectionPoints = new List<CaptasFourDetectionPoint>();

        LevelManager levelManager;
        CameraController cameraController;
        Vector2[] mapAnglesPos = new Vector2[4];

        public override void Awake()
        {
            base.Awake();

            levelManager = GameManager.Instance.levelManager;
            cameraController = GameManager.Instance.cameraController;
            equipementType = EquipementType.active;

            mapAnglesPos[0] = new Vector2(cameraController.limit.left, cameraController.limit.down);
            mapAnglesPos[1] = new Vector2(cameraController.limit.left, cameraController.limit.up);
            mapAnglesPos[2] = new Vector2(cameraController.limit.right, cameraController.limit.down);
            mapAnglesPos[3] = new Vector2(cameraController.limit.right, cameraController.limit.up);

            GameObject tempDetectionPoint;

            for (int i = 0; i < poolSize; i++)
            {
                tempDetectionPoint = Instantiate(detectionPointPrefab, GameManager.Instance.levelManager.transform);
                availableDetectionPoints.Add(tempDetectionPoint.GetComponent<CaptasFourDetectionPoint>());
            }
        }

        public override void UseEquipement(Coordinates userCoords)
        {
            base.UseEquipement(userCoords);
            GameManager.Instance.ExternalStartCoroutine(SonarWave(userCoords));
            readyToUse = false;
        }

        IEnumerator SonarWave(Coordinates userCoords)
        {
            Coordinates detectableCoords;
            float distance;
            float waveRange = 0;
            float waveTime = 0;
            float padding = 0;            

            if (!partialMode)
            {
                float tempDistance = 0;
                range = 0;

                foreach (Vector2 anglePos in mapAnglesPos)
                {
                    tempDistance = Mathf.Abs(Vector2.Distance(userCoords.position, anglePos));
                    if (tempDistance > range) range = tempDistance;
                }
            }

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
                            availableDetectionPoints[0].ActivatePoint(detectableCoords, pointFadeDuration, this);
                            usedDetectionPoints.Add(availableDetectionPoints[0]);
                            availableDetectionPoints.RemoveAt(0);
                        }
                        else Debug.Log("Not Enough object in pool");
                    }
                }

                waveTime += Time.deltaTime;
                padding = waveRange - (range * (waveTime / waveDuration));
                waveRange = range * (waveTime / waveDuration);

                yield return new WaitForEndOfFrame();
            }

            readyToUse = true;
        }
    }
}
