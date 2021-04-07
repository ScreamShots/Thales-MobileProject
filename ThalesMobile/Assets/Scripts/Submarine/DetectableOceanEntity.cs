using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEntities;

/// <summary>
/// Antoine Leroux - 27/03/2021 - Detectable State is an enum describing the entity's detectable state, meanig is the entity has been detected by something?
/// </summary>
public enum DetectableState
{
    undetected,
    detected,
    cantBeDetected
}

/// <summary>
/// Antoine Leroux - 27/03/2021 - DetectableOceanEntity is the root class for every entity which can be detected.
/// </summary>
public abstract class DetectableOceanEntity : OceanEntity
{
    public DetectableState currentDetectableState;
    public DetectionFeedback detectFeedback;
}
