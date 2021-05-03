using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MultiTextSystem : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI textField;
    [Space(10)]
    public Button leftButton;
    public Button rightButton;
    [Space(10)]
    public Image[] indicator = new Image[3];

    [Header("Parameter")]
    public int showWidow = 0;

    [Header("Texte")]
    [Multiline]
    public string[] text = new string[] { 
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        "Salut à toute est à tous, ici The Fantasio 974",
        "Il y a rien a voir gamin, passe ton chemin"
    };

    void OnEnable()
    {
        ShowWindow(0);
    }

    public void NextWindow()
    {
        if (showWidow < text.Length - 1)
        {
            showWidow++;
        }

        ShowWindow(showWidow);
    }
    public void PreviousWindow()
    {
        if(showWidow > 0)
        {
            showWidow--;
        }
        ShowWindow(showWidow);
    }

    public void ShowWindow(int nbr)
    {
        showWidow = Mathf.Clamp(nbr,0, text.Length - 1);

        if (text != null)
        {
            if (text[showWidow] != null)
            {
                textField.text = text[showWidow];
            }
        }

        for (int i = 0; i < indicator.Length; i++)
        {
            if (indicator[i] != null)
            {
                indicator[i].gameObject.SetActive(i == showWidow);
            }
        }
    }

}
