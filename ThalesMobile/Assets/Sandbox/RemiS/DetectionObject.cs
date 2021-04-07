using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectionState { noDetection, unknownDetection, revealedDetection }

public abstract class DetectionObject : MonoBehaviour
{
    [HideInInspector]
    public Coordinates coords;

    private DetectionState _detectionState;   
    protected List<DetectableOceanEntity> detectedEntities = new List<DetectableOceanEntity>();
    protected LevelManager levelManager;

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

    protected virtual void RefreshFeedBack(DetectionState newState)
    {

    }
}
