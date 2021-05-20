using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[ExecuteAlways]
public class CodexCompressor : MonoBehaviour
{

    [Range(0, 1)]
    public float range = 0 ;
    public Scrollbar scrollbar;

    public float minSize = 500;
    public float buttonSize = 85;

    [Range(0,1)]
    public List<float> buttonDispLvl;

    [ReorderableList]
    public List<GameObject> button;

    [ContextMenu("update")]
    private void Update()
    {
        if(scrollbar!= null)
        {
            range = 1 - scrollbar.value;
        }

        //FillButton Amount
        if (buttonDispLvl.Count < button.Count)
        {
            for (int i = buttonDispLvl.Count; i < button.Count; i++)
            {
                    buttonDispLvl.Add(0);
            }
        }
        else
        if (buttonDispLvl.Count > button.Count)
        {
            buttonDispLvl.RemoveRange(button.Count, buttonDispLvl.Count);
        }

        for (int i = 1; i < buttonDispLvl.Count; i++)
        {
            buttonDispLvl[i] = buttonDispLvl[i] <= buttonDispLvl[i - 1] ? buttonDispLvl[i - 1]: buttonDispLvl[i];
        }

        /*
        float piece = 1f / button.Count;
        for (int i = 0; i < button.Count; i++)
        {
            if (range > animButton.Evaluate(i * piece))
            {
                button[i].SetActive(1f - (piece * (i + 1)) < range);
            }
        }
        */

        //activate & desactivate
        for (int i = 0; i < buttonDispLvl.Count; i++)
        {
            button[i].SetActive(range < buttonDispLvl[i]);
        }

        if (transform.childCount != 0)
        {
            int activeNumber = 0;

            for (int i = 0; i < transform.childCount; i++)
            {
                activeNumber += transform.GetChild(i).gameObject.activeInHierarchy ? 1 : 0;
            }

            RectTransform rect = transform.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, minSize + buttonSize * activeNumber);
        }

    }
}
