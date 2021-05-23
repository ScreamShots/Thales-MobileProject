using OceanEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Tweek.FlagAttributes;

namespace PlayerEquipement
{
    [TweekClass]
    public class FlashFeedback : EquipementFeedback
    {
        PlayerOceanEntity sourceEntity;

        [Header("Drop")]
        [SerializeField]
        Vector3 originalPosOffset;
        Vector3 originPos;

        [SerializeField]
        LineRenderer flashLineRender;
        [SerializeField]
        MeshRenderer meshRenderer;
        [SerializeField]
        ParticleSystem splashParticles;

        [Header("Range")]
        Flash sourceFlash;
        [SerializeField]
        GameObject rangeDisplayObject;
        [SerializeField]
        MeshFilter quadHolder;
        [SerializeField]
        Material rangeMaterial;
        [SerializeField]
        ParticleSystem rangeParticles;
        float scaleRatio;

        [Header("Sound - Flash Effect")]
        [SerializeField]
        AudioMixerGroup targetGroup;
        [SerializeField]
        AudioSource flashSoundSource;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        AudioClip flashSound;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        float flashSoundVolume;


        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);

            sourceFlash = _source as Flash;
            sourceEntity = _source.currentUser;;

            originPos = sourceEntity.enitityFeedback.transform.position + originalPosOffset;

            scaleRatio = sourceFlash.extendedRange * (1 / quadHolder.sharedMesh.bounds.size.x);
            rangeDisplayObject.transform.localScale = Vector3.one * scaleRatio;

            rangeMaterial.SetFloat("Vector1_AAC87A69", sourceFlash.winningRange / sourceFlash.extendedRange);

            var main = rangeParticles.main;

            main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constantMin * scaleRatio, main.startSize.constantMax * scaleRatio);

            rangeDisplayObject.SetActive(false);

            ResetPos();
        }

        public void ResetPos()
        {
            meshRenderer.transform.localPosition = originPos;
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
                yield return null;
                timer += Time.deltaTime;
            }

            rangeMaterial.SetFloat("Float_Manual_Value", 0f);
            var shape = rangeParticles.shape;
            shape.radius = 0f;
            rangeDisplayObject.SetActive(true);

            flashSoundSource.volume = Mathf.Clamp(flashSoundVolume, 0, 1);
            GameManager.Instance.soundHandler.PlaySound(flashSound, flashSoundSource, targetGroup);
            splashParticles.Play();

            ResetPos();
        }

        public void UpdateWaveProgression(float globalProgression, float winningProgression, bool end = false)
        {
            if (end)
            {
                rangeDisplayObject.SetActive(false);
                return;
            }

            rangeMaterial.SetFloat("Float_Manual_Value", globalProgression);

            var shape = rangeParticles.shape;
            shape.radius = Mathf.Lerp(0 , 0.5f * (sourceFlash.winningRange/sourceFlash.extendedRange) * scaleRatio, winningProgression);
        }



    }
}
