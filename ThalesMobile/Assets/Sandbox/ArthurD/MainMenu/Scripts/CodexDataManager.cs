﻿using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CodexDataManager : MonoBehaviour
{
    [Header("Component")]
    public GameObject descriptionCanvas;
    public TextMeshProUGUI prodTitle;
    public MultiTextSystem multiText;

    public TextMeshProUGUI[] codexButtonTitle;

    [Header("Parameter")]
    public CodexData[] loadedMission;

    [ContextMenu("UpdateButton")]
    private void Start()
    {
        if (codexButtonTitle.Length != 0)
        {
            for (int i = 0; i < codexButtonTitle.Length; i++)
            {
                codexButtonTitle[i].text = loadedMission[i].title;
            }
        }
    }

    public void LoadDescription(int number)
    {
        prodTitle.text = loadedMission[number].title;
        multiText.text = loadedMission[number].description;

        descriptionCanvas.SetActive(true);
    }
}
