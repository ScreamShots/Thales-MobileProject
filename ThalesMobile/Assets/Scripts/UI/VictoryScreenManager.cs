using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VictoryScreenManager : MonoBehaviour
{

    [Header("UIElements")]
    public TextMeshProUGUI header;
    public Button menuButton;

    public CanvasGroup victoryCanvasGroup;
    public CanvasGroup deckCanvasGroup;
    public CanvasGroup entitiesSelectionCanvasGroup;
    public CanvasGroup submarineCanvasGroup;



    [Header("Descriptions")]
    public string victoryHeader;
    public string defeatHeader;

    [Header("Animation")]
    public TweeningAnimator blur;
    public TweeningAnimator leftCorner;
    public TweeningAnimator rightCorner;

    [Header("Sound")]
    public AudioSource source;
    public AudioMixerGroup targetGroup;
    public AudioClip winSound;
    public AudioClip defeatSound;
    public float winLooseVolume;

    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        blur.GetCanvasGroup();
        leftCorner.anim = Instantiate(leftCorner.anim);
        rightCorner.anim = Instantiate(rightCorner.anim);
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
                source.volume = Mathf.Clamp01(winLooseVolume);
                GameManager.Instance.soundHandler.PlaySound(winSound, source, targetGroup);
                header.text = victoryHeader;
            }
            else
            {
                source.volume = Mathf.Clamp01(winLooseVolume);
                GameManager.Instance.soundHandler.PlaySound(defeatSound, source, targetGroup);
                header.text = defeatHeader;
            }

            StartCoroutine(blur.anim.Play(blur, blur.originalPos));
            StartCoroutine(leftCorner.anim.Play(leftCorner, leftCorner.originalPos));
            StartCoroutine(rightCorner.anim.Play(rightCorner, rightCorner.originalPos));

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
