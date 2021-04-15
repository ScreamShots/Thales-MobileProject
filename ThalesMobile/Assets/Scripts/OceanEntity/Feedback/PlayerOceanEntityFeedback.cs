using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class PlayerOceanEntityFeedback : MonoBehaviour
    {
        [Header("Entity")]
        public PlayerOceanEntity renderedEntity;

        [Header("UI")]
        public GameObject selectionCircle;
        public LineRenderer lineRenderer;
        public GameObject targetPoint;

        [HideInInspector]public InputManager inputManager;

        public void Start()
        {
            inputManager = GameManager.Instance.inputManager;
        }

        public void Update()
        {
            if (GameManager.Instance.playerController.currentSelectedEntity == renderedEntity)
            {
                selectionCircle.SetActive(true);
            }
            else
            {
                selectionCircle.SetActive(false);
            }


            if (renderedEntity.currentTargetPoint != renderedEntity.nullVector || inputManager.getEntityTarget)
            {
                if (!targetPoint.activeSelf && GameManager.Instance.playerController.currentSelectedEntity == renderedEntity)
                    targetPoint.SetActive(true);



                if (renderedEntity.currentTargetPoint != renderedEntity.nullVector)
                    targetPoint.transform.position = Coordinates.ConvertVector2ToWorld(renderedEntity.currentTargetPoint);
                else
                {
                        targetPoint.transform.position = Coordinates.ConvertVector2ToWorld(inputManager.touchedSeaPosition);
                }

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, targetPoint.transform.position);
            }
            else
            {
                if (targetPoint.activeSelf)
                    targetPoint.SetActive(false);
            }
        }
    }
}
