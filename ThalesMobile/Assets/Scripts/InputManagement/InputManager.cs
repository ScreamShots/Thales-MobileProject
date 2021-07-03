﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using Plane = UnityEngine.Plane;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
 

 
public class InputManager : MonoBehaviour
{

    [Header("Camera")]
    public Camera mainCamera;
    public CameraController camController;

    [Header("Game")]
    public LayerMask selectableEntityLayer;
    [HideInInspector] public bool touchingGame;
    [HideInInspector] public bool getEntityTarget;
    [HideInInspector] public bool gettingEntityTarget;
    [HideInInspector] public bool canUseCam;
    [HideInInspector] public bool canZoomCam;
    [HideInInspector] public bool canMoveCam;
    private PlayerController playerController;

    [Header("Audio")]
    private SoundHandler soundHandler;
    public AudioSource audioSource;
    public AudioMixerGroup targetGroup;
     
    public AudioClip setTargetSound;
     
    public float setTargetSoundVolume;

    //Touch inputs
    [HideInInspector]public Vector2 touchedSeaPosition = new Vector2(-9999, -9999);
    private Touch touch;
    private float distance = 0;
    private float lastDistance;

    //Inertie
    [SerializeField] private bool activateInertie = true;
    private float dragStartTime = 0;
    [SerializeField] private float inertialVelocity = 0;
    private Vector2 move = Vector2.zero;
    private Vector2 dragStartPos = Vector2.zero;

    //UICards
    [HideInInspector] public bool isDraggingCard;
    [HideInInspector] public InteractableUI currentSelectedCard;

    //Raycasting
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    EventSystem currentEventSystem;
    PointerEventData pointerData;

    //Map Limits
    [HideInInspector] public float minX;
    [HideInInspector] public float maxX;
    [HideInInspector] public float minY;
    [HideInInspector] public float maxY;


    void Start()
    {
        currentEventSystem = EventSystem.current;
        playerController = GameManager.Instance.playerController;
        soundHandler = GameManager.Instance.soundHandler;

        canUseCam = true;
        canZoomCam = true;
        canMoveCam = true;
     }

    void Update()
    {
        if(Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            pointerData = new PointerEventData(currentEventSystem);
            pointerData.position = touch.position;
            currentEventSystem.RaycastAll(pointerData, raycastResults);

            if(raycastResults.Count < 1 || (raycastResults.Count == 1 && raycastResults[0].gameObject.TryGetComponent<InteractableUI>(out var I)))
            {
                touchingGame = true;

                if (getEntityTarget)
                {
                    gettingEntityTarget = true;
                    
                    touchedSeaPosition = GetSeaPosition();

                    if(!isDraggingCard)
                        playerController.SetEntityMoveTarget(touchedSeaPosition);
                    
                }
                else
                {
                    if(canUseCam)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            RaycastHit hit;
                            Ray touchRay;
                            touchRay = mainCamera.ScreenPointToRay(touch.position);
                            if (Physics.Raycast(touchRay, out hit, 200f, selectableEntityLayer))
                            {
                                var entity = hit.collider.transform.parent.GetComponent<PlayerOceanEntity>();

                                if (playerController.currentSelectedEntity == entity)
                                {
                                    if (camController != null)
                                    {
                                        camController.SetTarget(hit.collider.transform);
                                    }
                                }
                                else
                                {
                                    entity.linkedButton.SelectEntity();
                                }

                                dragStartTime = Time.time;
                                dragStartPos = touch.position;
                            }
                        }
                        if (touch.phase == TouchPhase.Moved)
                        {
                            if (touch.deltaPosition.magnitude > 5f && canMoveCam)
                            {
                                if (camController != null)
                                {

                                    camController.moveDirection = -touch.deltaPosition;
                                }
                            }

                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            #region inertie
                            if (activateInertie)
                            {
                                move = touch.position - dragStartPos;
                                inertialVelocity = move.magnitude / (Time.time - dragStartTime);

                                if (touch.deltaPosition.magnitude < 5f)
                                {
                                    inertialVelocity = 0;
                                }
                            }
                            #endregion
                        }
                        
                    }
                }
            }

            #region inertie
            if (activateInertie)
            {
                if (camController != null)
                {

                    //Apply inertie
                    camController.moveDirection -= move.normalized * inertialVelocity * 0.135f * Time.deltaTime;
                    inertialVelocity *= 10f * Time.deltaTime;
                }
            }
            #endregion

            if (touch.deltaPosition.magnitude < 5f && camController != null)
            {
                camController.moveDirection = Vector2.zero;
            }
        }
        else if (Input.touchCount == 2)
        {
            #region inertie
            if (activateInertie)
            {
                //Stop inertie
                inertialVelocity = 0;
            }
            #endregion

            if (canZoomCam)
            {
                Vector2 touch0;
                Vector2 touch1;

                touch0 = Input.GetTouch(0).position;
                touch1 = Input.GetTouch(1).position;

                if(distance == 0)
                {
                    distance = Vector2.Distance(touch0, touch1);
                }

                lastDistance = distance;
                distance = Vector2.Distance(touch0, touch1);
                float deltaDistance = distance - lastDistance;

                if (camController != null)
                {

                    if (deltaDistance > 1 && camController.zoomIntensity <= 1)
                    {
                        camController.zoomIntensity -= (0.01f * camController.zoomSpeed) * (1 - Mathf.Clamp01(0.01f * deltaDistance));
                        camController.zoomIntensity = Mathf.Clamp01(camController.zoomIntensity);
                    }
                    else if (deltaDistance < -1 && camController.zoomIntensity >= 0)
                    {
                        camController.zoomIntensity += (0.01f * camController.zoomSpeed) * (1 - Mathf.Clamp01(0.01f * deltaDistance));
                        camController.zoomIntensity = Mathf.Clamp01(camController.zoomIntensity);
                    }
                }
            }
        }
        else if (Input.touchCount == 0)
        {
            if (camController != null)
            {
                camController.moveDirection = Vector2.zero;
            }
            distance = 0;

            touchingGame = false;

            if (gettingEntityTarget )
            { 
                if(playerController.currentSelectedEntity.GetType() != typeof(Helicopter))
                {
                    getEntityTarget = false;
                    gettingEntityTarget = false;
                    if(currentSelectedCard!=null)
                        currentSelectedCard.Deselect();

                
                    playerController.SetEntityMoveTarget(touchedSeaPosition);
                    touchedSeaPosition = new Vector2(-9999, -9999);

                    audioSource.volume = Mathf.Clamp01(setTargetSoundVolume);
                    soundHandler.PlaySound(setTargetSound, audioSource, targetGroup);
                }

            }
        }
    }

    public Vector2 GetSeaPosition()
    {
        touch = Input.GetTouch(0);
        Ray touchRay;
        touchRay = mainCamera.ScreenPointToRay(touch.position);

        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, 0));
        float distance;
        ground.Raycast(touchRay, out distance);

        return ClampToMap(Coordinates.ConvertWorldToVector2(touchRay.GetPoint(distance)));
    }

    public Vector2 ClampToMap(Vector2 vector)
    {
        if (vector.x > maxX)
            vector.x = maxX;

        if (vector.x < minX)
            vector.x = minX;

        if (vector.y > maxY)
            vector.y = maxY;

        if (vector.y < minY)
            vector.y = minY;

            return vector;
    }

}
