using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Thales.Tool.LevelDesign
{
    [ExecuteInEditMode]
    public class LinkedPoints : MonoBehaviour
    {
        public Vector3 lastPos;
        private Boundary limit;
        public List<Transform> linkedPoints;
        [Space(20)]
        public Transform pointToLink;
        [Button("Link the point")]
        public void LinkAPoint()
        {
            LinkedPoints lp;
            if (pointToLink.GetComponent<LinkedPoints>())
            {
                lp = pointToLink.GetComponent<LinkedPoints>();
                lp.linkedPoints.Add(transform);
            }
            else
            {
                pointToLink.gameObject.AddComponent(typeof(LinkedPoints));
                lp = pointToLink.GetComponent<LinkedPoints>();
                lp.linkedPoints = new List<Transform>();
                lp.linkedPoints.Add(transform);
            }

            linkedPoints.Add(pointToLink);

            UpdatePosition();
        }

        private void Awake()
        {
            limit = transform.parent.GetComponent<Tool_LevelDesign>().limit;
        }

        private void Update()
        {
            if (transform.position != lastPos)
            {
                lastPos = transform.position;
                UpdatePosition();
            }

            //ClampPos();
        }

        private void UpdatePosition()
        {
            if (linkedPoints.Count > 0)
            {
                for (int i = 0; i < linkedPoints.Count; i++)
                {
                    linkedPoints[i].position = lastPos;
                }
            }
        }

        private void ClampPos()
        {
            transform.position += new Vector3(0, -transform.position.y, 0);

            //Finalement je les point ne sont pas coincé dans la zones
            #region ClampInZone
            /*
            if (transform.position.x < limit.offSet.x + limit.leftBorder)
            {
                transform.position = new Vector3(limit.offSet.x + limit.leftBorder, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > limit.offSet.x + limit.rightBorder)
            {
                transform.position = new Vector3(limit.offSet.x + limit.rightBorder, transform.position.y, transform.position.z);
            }

            if (transform.position.z < limit.offSet.y + limit.downBorder)
            {
                transform.position = new Vector3(limit.offSet.y + transform.position.x, transform.position.y, limit.downBorder);
            }
            else if (transform.position.z > limit.offSet.y + limit.upBorder)
            {
                transform.position = new Vector3(limit.offSet.y + transform.position.x, transform.position.y, limit.upBorder);
            }
            */
            #endregion
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if (pointToLink != null)
            {
                Gizmos.DrawSphere(pointToLink.position, 0.2f);
                Gizmos.DrawLine(transform.position, pointToLink.position);
                Gizmos.DrawSphere(transform.position, 0.2f);
            }
        }
    }
}
