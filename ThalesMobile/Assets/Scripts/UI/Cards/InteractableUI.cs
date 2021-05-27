using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableUI : MonoBehaviour
     , IPointerUpHandler
     , IBeginDragHandler
     , IEndDragHandler
     , IDragHandler
     , IPointerDownHandler
{
    public static TweeningAnimator darkBackAnim;

    public TweeningAnimator dragAnim;
    public TweeningAnimator holdAnim;
    public TweeningAnimator selectedAnim;
    public bool darkenBackWhileHold;

    [HideInInspector] public bool isDragged;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool descriptionOpened;
    [HideInInspector] public bool canClick;
    [HideInInspector] public bool canDrag;
    [HideInInspector] public bool canHold;

    private bool pointerEnter;
    private float holdTime;

    //Delegates
    public delegate void Abort();
    public Abort abortHandler;

    public delegate void Click();
    public Click clickHandler;

    public delegate void Drag();
    public Drag beginDragHandler;
    public Drag endDragHandler;

    public delegate void Hold();
    public Hold holdHandler;

    //Components
    EntityDeckUI deckUI;


    private void Start()
    {
        deckUI = GameManager.Instance.uiHandler.entityDeckUI;
        holdAnim = deckUI.descriptionContainerAnim;

        if(dragAnim.rectTransform != null)
        {
            dragAnim.GetCanvasGroup();
            dragAnim.anim = Instantiate(dragAnim.anim);
        }
        
        if(selectedAnim.rectTransform != null)
        {
            selectedAnim.anim = Instantiate(selectedAnim.anim);
        }

        holdAnim.GetCanvasGroup();
        holdAnim.anim = Instantiate(holdAnim.anim);

        canClick = true;
        canHold = true;
        canDrag = true;
    }

    private void Update()
    {
        if(pointerEnter && !isDragged)
        {
            holdTime += Time.deltaTime;
        }

        if (holdTime > 0.8f && !deckUI.descriptionOpened)
        {
            if(canHold)
                deckUI.OpenDescription();

            if (holdHandler != null && canHold)
            {
                holdHandler();
            }
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
    #endregion

    #region   InterfaceEvents
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(beginDragHandler!=null && canDrag)
            beginDragHandler();

        isDragged = true;

        if (dragAnim.rectTransform != null && canDrag)
            StartCoroutine(dragAnim.anim.Play(dragAnim, dragAnim.originalPos));
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(endDragHandler!=null && canDrag)
            endDragHandler();

        if (dragAnim.rectTransform != null && canDrag)
            StartCoroutine(dragAnim.anim.PlayBackward(dragAnim, dragAnim.originalPos, true));
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (holdTime < 0.8f && !isDragged)
        {
            if(clickHandler != null && canClick)
                clickHandler();
        }

        isDragged = false;
        pointerEnter = false;
        holdTime = 0;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerEnter = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //
    }
    #endregion
}
