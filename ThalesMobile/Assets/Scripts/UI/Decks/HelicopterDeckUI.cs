using OceanEntities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterDeckUI : MonoBehaviour
{

    public Button launchButton;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI percentageText;
    public Image fillBar;
    public Image iconFill;
    UIHandler handler;
    LevelManager levelManager;

    private Helicopter linkedHelicopter;

    // Start is called before the first frame update
    void Start()
    {
        handler = GameManager.Instance.uiHandler;
        levelManager = GameManager.Instance.levelManager;

        for (int i = 0; i < levelManager.playerOceanEntities.Count; i++)
        {
            if(levelManager.playerOceanEntities[i].GetType() ==  typeof(Helicopter))
            {
                linkedHelicopter = (Helicopter)levelManager.playerOceanEntities[i];
            }
        }

        linkedHelicopter.launchButton = launchButton;
        linkedHelicopter.deckUI = this;
        launchButton.onClick.AddListener(Launch);
    }

    private void Launch()
    {
            if (!linkedHelicopter.inFlight && !linkedHelicopter.inAlert && !linkedHelicopter.operating)
            {
                linkedHelicopter.LaunchButton();
            }
            else if(linkedHelicopter.inAlert)
            {
                linkedHelicopter.launch = true;
            }
            else if(linkedHelicopter.inFlight)
            {
                //use flash ?
            }
        
    }

    public void UpdateStatusText(string message)
    {
        statusText.text = " " + message;
    }

    public IEnumerator FillBar(float time, int direction)
    {
        if(direction > 0)
        {
            fillBar.fillAmount = 0;
            while(fillBar.fillAmount < 1)
            {
                percentageText.text = ((int)(fillBar.fillAmount * 100)).ToString() + " %";
                fillBar.fillAmount += Time.deltaTime / time;
                yield return null;
            }
        }
        else
        {
            fillBar.fillAmount = 1;
            while (fillBar.fillAmount > 0)
            {
                percentageText.text = ((int)(fillBar.fillAmount * 100)).ToString() + " %";
                fillBar.fillAmount -= Time.deltaTime / time;
                yield return null;
            }
        }
    }

    public IEnumerator FillIcon(float time, int direction)
    {
        if (direction > 0)
        {
            iconFill.fillAmount = 0;
            while (iconFill.fillAmount < 1)
            {
                iconFill.fillAmount += Time.deltaTime / time;
                yield return null;
            }
        }
        else
        {
            iconFill.fillAmount = 1;
            while (iconFill.fillAmount > 0)
            {
                iconFill.fillAmount -= Time.deltaTime / time;
                yield return null;
            }
        }
    }

}
