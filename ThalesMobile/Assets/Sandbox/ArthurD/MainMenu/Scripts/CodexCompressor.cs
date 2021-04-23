using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[ExecuteInEditMode]
public class CodexCompressor : MonoBehaviour
{
    [ReorderableList]
    public List<GameObject> button;

    [Range(0, 1)]
    public float range = 0 ;

    [Range(0,1)]
    public List<float> buttonDispLvl;

    [ContextMenu("update")]
    private void Update()
    {

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


        for (int i = 0; i < buttonDispLvl.Count; i++)
        {
            if (range > buttonDispLvl[i])
            {
                button[i].SetActive(false);
            }
            else
            {
                button[i].SetActive(true);
            }

        }
    }
}
