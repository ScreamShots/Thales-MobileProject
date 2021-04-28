using OceanEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerEquipement
{
    public class FlashFeedback : EquipementFeedback
    {
        PlayerOceanEntity sourceEntity;

        [SerializeField]
        Vector3 originalPosOffset;

        LineRenderer flashLineRender;
        MeshRenderer meshRenderer;

        Vector3 originPos;
        public override void EquipementFeedbackInit(Equipement _source)
        {
            base.EquipementFeedbackInit(_source);
            sourceEntity = _source.currentUser;
            flashLineRender = GetComponent<LineRenderer>();
            meshRenderer = GetComponent<MeshRenderer>();
            originPos = sourceEntity.enitityFeedback.transform.position + originalPosOffset;

            ResetPos();
        }

        public void ResetPos()
        {
            gameObject.transform.position = originPos;
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
            Vector3 start = gameObject.transform.position;

            while(timer < duration)
            {
                gameObject.transform.position = Vector3.Lerp(start, targetPos, timer/duration);
                flashLineRender.SetPosition(0, start);
                flashLineRender.SetPosition(1, gameObject.transform.position);
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }

            //Particle
            //set particle transform to cylinder transfrom
            //play particle

            ResetPos();
        }



    }
}
