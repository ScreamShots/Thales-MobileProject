using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;

/// <summary>
///  Rémi Sécher - 08/04/2021 - Enum that determine state of a detection object 
///  (if no detectable object is currently detected, if their is detected detectable but unknown, if their is detected detectable and there are revealed(by M.A.D. for exemple)) 
/// </summary>

public enum DetectionState { noDetection, unknownDetection, revealedDetection }

/// <summary>
///  Rémi Sécher - 08/04/2021 - Base class for Detection object that interract with equipement that do revelation (like M.A.D.) 
///  Rémi Sécher - 11/04/2021 - detectionState modification implemented
/// </summary>

public abstract class DetectionObject : MonoBehaviour
{
    [HideInInspector]
    public Coordinates coords;

    [SerializeField]
    private DetectionState _detectionState;

    protected ObservableCollection<DetectableOceanEntity> detectedEntities = new ObservableCollection<DetectableOceanEntity>();

    [SerializeField]
    protected List<DetectableOceanEntity> debugDetected = new List<DetectableOceanEntity>();

    protected LevelManager levelManager;

    [HideInInspector]
    public bool inMadRange;

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
        detectedEntities.CollectionChanged += detectedEntities_CollectionChanged;
    }

    protected virtual void Update()
    {
        debugDetected = detectedEntities.ToList();

        if (inMadRange && detectionState == DetectionState.unknownDetection) detectionState = DetectionState.revealedDetection;
        else if (!inMadRange && detectionState == DetectionState.revealedDetection && detectedEntities.Count > 0) detectionState = DetectionState.unknownDetection;
        else if (!inMadRange && detectionState == DetectionState.revealedDetection && detectedEntities.Count == 0) detectionState = DetectionState.noDetection;
    }

    protected virtual void detectedEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (detectedEntities.Count == 0 && detectionState != DetectionState.noDetection) detectionState = DetectionState.noDetection;
        else if (detectedEntities.Count > 0 && detectionState == DetectionState.noDetection) detectionState = DetectionState.unknownDetection;        
    }

    //Override this to perform feedBack modification depending on the state of detectionState
    protected virtual void RefreshFeedBack(DetectionState newState)
    {

    }

    protected void AddDetectable(DetectableOceanEntity entity)
    {
        detectedEntities.Add(entity);
        entity.detectors.Add(this);
    }

    protected void RemoveDetectable(DetectableOceanEntity entity)
    {
        if (detectedEntities.Contains(entity)) detectedEntities.Remove(entity);
        if (entity.detectors.Contains(this)) entity.detectors.Remove(this);
    }
}
