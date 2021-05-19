using System.Collections;
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

    //Touch inputs
    [HideInInspector]public Vector2 touchedSeaPosition = new Vector2(-9999, -9999);
    private Touch touch;
    private float distance = 0;
    private float lastDistance;

    //UICards
    [HideInInspector] public bool isDraggingCard;
    [HideInInspector] public InteractableUI currentSelectedCard;

    //Raycasting
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    EventSystem currentEventSystem;
    PointerEventData pointerData;

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
        //Tap Input
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
                    
                    //Get the sea position and pass it to the player controller
                    touchedSeaPosition = GetSeaPosition();

                    //Move with gizmo if not dragging a card
                    if(!isDraggingCard)
                        playerController.SetEntityMoveTarget(touchedSeaPosition);
                    
                }
                else
                {
                    if(canUseCam)
                    {
                        //If touched check if selected a Entity
                        if (touch.phase == TouchPhase.Began)
                        {
                            RaycastHit hit;
                            Ray touchRay;
                            touchRay = mainCamera.ScreenPointToRay(touch.position);
                            if (Physics.Raycast(touchRay, out hit, 200f, selectableEntityLayer))
                            {
                                playerController.currentSelectedEntity = hit.collider.transform.parent.GetComponent<PlayerOceanEntity>();
                                camController.SetTarget(hit.collider.transform);

                                //Select Button
                                GameManager.Instance.playerController.currentSelectedEntity.linkedButton.SelectEntity();
                            }
                        }
                        //If drag then move camera
                        else if (touch.deltaPosition.magnitude > 5f && canMoveCam)
                        {
                            camController.moveDirection = -touch.deltaPosition;
                        }
                    }
                }
            }

            if (touch.deltaPosition.magnitude < 5f)
            {
                camController.moveDirection = Vector2.zero;
            }
        }

        //Glide Input
        else if(Input.touchCount == 2)
        {
            if(canZoomCam)
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

                if(deltaDistance > 1 && camController.zoomIntensity <= 1)
                {
                    camController.zoomIntensity -= (0.01f * camController.zoomSpeed) * (1 - Mathf.Clamp01(0.01f * deltaDistance));
                    camController.zoomIntensity = Mathf.Clamp01(camController.zoomIntensity);
                }
                else if(deltaDistance < -1 && camController.zoomIntensity >= 0)
                {
                    camController.zoomIntensity += (0.01f * camController.zoomSpeed)* (1 - Mathf.Clamp01(0.01f * deltaDistance));
                    camController.zoomIntensity = Mathf.Clamp01(camController.zoomIntensity);
                }  
            }
        }

        //No fingers on screen.
        else if (Input.touchCount == 0)
        {
            camController.moveDirection = Vector2.zero;
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

        return Coordinates.ConvertWorldToVector2(touchRay.GetPoint(distance));
    }

}
