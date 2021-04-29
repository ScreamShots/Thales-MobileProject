using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityDeckUI : MonoBehaviour
{
    //Manager
    [HideInInspector]public UIHandler handler;
    private List<GameObject> decks = new List<GameObject>();
    [HideInInspector] public bool descriptionOpened; 

    [Header("UI Elements")]
    public GameObject currentDeck;
    public GameObject container;

    public GameObject descriptionContainer;
    public TextMeshProUGUI descriptionHeaderText;
    public TextMeshProUGUI descriptionText;

    public CanvasGroup deckCanvasGroup;
    public CanvasGroup descriptionCanvasGroup;
    public CanvasGroup entitiesSelectionCanvasGroup;
    public CanvasGroup submarineCanvasGroup;


    [Header("Animation")]
    public TweeningAnimator descriptionContainerAnim;
    public TweeningAnimator descriptionPanelAnim;

    public TweeningAnimator deckAnimationAppear;
    public TweeningAnimator deckAnimationDisappear;

    public void Initialize()
    {
        for (int i = 0; i < handler.entities.Count; i++)
        {
            GameObject temp = Instantiate(handler.entities[i].entityDeck, container.transform);
            temp.SetActive(false);
            handler.entities[i].entityDeck = temp;
            decks.Add(temp);
            handler.uIElements.Add(temp);
        }

        

        descriptionContainerAnim.GetCanvasGroup();
        descriptionContainerAnim.anim = Instantiate(descriptionContainerAnim.anim);
        descriptionPanelAnim.anim = Instantiate(descriptionPanelAnim.anim);

        deckAnimationAppear.anim = Instantiate(deckAnimationAppear.anim);

        deckAnimationDisappear.anim = Instantiate(deckAnimationDisappear.anim);
    }

    public void UpdateCurrentDeck(GameObject newDeck)
    {
        if(GameManager.Instance.inputManager.currentSelectedCard != null)
            GameManager.Instance.inputManager.currentSelectedCard.abortHandler();
        
        if(currentDeck != null && currentDeck != newDeck)
        {
            deckAnimationDisappear.rectTransform = (RectTransform)currentDeck.transform;
            currentDeck.GetComponent<CanvasGroup>().blocksRaycasts = false;
            deckAnimationDisappear.GetCanvasGroup();
            StartCoroutine(deckAnimationDisappear.anim.Play(deckAnimationDisappear, deckAnimationDisappear.originalPos));
        }
        
        if(currentDeck != newDeck)
        {
            if (!newDeck.activeSelf)
                newDeck.SetActive(true);

            currentDeck = newDeck;
            deckAnimationAppear.rectTransform = (RectTransform)currentDeck.transform;
            deckAnimationAppear.GetCanvasGroup();
            StartCoroutine(deckAnimationAppear.anim.Play(deckAnimationAppear, deckAnimationAppear.originalPos));
            currentDeck.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void CloseDescription()
    {
        StartCoroutine(descriptionContainerAnim.anim.PlayBackward(descriptionContainerAnim, descriptionContainerAnim.originalPos, true));
        StartCoroutine(descriptionPanelAnim.anim.PlayBackward(descriptionPanelAnim, descriptionPanelAnim.originalPos, true));
        entitiesSelectionCanvasGroup.blocksRaycasts = true;
        descriptionCanvasGroup.blocksRaycasts = false;
        deckCanvasGroup.blocksRaycasts = true;
        submarineCanvasGroup.blocksRaycasts = true;
        descriptionOpened = false;
    }

    public void OpenDescription()
    {
        descriptionOpened = true;

        entitiesSelectionCanvasGroup.blocksRaycasts = false;
        descriptionCanvasGroup.blocksRaycasts = true;
        deckCanvasGroup.blocksRaycasts = false;
        submarineCanvasGroup.blocksRaycasts = false;
        StartCoroutine(descriptionContainerAnim.anim.Play(descriptionContainerAnim, descriptionContainerAnim.originalPos));
        StartCoroutine(descriptionPanelAnim.anim.Play(descriptionPanelAnim, descriptionPanelAnim.originalPos));
    }
}
