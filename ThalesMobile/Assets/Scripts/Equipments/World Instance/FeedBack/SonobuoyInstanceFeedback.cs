using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine.Audio;
using Tweek.FlagAttributes;

namespace PlayerEquipement
{    
    [TweekClass]
    public class SonobuoyInstanceFeedback : MonoBehaviour
    {
        public enum AnimActionType { Add, Remove }

        [Serializable]
        public struct RevealIcon
        {
            public RectTransform rectT;
            public Image imageComp;

            [HideInInspector]
            public Vector2 animStartPos;
            [HideInInspector]
            public Vector2 animEndPos;

            public void Init(float initXPos)
            {
                rectT.anchoredPosition = new Vector2(initXPos, rectT.anchoredPosition.y);
                rectT.localScale = Vector3.zero;
                imageComp.preserveAspect = true;
            }

            public void SetAnimStartPos(Vector2 v2) { animStartPos = v2; }
            public void SetAnimEndPos(Vector2 v2) { animEndPos = v2; }
        }        

        public struct AnimInfos
        {
            public AnimActionType animType;
            public Sprite displaySprite;
            public RevealIcon targetIcon;

            public AnimInfos(AnimActionType _animType, Sprite _targetEntitySprite, RevealIcon _targetIcon)
            {
                animType = _animType;
                displaySprite = _targetEntitySprite;
                targetIcon = _targetIcon;
            }
        }

        float scaleFactor
        {
            get
            {
                return (source.detectionRange * 2) / rangeRenderer.bounds.size.x;
            }
        }
        bool revelationOn
        {
            get
            {
                return source.inMadRange;
            }
        }

        [Header("References")]

        [SerializeField]
        SonobuoyInstance source;        

        [Header("Range Params")]

        [SerializeField]
        MeshRenderer rangeRenderer;
        [SerializeField]
        Material emptyRangeMaterial;
        [SerializeField]
        Material detectionRangeMaterial;

        [Header("Reveal Param")]
        [SerializeField]
        List<RevealIcon> allIcons;
        [SerializeField]
        float iconGap;
        [SerializeField]
        float animationsDuration;
        bool onGoingAnim;

        Dictionary<Type, RevealIcon> activeIcons;
        Dictionary<Type, AnimInfos> queue;

        [Header("Sound - Detection")]
        [SerializeField]
        AudioMixerGroup targetGroup;
        [SerializeField]
        AudioSource detectionAudioSource;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        AudioClip detectionSound;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        float detectionSoundVolume;
        [SerializeField]
        AudioSource dropSoundSource;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        AudioClip dropSound;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        float dropSoundVolume;
        [SerializeField]
        AudioSource backgroundSoundSource;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        AudioClip backgroundSound;
        [SerializeField, TweekFlag(FieldUsage.Sound)]
        float backgroundSoundVolume;

        public void Init()
        {
            rangeRenderer.transform.localScale = Vector3.one;
            rangeRenderer.transform.localScale *= scaleFactor;

            for (int i = 0; i < allIcons.Count; i++)
            {
                allIcons[i].Init(iconGap * i * 2 + iconGap);
            }

            activeIcons = new Dictionary<Type, RevealIcon>();
            queue = new Dictionary<Type, AnimInfos>();
        }

        public IEnumerator ResetReveal()
        {
            StopAllCoroutines();
            foreach(KeyValuePair<Type, RevealIcon> pair in activeIcons)
            {
                StartCoroutine(ScaleIcon(pair.Value, animationsDuration, Vector3.zero));
            }
            activeIcons = new Dictionary<Type, RevealIcon>();
            queue = new Dictionary<Type, AnimInfos>();

            yield return new WaitForSeconds(animationsDuration);

            for (int i = 0; i < allIcons.Count; i++)
            {
                allIcons[i].Init(iconGap * i * 2 + iconGap);
            }
        }

        public void OnEnable()
        {
            backgroundSoundSource.loop = true;
            backgroundSoundSource.volume = Mathf.Clamp01(backgroundSoundVolume);
            GameManager.Instance.soundHandler.PlaySound(backgroundSound, backgroundSoundSource, targetGroup);
            dropSoundSource.volume = Mathf.Clamp01(dropSoundVolume);
            GameManager.Instance.soundHandler.PlaySound(dropSound, dropSoundSource, targetGroup);
        }

        private void Update()
        {
            if(!onGoingAnim && queue.Count > 0 && revelationOn)
            {
                onGoingAnim = true;
                switch (queue.ElementAt(0).Value.animType)
                {
                    case AnimActionType.Add:
                        AddIcon(queue.ElementAt(0).Value.displaySprite, queue.ElementAt(0).Key);
                        break;
                    case AnimActionType.Remove:
                        RemoveIcon(queue.ElementAt(0).Value.targetIcon, queue.ElementAt(0).Key);
                        break;
                }
                queue.Remove(queue.ElementAt(0).Key);
            }
            else if(!revelationOn && activeIcons.Count > 0)
            {
                StartCoroutine(ResetReveal());
            }
        }

        public void DetectionRangeFeedBack(bool detection)
        {
            if (detection)
            {
                rangeRenderer.material = detectionRangeMaterial;
                detectionAudioSource.volume = Mathf.Clamp01(detectionSoundVolume);
                GameManager.Instance.soundHandler.PlaySound(detectionSound, detectionAudioSource, targetGroup);
            }
            else rangeRenderer.material = emptyRangeMaterial;
        }

