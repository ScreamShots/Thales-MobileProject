using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class PlaneFeedback : MonoBehaviour
    {
        [Header("Ship")]
        public Plane renderedPlane;

        [Header("Particles")]
        public ParticleSystem trailParticles;

        [Header("UI")]
        public GameObject selectionCircle;

        // Update is called once per frame
        void Update()
        {
            if (renderedPlane.currentTargetPoint != null)
            {
                RotateModel(renderedPlane);
                trailParticles.gameObject.SetActive(true);
            }
            else
            {
                trailParticles.gameObject.SetActive(false);
            }

            /*
            if(GameManger.Instance.PlayerController.currentSelectedEntity == renderedPlane)
            {
                selectionCircle.SetActive(true);
            }
            else
            {
                selectionCircle.SetActive(false);
            }     
            */
        }

        public void RotateModel(Plane plane)
        {
            transform.forward = plane.coords.direction;
        }
    }

}
