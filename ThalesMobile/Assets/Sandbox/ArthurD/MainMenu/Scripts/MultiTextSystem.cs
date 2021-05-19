using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MultiTextSystem : MonoBehaviour
{
    [Header("Component")]
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

    [Header("SystemParameter")]
    public bool oldSystem = false;
    public bool newSystem { get { return !oldSystem; } }

    //Old
    [BoxGroup("OldSysteme"), ShowIf("oldSystem")] public TextMeshProUGUI textField;
    //New
    [BoxGroup("NewSysteme"), ShowIf("newSystem")] public Scrollbar scrollBar;
    [Space(10)]
    [BoxGroup("NewSysteme"), ShowIf("newSystem")] public TextMeshProUGUI textFieldA;
    [BoxGroup("NewSysteme"), ShowIf("newSystem")] public TextMeshProUGUI textFieldB;
    [BoxGroup("NewSysteme"), ShowIf("newSystem")] public TextMeshProUGUI textFieldC;
    private bool refocusing = false;

    void OnEnable()
    {
        if (oldSystem)
        {
            ShowWindow(0);
        }
        else
        {
            SlideWindow(0);

            textFieldA.text = text[0];
            textFieldB.text = text[1];
            textFieldC.text = text[2];
        }
    }

    private void Update()
    {
        if (oldSystem)
        {
            UpdateIndicator();
        }
        else
        {
            if (refocusing)
            {
                showWidow = Mathf.RoundToInt(scrollBar.value * (indicator.Length - 1));
            }
            UpdateIndicator();
        }
    }

    public void NextWindow()
    {
        if (showWidow < text.Length - 1)
        {
            showWidow++;
        }

        if (oldSystem)
        {
            ShowWindow(showWidow);
        }
        else
        {
            SlideWindow(showWidow);
        }
    }
    public void PreviousWindow()
    {
        if(showWidow > 0)
        {
            showWidow--;
        }

        if (oldSystem)
        {
            ShowWindow(showWidow);
        }
        else
        {
            SlideWindow(showWidow);
        }
    }
    private void UpdateIndicator()
    {
        for (int i = 0; i < indicator.Length; i++)
        {
            if (indicator[i] != null)
            {
                indicator[i].gameObject.SetActive(i == showWidow);
            }
        }
    }


    //On old System
    public void ShowWindow(int nbr)
    {
        showWidow = Mathf.Clamp(nbr,0, text.Length - 1);

        //Change Text
        if (text != null)
        {
            if (text[showWidow] != null)
            {
                textField.text = text[showWidow];
            }
        }
    }

    //On new System
    public void SlideWindow(int nbr)
    {
        showWidow = Mathf.Clamp(nbr, 0, text.Length - 1);

        //Change ScrollBar value
        float value = (1 / (indicator.Length - 1)) * showWidow;

        StopAllCoroutines();
        StartCoroutine(SlideTo( value, 1f));
    }

    private IEnumerator SlideTo(float value, float speed)
    {
        refocusing = true;

        float time = 0f;
        float distance = value - scrollBar.value;
        float distAbsolute = Mathf.Abs(distance);
        float baseValue = scrollBar.value;

        while (time < distAbsolute)
        {
            time += speed * Time.deltaTime;
            scrollBar.value = Mathf.Lerp(baseValue, baseValue + distance, time);
            yield return null;
        }

        refocusing = false;
        scrollBar.SetValueWithoutNotify(value);
    }
}
