using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class HelicopterFeedback : MonoBehaviour
    {
        [Header("Helicopter")]
        public Helicopter renderedHelicopter;

        [Header("Particles")]
        public ParticleSystem trailParticles;

        [Header("UI")]
        public GameObject selectionCircle;

        // Update is called once per frame
        void Update()
        {
            if (renderedHelicopter.currentTargetPoint != null)
            {
                RotateModel(renderedHelicopter);
                trailParticles.gameObject.SetActive(true);
            }
            else
            {
                trailParticles.gameObject.SetActive(false);
            }

            /*
            if(GameManger.Instance.PlayerController.currentSelectedEntity == renderedHelicopter)
            {
                selectionCircle.SetActive(true);
            }
            else
            {
                selectionCircle.SetActive(false);
            }     
            */
        }

        public void RotateModel(Helicopter heli)
        {
            transform.forward = heli.coords.direction;
        }
    }

}