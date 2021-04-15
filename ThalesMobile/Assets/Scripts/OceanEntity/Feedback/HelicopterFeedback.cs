using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class HelicopterFeedback : PlayerOceanEntityFeedback
    {
        [Header("Helicopter")]
        public Helicopter renderedHelicopter;
        public GameObject rendererContainer;

        [Header("Particles")]
        public ParticleSystem trailParticles;
        public ParticleSystem sonicDropParticles;
        public ParticleSystem helicopterTakeOffParticles;
        public ParticleSystem helicopterLandingParticles;

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }

        public void RotateModel(Helicopter heli)
        {
            transform.forward = heli.coords.direction;
        }

        public void TakeOffFeedback()
        {
            //ActivateRenderer.
            rendererContainer.SetActive(true);
            //Play particles.
            helicopterTakeOffParticles.Play();
        }

        public IEnumerator LandFeedback()
        {
            helicopterLandingParticles.Play();

            yield return new WaitUntil(() => !helicopterLandingParticles.isPlaying);
            rendererContainer.SetActive(false);
        }

        public void DropSonicFeedback()
        {
            sonicDropParticles.Play();
        }

    }

}