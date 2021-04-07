using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectionState { noDetection, unknownDetection, revealedDetection }

public abstract class DetectionObject : MonoBehaviour
{
    private DetectionState _detectionState;

    protected List<DetectableOceanEntity> detectedEntities = new List<DetectableOceanEntity>();

    protected LevelManager levelManager;

    protected virtual void Awake()
    {
        levelManager = GameManager.Instance.levelManager;
    }

    public DetectionState detectionState
    {
        get { return _detectionState; }
        set
        {
            if (value != _detectionState) RefreshFeedBack(value);
            _detectionState = value;
        }
    }

    protected virtual void RefreshFeedBack(DetectionState newState)
    {

    }
}
