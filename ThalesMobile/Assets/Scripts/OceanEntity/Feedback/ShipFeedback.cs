using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class ShipFeedback : MonoBehaviour
    {
        [Header("Ship")]
        public Ship renderedShip;

        [Header("Particles")]
        public ParticleSystem trailParticles;

        [Header("UI")]
        public GameObject selectionCircle;

        // Update is called once per frame
        void Update()
        {
            if(renderedShip.currentTargetPoint != null)
            {
                RotateModel(renderedShip);
                trailParticles.gameObject.SetActive(true);
            }
            else
            {
                trailParticles.gameObject.SetActive(false);
            }

            /*
            if(GameManger.Instance.PlayerController.currentSelectedEntity == renderedShip)
            {
                selectionCircle.SetActive(true);
            }
            else
            {
                selectionCircle.SetActive(false);
            }     
            */
        }

        public void RotateModel(Ship ship)
        {
            transform.forward = ship.coords.direction;
        }
    }

}

