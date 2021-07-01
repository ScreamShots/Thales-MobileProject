using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using OceanEntities;

namespace PlayerEquipement
{
    /// <summary>
    /// Rémi Sécher - 08/04/2021 - Capatas4 Equipement. Launch a wave on commande that reveal position of detectable entity.
    /// </summary>

    [CreateAssetMenu(menuName = "Equipement/Captas 4")]
    public class CaptasFour : Equipement
    {
        [Header("Captas 4 Params")]

        [SerializeField]
        bool partialMode;
        [SerializeField, ShowIf("partialMode")]
        float range;
        [SerializeField, Min(0f)]
        float waveDuration;
        [SerializeField, Min(0f)]
        float pointFadeDuration;

        #region Deprecated
        //[Header("Pooling Params")]

        //[SerializeField]
        //GameObject detectionPointPrefab;
        //[SerializeField, Min(0)]
        //int poolSize;

        
        //[HideInInspector]
        //public List<CaptasFourDetectionPoint> availableDetectionPoints;
        //[HideInInspector]
        //public List<CaptasFourDetectionPoint> usedDetectionPoints;
        #endregion

        LevelManager levelManager;
        CameraController cameraController;
        Vector2[] mapAnglesPos = new Vector2[4];

        public override void Init(PlayerOceanEntity user)
        {
            base.Init(user);
            readyToUse = true;

            levelManager = GameManager.Instance.levelManager;
            cameraController = GameManager.Instance.cameraController;
            equipementType = EquipementType.active;

            //Setting pos of all map's limitation's angles
            mapAnglesPos[0] = new Vector2(cameraController.limit.leftBorder, cameraController.limit.downBorder);
            mapAnglesPos[1] = new Vector2(cameraController.limit.leftBorder, cameraController.limit.upBorder);
            mapAnglesPos[2] = new Vector2(cameraController.limit.rightBorder, cameraController.limit.downBorder);
            mapAnglesPos[3] = new Vector2(cameraController.limit.rightBorder, cameraController.limit.upBorder);

            #region deprecated
            ////Setting up the pooling of detection points
            //availableDetectionPoints = new List<CaptasFourDetectionPoint>();
            //usedDetectionPoints = new List<CaptasFourDetectionPoint>();

            //GameObject tempDetectionPoint;

            //for (int i = 0; i < poolSize; i++)
            //{
            //    tempDetectionPoint = Instantiate(detectionPointPrefab, GameManager.Instance.levelManager.transform);
            //    availableDetectionPoints.Add(tempDetectionPoint.GetComponent<CaptasFourDetectionPoint>());
            //}
            #endregion
        }

        public override void UseEquipement(PlayerOceanEntity user)
        {
            base.UseEquipement(user);
            GameManager.Instance.ExternalStartCoroutine(SonarWave(user.coords));
            readyToUse = false;
        }

        IEnumerator SonarWave(Coordinates userCoords)
        {
            Captas4Feedback waveFeedback = feedbackBehavior as Captas4Feedback;

            Coordinates detectableCoords;
            float distance;
            float waveRange = 0;
            float waveTime = 0;
            float padding = 0;

            //If Captas4 is set on global mode, deffine range of Captas4 to the distance with the farthest map angle
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

            waveFeedback.StartWave(range, waveDuration);

            while (waveTime < waveDuration)
            {
                //looping through all detectable on the map
                foreach (DetectableOceanEntity detectable in levelManager.submarineEntitiesInScene)
                {
                    detectableCoords = new Coordinates(detectable.transform.position, Vector2.zero, 0f);
                    distance = Mathf.Abs(Vector2.Distance(userCoords.position, detectableCoords.position));

                    //if current tested detectable is in range place a point on his pos
                    //there is a security (padding) if object pos is at a position beetween two tested distance increasing at each frame (depend on the progression of the timer)
                    if (distance <= waveRange && distance >= waveRange + padding)
                    {
                        #region deprecated
                        //if (availableDetectionPoints.Count > 0)
                        //{
                        //    availableDetectionPoints[0].ActivatePoint(detectable, pointFadeDuration, this);
                        //    usedDetectionPoints.Add(availableDetectionPoints[0]);
                        //    availableDetectionPoints.RemoveAt(0);
                        //}
                        //else Debug.Log("Not Enough object in pool");
                        #endregion

                        if (detectable.linkedGlobalDetectionPoint.activated) detectable.linkedGlobalDetectionPoint.UpdatePoint();
                        else detectable.linkedGlobalDetectionPoint.InitPoint();
                    }
                }

                waveTime += Time.deltaTime;
                padding = waveRange - (range * (waveTime / waveDuration));
                waveRange = range * (waveTime / waveDuration);
                yield return null;
            }

            readyToUse = true;
        }
    }
}
