using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;
using Plane = UnityEngine.Plane;

public class InputManager : MonoBehaviour
{

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Game")]
    public LayerMask selectableEntity;
    [HideInInspector] public bool getEntityTarget = false; 

    Vector2 touchedSeaPosition;
    Touch touch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (getEntityTarget)
            {
                //Get the sea position and pass it to the player controller
                touchedSeaPosition = GetSeaPosition();
                GameManager.Instance.playerController.SetEntityMoveTarget(touchedSeaPosition);
                print(touchedSeaPosition);
            }
            else
            {
                //If touched check if selected a Entity
                if(touch.phase == TouchPhase.Began)
                {
                    RaycastHit hit;
                    Ray touchRay;
                    touchRay = mainCamera.ScreenPointToRay(touch.position);
                    if(Physics.Raycast(touchRay, out hit, 200f, selectableEntity))
                    {
                        GameManager.Instance.playerController.currentSelectedEntity = hit.collider.gameObject.GetComponent<PlayerOceanEntity>();
                        getEntityTarget = true;
                    }
                }
                //If drag then move camera
                else if(touch.phase == TouchPhase.Moved && touch.deltaPosition.magnitude > 0.1f)
                {
                    print("MovingCam");
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