        //public void UpdateReveal(bool state)
        //{
        //    //print("Update Reveal");
            
        //    if(state) revelationOn = state;

        //    foreach (KeyValuePair<Type, RevealIcon> pair in activeIcons)
        //    {
        //        Debug.Log("Update");
        //        if(state) StartCoroutine(ScaleIcon(pair.Value, animationsDuration, Vector3.one));
        //        else StartCoroutine(ScaleIcon(pair.Value, animationsDuration, Vector3.zero));
        //    }

        //    if (!state) revelationOn = state;

        //    for (int i = 0; i < allIcons.Length; i++)
        //    {
        //        if (state && allIcons[i].isActive)
        //        {
        //            revelationOn = state;
        //            StartCoroutine(ScaleIcon(allIcons[i], animationsDuration, Vector3.one));
        //        }                    
        //        else if (!state && allIcons[i].isActive)
        //        {
        //            StartCoroutine(ScaleIcon(allIcons[i], animationsDuration, Vector3.zero));                    
        //        }                    
        //    }
        //    revelationOn = state;
        //}

        public void UpdateIcons(List<DetectableOceanEntity> alldetectedEntities)
        {
            Type tempType = null;
            List<Type> tempAllType = new List<Type>();

            foreach (DetectableOceanEntity entity in alldetectedEntities)
            {
                tempType = entity.GetType();
                tempAllType.Add(tempType);

                if (!activeIcons.ContainsKey(tempType))
                    if (!queue.ContainsKey(tempType)) 
                        queue.Add(tempType, new AnimInfos(AnimActionType.Add, entity.detectFeedback.sonobuoyRevealIcon, new RevealIcon()));
                else
                    if (queue.ContainsKey(tempType))
                        if (queue[tempType].animType == AnimActionType.Remove) queue.Remove(tempType);
            }

            for (int i = 0; i < activeIcons.Count; i++)
            {
                if (!tempAllType.Contains(activeIcons.ElementAt(i).Key))
                    if (!queue.ContainsKey(activeIcons.ElementAt(i).Key))
                        queue.Add(activeIcons.ElementAt(i).Key, new AnimInfos(AnimActionType.Remove, null, activeIcons.ElementAt(i).Value));
            }
        }

        public void AddIcon(Sprite entityDisplay, Type entityType)
        {
            for (int i = 0; i < allIcons.Count; i++)
            {
                if (!activeIcons.ContainsValue(allIcons[i]))
                {
                    activeIcons.Add(entityType, allIcons[i]);
                    if(entityDisplay != null) allIcons[i].imageComp.sprite = entityDisplay;
                    if(revelationOn) StartCoroutine(ScaleIcon(allIcons[i], animationsDuration, Vector3.one));
                    break;
                }
            }

            for (int i = 0; i < allIcons.Count; i++)
            {
                StartCoroutine(MoveIcon(false, allIcons[i], animationsDuration, iconGap));
            }
        }

        public void RemoveIcon(RevealIcon target, Type targetEntity)
        {
            activeIcons.Remove(targetEntity);
            if(revelationOn) StartCoroutine(ScaleIcon(target, animationsDuration, Vector3.zero, true));

            for (int i = 0; i < allIcons.Count; i++)
            {
                if (!allIcons[i].Equals(target))
                {
                    if (allIcons[i].rectT.anchoredPosition.x > target.rectT.anchoredPosition.x) StartCoroutine(MoveIcon(false, allIcons[i], animationsDuration, iconGap));
                    else StartCoroutine(MoveIcon(true, allIcons[i], animationsDuration, iconGap));
                }
            }
        }

        IEnumerator MoveIcon(bool direction, RevealIcon icon, float duration, float distance)
        {
            float timer = 0;
            int directionFactor = direction ? 1 : -1;

            Vector2 startPos = icon.rectT.anchoredPosition;
            Vector2 endPos = new Vector2(icon.rectT.anchoredPosition.x + (distance * directionFactor), icon.rectT.anchoredPosition.y);

            while (timer < duration)
            {
                icon.rectT.anchoredPosition = Vector2.Lerp(startPos, endPos, timer / duration);
                yield return new WaitForFixedUpdate();
                timer += Time.deltaTime;
            }

            icon.rectT.anchoredPosition = endPos;

            if (onGoingAnim) onGoingAnim = false;
        }

        IEnumerator ScaleIcon(RevealIcon icon, float duration, Vector3 scaleTarget, bool resetPos = false)
        {
            float timer = 0;
            Vector3 startScale = icon.rectT.localScale;

            while (timer < duration)
            {
                icon.rectT.localScale = Vector3.Lerp(startScale, scaleTarget, timer / duration);
                yield return new WaitForFixedUpdate();
                //Debug.Log(timer.ToString() + " " + icon.rectT.localScale.ToString());
                timer += Time.deltaTime;
            }

            icon.rectT.localScale = scaleTarget;

            if (resetPos)
            {
                Vector2 lastPos = Vector2.zero;

                for (int i = 0; i < allIcons.Count; i++)
                {
                    if (!allIcons[i].Equals(icon) && allIcons[i].rectT.anchoredPosition.x > lastPos.x)
                    {
                        lastPos = allIcons[i].rectT.anchoredPosition;
                    }
                }

                lastPos.x += iconGap * 2;
                icon.rectT.anchoredPosition = lastPos;

                allIcons.Remove(icon);
                allIcons.Add(icon);              
            }
        }
    }  
}

