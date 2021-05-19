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
        public MeshRenderer baseRenderer;
        public Transform mainPropeller;
        public Transform rearPropeller;
        public float spinSpeed;

        [Header("FlashFeedback")]
        public LineRenderer flashLineRenderer;
        public GameObject flashCylinder;

        [Header("Particles")]
        public ParticleSystem sonicDropParticles;
        public ParticleSystem helicopterTakeOffParticles;
        public ParticleSystem helicopterLandingParticles;

        // Update is called once per frame
        new void Update()
        {
            base.Update();
            RotatePropeller();
        }

        private void OnDisable()
        {
            targetPoint.SetActive(false);
            LandFeedback();    
        }

        private void OnEnable()
        {
            targetPoint.SetActive(true);
            TakeOffFeedback();
        }

        public void TakeOffFeedback()
        {
            //Play particles.
            helicopterTakeOffParticles.Play();
        }

        public void LandFeedback()
        {
            helicopterLandingParticles.Play();
        }

        public void DropRearFeedback()
        {
            sonicDropParticles.Play();
        }


        public void RotatePropeller()
        {
            mainPropeller.rotation = Quaternion.Euler(mainPropeller.rotation.eulerAngles.x, mainPropeller.rotation.eulerAngles.y + spinSpeed * Time.deltaTime, mainPropeller.rotation.eulerAngles.z);
            rearPropeller.rotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, Vector3.up);
        }

        public IEnumerator BlinkHelicopter(float time)
        {
            baseRenderer.material.SetInt("isDroppingFlash", 1);
            yield return new WaitForSeconds(time);
            baseRenderer.material.SetInt("isDroppingFlash", 0);
        }
    }

}