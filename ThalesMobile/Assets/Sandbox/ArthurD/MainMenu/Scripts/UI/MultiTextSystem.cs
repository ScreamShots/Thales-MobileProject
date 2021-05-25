using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MultiTextSystem : MonoBehaviour
{
    [Header("Component")]
    public Image[] indicator = new Image[3];

    [Header("Parameter")]
    public int showWidow = 0;

    [Header("Texte")]
    public Scrollbar scrollBar;
    public CodexData data = null;
    [Space(10)]
    public TextMeshProUGUI FieldA_text;
    public Image fieldB_image;
    public TextMeshProUGUI FieldC_text;
    private bool refocusing = false;

    void OnEnable()
    {
        //SlideWindow(0);
    }

    private void LateUpdate()
    {
        /*
        if (!refocusing)
        {
            showWidow = Mathf.RoundToInt(scrollBar.value * (indicator.Length - 1));
        }
        */

        UpdateIndicator();

        if (data.description != null)
        {
            FieldA_text.text = data.description;
        }

        if (data.image != null)
        {
            fieldB_image.color = Color.white;
            fieldB_image.sprite = data.image;
        }
        else
        {
            fieldB_image.sprite = null;
            fieldB_image.color = new Color(0,0,0,0);
        }
        if (data.link != null)
        {
            FieldC_text.text = data.linkName;
        }
    }

    public void NextWindow()
    {
        if (showWidow < indicator.Length - 1)
        {
            showWidow++;
        }

        SlideWindow(showWidow);
    }
    public void PreviousWindow()
    {
        if (showWidow > 0)
        {
            showWidow--;
        }

        SlideWindow(showWidow);
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

    //Don't work
    public void SlideWindow(int nbr)
    {
        showWidow = Mathf.Clamp(nbr, 0, indicator.Length - 1);

        //Change ScrollBar value
        float value = (1 / (indicator.Length - 1)) * showWidow;

        StopAllCoroutines();
        StartCoroutine(SlideTo(value, 1f));
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
            scrollBar.Rebuild(CanvasUpdate.Layout);
            yield return null;
        }

        refocusing = false;
        scrollBar.SetValueWithoutNotify(value);
    }

    public void LoadURL()
    {
        Application.OpenURL(data.link);
    }
}
