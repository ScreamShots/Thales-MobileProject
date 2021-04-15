using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableUI : MonoBehaviour
     , IPointerClickHandler
     , IDragHandler
     , IPointerEnterHandler
     , IPointerExitHandler
     , IDropHandler
     , IEndDragHandler
{
    public static bool pointerFocusedOnCard;
    public static bool anyCardSelected;
    private static List<InteractableUI> allCards = new List<InteractableUI>();
    public static TweeningAnimator darkBackAnim;

    public TweeningAnimator dragAnim;
    public TweeningAnimator holdAnim;
    public TweeningAnimator selectedAnim;
    public bool darkenBackWhileHold;
    public bool canBeSelected;

    [HideInInspector] public bool isDragged;
    [HideInInspector] public bool isDropped;
    [HideInInspector] public bool isHovered;
    [HideInInspector] public bool isCursorOn;
    [HideInInspector] public bool isFocused;
    [HideInInspector] public bool isClicked;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool descriptionOpened;

    private int dropCount;
    private int clickCount;
    private bool dragFlag;
    private bool selectedFlag;
    private float holdTime;
    private bool cursorGoOut;

    public delegate void Abort();
    public Abort abortHandler;

    private void Start()
    {
        dragFlag = true;
        allCards.Add(this);
    }

    private void Update()
    {
        isHovered = isHovered && Input.touchCount > 0;
        isFocused = isHovered || isDragged || isSelected;

        if (dropCount > 0)
        {
            dropCount--;
            isDropped = true;
        }
        else
        {
            isDropped = false;
        }

        if (clickCount > 0)
        {
            clickCount--;
            isClicked = true;
        }
        else
        {
            isClicked = false;
        }

        if (isClicked && canBeSelected)
        {
            if (isSelected)
            {
                Deselect();
            }
            else
            {
                SelectCard(this);
            }
        }

        if (isHovered && !cursorGoOut)
        {
            holdTime += Time.deltaTime;
        }
        if (Input.touchCount == 0)
        {
            holdTime = 0;
        }

        if (isDropped)
        {
            cursorGoOut = false;
        }

        if (holdTime > 0.8f && !descriptionOpened)
        {
            descriptionOpened = true;
            StopAllCoroutines();

            if (darkenBackWhileHold)
                StartCoroutine(darkBackAnim.anim.Play(darkBackAnim, darkBackAnim.originalPos));

            if (holdAnim.rectTransform != null)
                StartCoroutine(holdAnim.anim.Play(holdAnim, holdAnim.originalPos));
        }

        if (holdTime <= 0 && descriptionOpened)
        {
            descriptionOpened = false;

            if (darkenBackWhileHold)
                StartCoroutine(darkBackAnim.anim.PlayBackward(darkBackAnim, darkBackAnim.originalPos, true));

            if (holdAnim.rectTransform != null)
                StartCoroutine(holdAnim.anim.PlayBackward(holdAnim, holdAnim.originalPos, true));
        }


        if (dragFlag && isHovered && !descriptionOpened && !isSelected && isDragged)
        {
            if (dragAnim.rectTransform != null)
                StartCoroutine(dragAnim.anim.Play(dragAnim, dragAnim.originalPos));
            dragFlag = false;
        }
    }

    #region CustomMethods
    public void Select()
    {
        isSelected = true;
        if (selectedAnim.rectTransform != null)
            StartCoroutine(selectedAnim.anim.Play(selectedAnim, selectedAnim.originalPos));
    }

    public void Deselect()
    {
        if (isSelected)
        {
            isSelected = false;
            if (selectedAnim.rectTransform != null)
            {
                StartCoroutine(selectedAnim.anim.PlayBackward(selectedAnim, selectedAnim.originalPos, true));
            }
        }
    }

    public static void SelectCard(InteractableUI card)
    {
        card.Select();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i] != card && allCards[i].isSelected)
            {
                allCards[i].Deselect();
            }
        }
    }
   
    public static void UpdateFocusCard()
    {
        pointerFocusedOnCard = false;
        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].isFocused)
                pointerFocusedOnCard = allCards[i].isFocused;
        }
    }
   
    public static void UpdateSelectedCard()
    {
        anyCardSelected = false;
        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].isSelected)
                anyCardSelected = allCards[i].isSelected;
        }
    }
    #endregion

    #region   InterfaceEvents
    public void OnPointerClick(PointerEventData eventData)
    {

        if (!cursorGoOut)
        {
            clickCount = 1;
        }
        else
        {
            cursorGoOut = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragged = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        isCursorOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCursorOn = false;
        isHovered = false;
        if (isDragged)
        {
            cursorGoOut = true;
        }
        if (!dragFlag && !isDragged)
        {
            dragFlag = true;
            if (!descriptionOpened)
            {
                if (dragAnim.rectTransform != null)
                    StartCoroutine(dragAnim.anim.PlayBackward(dragAnim, dragAnim.originalPos, true));
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragged = false;
        if (!dragFlag)
        {
            dragFlag = true;
            if (dragAnim.rectTransform != null && !isSelected)
                StartCoroutine(dragAnim.anim.PlayBackward(dragAnim, dragAnim.originalPos, true));
        }
        dropCount = 1;
    }

    #endregion
}
