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
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        AudioClip flashSound;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        float flashSoundVolume;


        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);
            sourceEntity = _source.currentUser;;
            originPos = sourceEntity.enitityFeedback.transform.position + originalPosOffset;

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
                yield return new WaitForFixedUpdate();
                timer += Time.deltaTime;
            }

            flashSoundSource.volume = Mathf.Clamp(flashSoundVolume, 0, 1);
            GameManager.Instance.soundHandler.PlaySound(flashSound, flashSoundSource, targetGroup);
            splashParticles.Play();

            ResetPos();
        }



    }
}
