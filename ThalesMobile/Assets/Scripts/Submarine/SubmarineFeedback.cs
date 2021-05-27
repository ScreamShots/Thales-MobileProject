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

        if (submarineScript.changeUIDecoy)
        {
            NoiseVigilance();
        }
        else
        {
            UpdateVigilanceState();
        }
        
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
            /*case VigilanceState.worried:
                submarineUI.submarineStatusImage.material = submarineUI.submarineAlertStatusMaterial;
                break;*/
            case VigilanceState.panicked:
                submarineUI.submarineStatusImage.material = submarineUI.submarinePanicStatusMaterial;
                break;
        }
    }

    private void NoiseVigilance()
    {
        submarineUI.submarineStatusImage.material = submarineUI.submarineNoise;
    }
}
