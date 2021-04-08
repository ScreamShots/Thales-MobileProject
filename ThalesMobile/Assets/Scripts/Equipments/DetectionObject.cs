using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Rémi Sécher - 08/04/2021 - Enum that determine state of a detection object 
///  (if no detectable object is currently detected, if their is detected detectable but unknown, if their is detected detectable and there are revealed(by M.A.D. for exemple)) 
/// </summary>

public enum DetectionState { noDetection, unknownDetection, revealedDetection }

/// <summary>
///  Rémi Sécher - 08/04/2021 - Base class for Detection object that interract with equipement that do revelation (lie M.A.D.) 
/// </summary>

public abstract class DetectionObject : MonoBehaviour
{
    [HideInInspector]
    public Coordinates coords;

    private DetectionState _detectionState;   
    protected List<DetectableOceanEntity> detectedEntities = new List<DetectableOceanEntity>();
    protected LevelManager levelManager;

    //Phantom value reference that call refresh of FeedBack (method) whenever a change is done to the detection state value
    public DetectionState detectionState
    {
        get { return _detectionState; }
        set
        {
            if (value != _detectionState) RefreshFeedBack(value);
            _detectionState = value;
        }
    }

    protected virtual void Start()
    {
        levelManager = GameManager.Instance.levelManager;
    }  

    //Override this to perform feedBack modification depending on the state of detectionState
    protected virtual void RefreshFeedBack(DetectionState newState)
    {

    }
}
