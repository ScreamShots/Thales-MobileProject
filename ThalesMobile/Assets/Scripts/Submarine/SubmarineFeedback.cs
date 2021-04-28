using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineFeedback : MonoBehaviour
{
    public Submarine submarineScript;

    private int pointsToHack;
    private int pointsHacked;
    private VigilanceState submarineVigilanceState;

    private SubmarineUIManager submarineUI;

    void Start()
    {
        submarineUI = GameManager.Instance.uiHandler.submarineUI;
    }

    void Update()
    {
        GetCurrentValues();

        UpdateVigilanceState();
        SetSubmarineVictoryCondition();
        UpdateSubmarineProgression();
    }

    private void GetCurrentValues()
    {
        pointsToHack = submarineScript.pointsToHack;
        pointsHacked = submarineScript.pointsHacked;
        submarineVigilanceState = submarineScript.currentState;
    }

    private void UpdateVigilanceState()
    {
        switch (submarineVigilanceState)
        {
            case VigilanceState.calm:
                submarineUI.submarineStatusImage.material = submarineUI.submarineCalmStatusMaterial;
                break;
            case VigilanceState.worried:
                submarineUI.submarineStatusImage.material = submarineUI.submarineAlertStatusMaterial;
                break;
            case VigilanceState.panicked:
                submarineUI.submarineStatusImage.material = submarineUI.submarinePanicStatusMaterial;
                break;
        }
    }

    private void SetSubmarineVictoryCondition()
    {
        switch (pointsToHack)
        {
            case 0:
                submarineUI.fillBarBlocked.fillAmount = 1f;
                break;
            case 1:
                submarineUI.fillBarBlocked.fillAmount = 0.8f;
                break;
            case 2:
                submarineUI.fillBarBlocked.fillAmount = 0.65f;
                break;
            case 3:
                submarineUI.fillBarBlocked.fillAmount = 0.5f;
                break;
            case 4:
                submarineUI.fillBarBlocked.fillAmount = 0.35f;
                break;
            case 5:
                submarineUI.fillBarBlocked.fillAmount = 0.23f;
                break;
            case 6:
                submarineUI.fillBarBlocked.fillAmount = 0f;
                break;
        }
    }

    private void UpdateSubmarineProgression()
    {
        switch (pointsHacked)
        {
            case 0:
                submarineUI.fillBar.fillAmount = 0f;
                break;
            case 1:
                submarineUI.fillBar.fillAmount = 0.23f;
                break;
            case 2:
                submarineUI.fillBar.fillAmount = 0.35f;
                break;
            case 3:
                submarineUI.fillBar.fillAmount = 0.5f;
                break;
            case 4:
                submarineUI.fillBar.fillAmount = 0.65f;
                break;
            case 5:
                submarineUI.fillBar.fillAmount = 0.8f;
                break;
            case 6:
                submarineUI.fillBar.fillAmount = 1f;
                break;
        }
    }
}
