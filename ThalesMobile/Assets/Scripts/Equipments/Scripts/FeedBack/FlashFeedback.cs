﻿using OceanEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PlayerEquipement
{
    public class FlashFeedback : EquipementFeedback
    {
        PlayerOceanEntity sourceEntity;

        [SerializeField]
        Vector3 originalPosOffset;
        Vector3 originPos;

        [SerializeField]
        LineRenderer flashLineRender;
        [SerializeField]
        MeshRenderer meshRenderer;
        [SerializeField]
        ParticleSystem splashParticles;

        [Header("Sound - Flash Effect")]
        [SerializeField]
        AudioMixerGroup targetGroup;
        [SerializeField]
        AudioSource flashSoundSource;
        [SerializeField]
        AudioClip flashSound;
        

        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);
            sourceEntity = _source.currentUser;;
            originPos = sourceEntity.enitityFeedback.transform.position + originalPosOffset;

            ResetPos();
        }

        public void ResetPos()
        {
            meshRenderer.transform.position = originPos;
            flashLineRender.SetPosition(0, gameObject.transform.position);
            flashLineRender.SetPosition(1, gameObject.transform.position);
            meshRenderer.enabled = false;
        }

        public void DropFlash(float duration, Vector3 targetPos)
        {
            meshRenderer.enabled = true;
            StartCoroutine(DropCoroutine(duration, targetPos));
        }

        public IEnumerator DropCoroutine(float duration, Vector3 targetPos)
        {

            float timer = 0;
            Vector3 start = meshRenderer.transform.position;

            while(timer < duration)
            {
                meshRenderer.transform.position = Vector3.Lerp(start, targetPos, timer/duration);
                flashLineRender.SetPosition(0, start);
                flashLineRender.SetPosition(1, meshRenderer.transform.position);
                yield return new WaitForFixedUpdate();
                timer += Time.deltaTime;
            }

            GameManager.Instance.soundHandler.PlaySound(flashSound, flashSoundSource, targetGroup);
            splashParticles.Play();

            ResetPos();
        }



    }
}
