using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private GameManager gameManager;
    private InputManager inputManager;
    private UIHandler uiHandler;



    [Header("UI")]
    public GameObject opaquePanel;
    [Space]
    public GameObject screenOneTextContainer;
    public TextMeshProUGUI screenOneText;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        inputManager = gameManager.inputManager;
        uiHandler = gameManager.uiHandler;



        StartCoroutine(TutorialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TutorialCoroutine()
    {
        //Initialize elements to be disabled


        //Screen 1 : Tutorial Introduction
        opaquePanel.SetActive(true);
        screenOneTextContainer.SetActive(true);
        screenOneText.text = "Bienvenue dans ce CASEX.";

        yield return new WaitUntil(() => Input.touchCount == 1);
        yield return new WaitUntil(() => Input.touchCount == 0);

        screenOneTextContainer.SetActive(false);
        opaquePanel.SetActive(false);

        //Screen 2 : Camera Movement


    }




}
