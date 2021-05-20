using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class TestSonoBuoyInstance : MonoBehaviour
{
    [Serializable]
    public struct IconPos
    {        
        public GameObject holder;
        public bool isActive;

        [HideInInspector]
        public RectTransform rectT;
        [HideInInspector]
        public Image imageComp;

        [HideInInspector]
        public Vector2 animStartPos;
        [HideInInspector]
        public Vector2 animEndPos;

        public void Init(float initXPos)
        {
            rectT = holder.GetComponent<RectTransform>();
            rectT.anchoredPosition = new Vector2(initXPos, rectT.anchoredPosition.y);

            imageComp = holder.GetComponent<Image>();
            imageComp.preserveAspect = true;
        }

        public void SetAnimStartPos(Vector2 v2)
        {
            animStartPos = v2;
        }

        public void SetAnimEndPos(Vector2 v2)
        {
            animEndPos = v2;
        }
    }

    public IconPos[] emptyPos;
    public float iconGap;

    private void Start()
    {
        for (int i = 0; i < emptyPos.Length; i++)
        {
            emptyPos[i].Init(iconGap * i * 2 + iconGap);
        }
    }

    [ContextMenu("Add")]
    public void AddIcon()
    {
        for (int i = 0; i < emptyPos.Length; i++)
        {
            if (!emptyPos[i].isActive)
            {
                emptyPos[i].isActive = true;
                StartCoroutine(ScaleIcon(emptyPos[i], 1f, Vector3.one));
                break;
            }
        }

        for (int i = 0; i < emptyPos.Length; i++)
        {
            StartCoroutine(MoveIcon(false, emptyPos[i], 1, 0.35f));
        }
    }

    [ContextMenu("Remove")]
    public void RemoveIcon()
    {
        IconPos target = emptyPos[UnityEngine.Random.Range(0,emptyPos.Length - 1)];

        StartCoroutine(ScaleIcon(target, 1, Vector3.zero, true));

        for (int i = 0; i < emptyPos.Length; i++)
        {
            if(!emptyPos[i].Equals(target))
            {
                if (emptyPos[i].rectT.anchoredPosition.x > target.rectT.anchoredPosition.x) StartCoroutine(MoveIcon(false, emptyPos[i], 1, 0.35f));
                else StartCoroutine(MoveIcon(true, emptyPos[i], 1, 0.35f));
            }
        }
    }

    IEnumerator MoveIcon(bool direction, IconPos icon, float duration, float distance)
    {
        float timer = 0;
        int directionFactor = direction ? 1 : -1;

        Vector2 startPos = icon.rectT.anchoredPosition;
        Vector2 endPos = new Vector2(icon.rectT.anchoredPosition.x + (distance * directionFactor), icon.rectT.anchoredPosition.y);

        while(timer < duration)
        {
            icon.rectT.anchoredPosition = Vector2.Lerp(startPos, endPos, timer / duration);
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }

        icon.rectT.anchoredPosition = endPos;
    }

    IEnumerator ScaleIcon(IconPos icon, float duration, Vector3 scaleTarget, bool resetPos = false)
    {
        float timer = 0;
        Vector3 startScale = icon.rectT.localScale;

        while(timer < duration)
        {
            icon.rectT.localScale = Vector3.Lerp(startScale, scaleTarget, timer / duration);
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;            
        }

        icon.rectT.localScale = scaleTarget;

        if (resetPos)
        {
            Vector2 lastPos = Vector2.zero;

            for (int i = 0; i < emptyPos.Length; i++)
            {
                if (!emptyPos[i].Equals(icon) && emptyPos[i].rectT.anchoredPosition.x > lastPos.x)
                {
                    lastPos = emptyPos[i].rectT.anchoredPosition;
                }                
            }

            lastPos.x += iconGap*2;
            icon.rectT.anchoredPosition = lastPos;
        }    
    }
}
