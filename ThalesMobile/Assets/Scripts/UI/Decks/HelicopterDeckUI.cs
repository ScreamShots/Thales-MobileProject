using OceanEntities;
using PlayerEquipement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum HelicopterButtonState
{
    Start,
    Launch,
    Return,
    Disable
}
public class HelicopterDeckUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI percentageText;
    public Image fillBar;
    
    [Header("Main Button")]
    public Button launchButton;
    public Image launchButtonImage;
    public Sprite emptySprite;
    public Sprite flashSprite;
    public Sprite flashPressedSprite;
    public Image buttonOverlay;


    [Header("Secondary Button")]
    public Button secondaryButton;
    public Image secondaryButtonImage;
    public Sprite startSprite;
    public Sprite launchSprite;
    public Sprite returnSprite;
    public Sprite unavailableSprite;

    UIHandler handler;
    LevelManager levelManager;

    private Helicopter linkedHelicopter;
    private Flash flashEquipement;
    public Coroutine buttonCoroutine;

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


        launchButton.interactable = false;
        launchButton.onClick.AddListener(DropFlash);

        secondaryButton.onClick.AddListener(Launch);

        flashEquipement = (Flash) linkedHelicopter.activeEquipement;
    }

    private void DropFlash()
    {
        if(linkedHelicopter.inFlight && linkedHelicopter.activeEquipement.readyToUse)
        {
            linkedHelicopter.activeEquipement.UseEquipement(linkedHelicopter);
            StartCoroutine(linkedHelicopter.helicopterFeedback.BlinkHelicopter(flashEquipement.dropDuration));

            //disable Button
            //DeactivateButton();
            buttonCoroutine = StartCoroutine(FlashCoolDown());
        }
    }

    public void Launch()
    {
        if (!linkedHelicopter.inFlight && !linkedHelicopter.inAlert && !linkedHelicopter.operating)
        {
            linkedHelicopter.LaunchButton();

            UpdateSecondaryButton(HelicopterButtonState.Disable);

        }
        else if (linkedHelicopter.inAlert)
        {
            linkedHelicopter.launch = true;
            UpdateSecondaryButton(HelicopterButtonState.Disable);
        }
        else if(linkedHelicopter.inFlight)
        {
            linkedHelicopter.inFlight = false;

            UpdateSecondaryButton(HelicopterButtonState.Disable);
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

            if (percentageText.text == "99 %")
                percentageText.text = "100 %";
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

    public void ActivateButton()
    {
        launchButton.interactable = true;
        launchButtonImage.sprite = flashSprite;
        buttonOverlay.fillAmount = 0;
    }

    public void DeactivateButton(bool fillBar)
    {
        launchButtonImage.sprite = emptySprite;
        launchButton.interactable = false;
        buttonOverlay.fillAmount = 1;

        if (fillBar)
            StartCoroutine(FillOverlay(flashEquipement.dropDuration - 0.1f));
    }

    public void UpdateSecondaryButton(HelicopterButtonState buttonState)
    {
        switch (buttonState)
        {
            case HelicopterButtonState.Start:
                secondaryButton.interactable = true;
                secondaryButtonImage.sprite = startSprite;
                break;

            case HelicopterButtonState.Launch:
                secondaryButton.interactable = true;
                secondaryButtonImage.sprite = launchSprite;
                break;
            case HelicopterButtonState.Return:
                secondaryButton.interactable = true;
                secondaryButtonImage.sprite = returnSprite;
                break;
            case HelicopterButtonState.Disable:
                secondaryButton.interactable = false;
                secondaryButtonImage.sprite = unavailableSprite;
                break;
        }

    } 

    public IEnumerator FlashCoolDown()
    {
        DeactivateButton(true);
        yield return new WaitForSeconds(flashEquipement.dropDuration);
        ActivateButton();
    }

    public IEnumerator FillOverlay(float time)
    {
        buttonOverlay.fillAmount = 1;
        while (buttonOverlay.fillAmount > 0)
        {
            buttonOverlay.fillAmount -= Time.deltaTime / time;
            yield return null;
        }
        buttonOverlay.fillAmount = 0;
    }

}
