using OceanEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterDeckUI : MonoBehaviour
{

    Button launchButton;
    UIHandler handler;
    LevelManager levelManager;

    private Helicopter linkedHelicopter;


    //Events
    public bool isClicked;

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
        launchButton.onClick.AddListener(Launch);
    }

    // Update is called once per frame
    void Update()
    {
        if(isClicked)
        {
            isClicked = false;
        }
    }

    private void Launch()
    {
        isClicked = true;
        linkedHelicopter.LaunchButton();
    }


}
