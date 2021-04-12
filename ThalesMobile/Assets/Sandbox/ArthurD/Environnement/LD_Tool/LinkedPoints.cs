using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Thales.Tool.LevelDesign
{
    [ExecuteInEditMode]
    public class LinkedPoints : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 lastPos;
        [HideInInspector] 
        public Boundary limit;

        [Header("Linked Points")]
        public List<Transform> linkedPoints;

        [Header("Link Parameter")]
        [SerializeField] float debugSelectionSize = 0.5f;
        [SerializeField] Color debugColor = new Color(1,0,1,0.5f);

        [Space(5)]
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
            pointToLink = null;
            UpdatePosition();
        }

        [Button("Unlink the point")]
        public void UnlinkAPoint()
        {
            LinkedPoints lp;
            if (pointToLink.GetComponent<LinkedPoints>())
            {
                lp = pointToLink.GetComponent<LinkedPoints>();
                lp.linkedPoints.Remove(transform);
            }

            linkedPoints.Remove(pointToLink);
        }

        [Button("BreakAllLink")]
        public void BreakAllLink()
        {
            for (int i = 0; i < linkedPoints.Count; i++)
            {
                linkedPoints[i].GetComponent<LinkedPoints>().linkedPoints.Remove(transform);
                linkedPoints.Remove(linkedPoints[i]);
            }
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

            ClampPos();
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
            //Draw The link
            Gizmos.color = debugColor;
            if (pointToLink != null)
            {
                //Le trait
                Gizmos.DrawLine(transform.position, pointToLink.position);
                //Les deux points
                Gizmos.DrawSphere(pointToLink.position, debugSelectionSize);
                Gizmos.DrawSphere(transform.position, debugSelectionSize);
            }
        }
    }
}
