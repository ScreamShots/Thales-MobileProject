using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using Plane = UnityEngine.Plane;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GlobeInput : MonoBehaviour
{
    [Header("Camera")]
    public GlobeCamera camController;

    [Header("Menu")]
    public LayerMask selectableEntityLayer;
    public GameObject earth;
    [Space(5)]
    private bool touchingEarth = false;
    public float rotationSpeed = 50;
    public Vector2 velocity = Vector2.zero;

    //Touch inputs
    private Touch touch;
    //Glide
    private float distance = 0;
    private float lastDistance;
    //
    private float dragStartTime = 0;
    public float inertialVelocity = 0;
    private Vector2 move = Vector2.zero;
    private Vector2 dragStartPos = Vector2.zero;


    //Raycasting
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    EventSystem currentEventSystem;
    PointerEventData pointerData;

    [Header("Information")]
    public bool isDraging = false;

    void Start()
    {
        currentEventSystem = EventSystem.current;
    }

    void Update()
    {
        //Tap Input
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            pointerData = new PointerEventData(currentEventSystem);
            pointerData.position = touch.position;
            currentEventSystem.RaycastAll(pointerData, raycastResults);
            
            //Moving The Sphere
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray touchRay;
                touchRay = camController.cam.ScreenPointToRay(touch.position);
                if (Physics.Raycast(touchRay, out hit, 200f, selectableEntityLayer))
                {
                    if(hit.collider.gameObject == earth)
                    {
                        touchingEarth = true;
                    }
                    else
                    {
                        touchingEarth = false;
                    }
                }
                else
                {
                    touchingEarth = false;
                }

                dragStartTime = Time.time;
                dragStartPos = touch.position;
            }
            //If drag then move camera
            if (touch.phase == TouchPhase.Moved)
            {
                isDraging = true;

                if (touch.deltaPosition.magnitude > 5f)
                {
                    if (touchingEarth)
                    {
                        camController.aimPos -= touch.deltaPosition.normalized * rotationSpeed * Time.deltaTime;
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                isDraging = false;
                touchingEarth = false;
                
                move = touch.position - dragStartPos;
                inertialVelocity = move.magnitude / (Time.time -dragStartTime);

                if(touch.deltaPosition.magnitude < 5f)
                {
                    inertialVelocity = 0;
                }
            }
        }
        //Glide Input
        else if (Input.touchCount == 2)
        {
            //Stop inertie
            inertialVelocity = 0;

            Vector2 touch0;
            Vector2 touch1;

            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;

            if (distance == 0)
            {
                distance = Vector2.Distance(touch0, touch1);
            }

            lastDistance = distance;
            distance = Vector2.Distance(touch0, touch1);
            float deltaDistance = distance - lastDistance;

            if (deltaDistance > 1 && camController.zoom <= 1)
            {
                camController.zoom -= 0.01f * (1 - Mathf.Clamp01(0.01f * deltaDistance));
                camController.zoom = Mathf.Clamp01(camController.zoom);
            }
            else if (deltaDistance < -1 && camController.zoom >= 0)
            {
                camController.zoom += 0.01f * (1 - Mathf.Clamp01(0.01f * deltaDistance));
                camController.zoom = Mathf.Clamp01(camController.zoom);
            }
        }
        //No fingers on screen.
        else if (Input.touchCount == 0)
        {
            distance = 0;
        }

        camController.aimPos -= move.normalized * inertialVelocity * 0.135f * Time.deltaTime;
        inertialVelocity *= 10f * Time.deltaTime;

    }

    public Vector2 GetSeaPosition()
    {
        touch = Input.GetTouch(0);
        Ray touchRay;
        touchRay = camController.cam.ScreenPointToRay(touch.position);

        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, 0));
        float distance;
        ground.Raycast(touchRay, out distance);

        return Coordinates.ConvertWorldToVector2(touchRay.GetPoint(distance));
    }
}
