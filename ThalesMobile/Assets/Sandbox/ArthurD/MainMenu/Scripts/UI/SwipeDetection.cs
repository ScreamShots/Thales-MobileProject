using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeDetection : MonoBehaviour
{
    [Header("Action")]
    public UnityEvent leftSwipe = new UnityEvent();
    public UnityEvent rightSwipe = new UnityEvent();

    [Header("Component")]
    public Camera cam = null;
    [Space(10)]
    public LayerMask uiLayer;
    public Image swipeZone = null;

    //Raycasting
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    PointerEventData pointerData;
    public EventSystem currentEventSystem;
    public GraphicRaycaster raycaster = null;

    //Touch inputs
    private Touch touch;
    private Vector2 beganTouchPos;
    private Vector2 endTouchPos;

    [Header("Information")]
    public bool touchSwipeZone = false;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            pointerData = new PointerEventData(currentEventSystem);
            pointerData.position = touch.position;
            currentEventSystem.RaycastAll(pointerData, raycastResults);
            List<RaycastResult> results = new List<RaycastResult>();

            //Moving The Sphere
            if (touch.phase == TouchPhase.Began)
            {
                beganTouchPos = touch.position;

                //RaycastHit hit;
                //Ray touchRay;
                //touchRay = cam.ScreenPointToRay(touch.position);

                raycaster.Raycast(pointerData, results);

                if (results.Count > 0)
                {
                    if (results[0].gameObject == swipeZone.gameObject)
                    {
                        touchSwipeZone = true;
                    }
                    else
                    {
                        touchSwipeZone = false;
                    }
                }
            }
            else
            {
                touchSwipeZone = false;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;

                if (touchSwipeZone)
                {
                    float horizontalSwapCoeff = Vector2.Dot(Vector2.left, endTouchPos - beganTouchPos);
                    if (Mathf.Abs(horizontalSwapCoeff) > 0.5f)
                    {
                        //Left
                        if (horizontalSwapCoeff > 0)
                        {
                            leftSwipe?.Invoke();
                        }
                        //Right
                        else
                        {
                            rightSwipe?.Invoke();
                        }
                    }

                }
            }

        }
    }
}
