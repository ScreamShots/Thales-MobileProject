﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OceanEntities
{
    public class HelicopterFeedback : PlayerOceanEntityFeedback
    {
        [Header("Helicopter")]
        public Helicopter renderedHelicopter;
        public GameObject rendererContainer;
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

            if(flashCylinder.activeSelf)
            {
                flashLineRenderer.SetPosition(0, transform.position - new Vector3(0, 0, 0.222f));
                flashLineRenderer.SetPosition(1, flashCylinder.transform.position);
            }
        }

        private void OnDisable()
        {
            targetPoint.SetActive(false);
        }

        private void OnEnable()
        {
            targetPoint.SetActive(true);
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

        public void DropRearFeedback()
        {
            sonicDropParticles.Play();
        }


        public void RotatePropeller()
        {
            mainPropeller.rotation = Quaternion.Euler(mainPropeller.rotation.eulerAngles.x, mainPropeller.rotation.eulerAngles.y + spinSpeed * Time.deltaTime, mainPropeller.rotation.eulerAngles.z);
            rearPropeller.rotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, Vector3.up);
        }
    }

}