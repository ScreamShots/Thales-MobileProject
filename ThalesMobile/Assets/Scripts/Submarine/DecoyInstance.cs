using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyInstance : DetectableOceanEntity
{
    private Transform _transform;

    [Header("References")]
    public LevelManager levelManager;
    public Submarine submarine;

    [Header("Decoy Number")]


    [HideInInspector] public LayerMask layerMaskTarget;
    public float decoyAngle;
    [HideInInspector] public int randomDirection;

    [HideInInspector] public bool decoyIsActive;
    private bool castRaycast = true;

    //private bool dontUpdateCoord;
    private Vector2 submarineDirection;

    protected override void Start()
    {
        base.Start();
        _transform = transform;
        coords.position = Coordinates.ConvertWorldToVector2(_transform.position);
        currentSeaLevel = OceanEntities.SeaLevel.submarine;

        levelManager = GameManager.Instance.levelManager;
        submarine = levelManager.submarine;
    }

    protected override void Update()
    {
        base.Update();

        if (decoyIsActive)
        {
            if (castRaycast)
            {
                currentDetectableState = DetectableState.undetected;
                CheckIfLandIsClose();
                //dontUpdateCoord = false;
            }
            else
            {
                DecoyMovement();
            }
        }
        else
        {
            currentDetectableState = DetectableState.cantBeDetected;
            castRaycast = true;

            // Stay at the same position of submarine. 
            coords.direction = submarine.coords.direction;
            coords.position = submarine.coords.position;
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
        }
    }

    private void CheckIfLandIsClose()
    {
        float decoyRaycast = RaySensor(_transform.position, Quaternion.Euler(0, decoyAngle, 0) * Coordinates.ConvertVector2ToWorld(coords.direction), 5, layerMaskTarget);

        if (decoyRaycast > 0)
        {
            if (decoyAngle > 0)
            {
                decoyAngle++;
            }
            else
            {
                decoyAngle--;
            }
        }
        else
        {
            castRaycast = false;
        }
    }

    private void DecoyMovement()
    {
        // Decoy go in decoy angle direction. 
        /*if (randomDirection == 0)
        {
            coords.direction = Coordinates.ConvertWorldToVector2(Quaternion.Euler(0, decoyAngle, 0) * Coordinates.ConvertVector2ToWorld(submarine.coords.direction.normalized));
            coords.position += coords.direction * Time.deltaTime * submarine.currentSpeed;
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
        }
        // Decoy go in submarine direction. 
        else
        {
            if (!dontUpdateCoord)
            {
                dontUpdateCoord = true;
                submarineDirection = submarine.coords.direction;
            }
            coords.direction = Coordinates.ConvertWorldToVector2(Quaternion.Euler(0, -decoyAngle, 0) * Coordinates.ConvertVector2ToWorld(submarine.coords.direction.normalized));
            coords.position += coords.direction * Time.deltaTime * submarine.currentSpeed;
            _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
        }   */

        coords.direction = Coordinates.ConvertWorldToVector2(Quaternion.Euler(0, decoyAngle, 0) * Coordinates.ConvertVector2ToWorld(Vector2.right));
        coords.position += coords.direction * Time.deltaTime * submarine.currentSpeed;
        _transform.position = Coordinates.ConvertVector2ToWorld(coords.position);
    }

    private float RaySensor(Vector3 startPoint, Vector3 direction, float lenght, LayerMask mask)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, lenght, mask))
        {
            Debug.DrawRay(startPoint, direction * hit.distance, Color.green);
            return 1;
        }
        else
        {
            Debug.DrawRay(startPoint, direction * lenght, Color.red);
            return 0;
        }
    }

    public override void Move(Vector2 targetPosition)
    {
        throw new System.NotImplementedException();    
    }
}
