using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenManager : MonoBehaviour
{

    [Header("UIElements")]
    public TextMeshProUGUI header;
    public TextMeshProUGUI description;
    public Button menuButton;

    public CanvasGroup victoryCanvasGroup;
    public CanvasGroup deckCanvasGroup;
    public CanvasGroup entitiesSelectionCanvasGroup;
    public CanvasGroup submarineCanvasGroup;



    [Header("Descriptions")]
    public string victoryHeader;
    public string defeatHeader;
    public string victoryDescription;
    public string defeatDescription;

    [Header("Animation")]
    public TweeningAnimator blur;
    public TweeningAnimator victoryPanel;

    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        blur.GetCanvasGroup();
        victoryPanel.anim = Instantiate(victoryPanel.anim);
        blur.anim = Instantiate(blur.anim);
    }

    public void Victory(bool victory)
    {
        if(!once)
        {
            once = true;
            victoryCanvasGroup.blocksRaycasts = true;
            entitiesSelectionCanvasGroup.blocksRaycasts = false;
            deckCanvasGroup.blocksRaycasts = false;
            submarineCanvasGroup.blocksRaycasts = false;

            if (victory)
            {
                header.text = victoryHeader;
                description.text = victoryDescription;
            }
            else
            {
                header.text = defeatHeader;
                description.text = defeatDescription;
            }

            StartCoroutine(blur.anim.Play(blur, blur.originalPos));
            StartCoroutine(victoryPanel.anim.Play(victoryPanel, victoryPanel.originalPos));

            //GameManager.Instance.pauseHandler.Pause();
        }
    }

    public void ReturnToMenu()
    {
        //Implement button
        menuButton.interactable = false;
        GameManager.Instance.sceneHandler.LoadScene(0);
    }

}
