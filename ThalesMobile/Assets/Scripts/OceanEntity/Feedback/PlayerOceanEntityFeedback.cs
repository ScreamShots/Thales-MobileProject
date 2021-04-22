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
        public GameObject targetArrow;
        private float targetArrowDistance; 


        [HideInInspector]public InputManager inputManager;

        public void Start()
        {
            inputManager = GameManager.Instance.inputManager;
            targetArrowDistance = (transform.position - targetArrow.transform.position).magnitude;
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

                if (GameManager.Instance.playerController.currentSelectedEntity == renderedEntity)
                {
                    targetPoint.SetActive(true);
                    targetArrow.SetActive(false);
                }
                else
                {
                    if(renderedEntity.currentTargetPoint != renderedEntity.nullVector)
                    {
                        targetArrow.SetActive(true);
                        targetPoint.SetActive(false);
                    }
                }

                if (renderedEntity.currentTargetPoint != renderedEntity.nullVector)
                {
                    targetPoint.transform.position = Coordinates.ConvertVector2ToWorld(renderedEntity.currentTargetPoint);
                    if(!inputManager.getEntityTarget)
                    {
                        Vector3 direction = Coordinates.ConvertVector2ToWorld(renderedEntity.currentTargetPoint) - transform.position;
                        direction = new Vector3(direction.x, 0, direction.z);
                        targetArrow.transform.position = transform.position + direction.normalized * targetArrowDistance;
                        targetArrow.transform.forward = -new Vector3(direction.x, 0, direction.z);
                    }
                }
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

                if (targetArrow.activeSelf)
                    targetArrow.SetActive(false);
            }
        }
    }
}
