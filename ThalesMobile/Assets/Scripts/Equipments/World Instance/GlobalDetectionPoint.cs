using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tweek.FlagAttributes;

public enum ExpirationValue {Fresh, NearExpiration, Expired, Revealed }


[TweekClass]
public class GlobalDetectionPoint : DetectionObject
{
    public DetectableOceanEntity linkedEntity;
    [SerializeField]
    GlobalPointFeedBack feedbackBehavior;

    [Header("Expiration Params")]
    public ExpirationValue expirationState;
    [TweekFlag(FieldUsage.Gameplay)]
    public float expirationDuration;
    [Range(0,1)]
    [TweekFlag(FieldUsage.Gameplay)]
    public float expirationWarningRatio;
    [TweekFlag(FieldUsage.Gameplay)]
    public float revealDuration;

    [Header("Update Params")]
    public bool updateWithMove;
    public float lerpUpdateDuration;

    [HideInInspector]
    public bool activated;
    float expirationTimer;
    float lerpTimer;

    public void InitPoint()
    {
        levelManager = GameManager.Instance.levelManager;
        levelManager.activatedDetectionObjects.Add(this);
        detectedEntities.CollectionChanged += detectedEntities_CollectionChanged;
        AddDetectable(linkedEntity);
        gameObject.SetActive(true);

        transform.position = Coordinates.ConvertVector2ToWorld(linkedEntity.coords.position);
        SetCoords();
        
        expirationTimer = 0;

        expirationState = ExpirationValue.Fresh;
        feedbackBehavior.UpdateColor(ExpirationValue.Fresh);

        activated = true;        
    }

    protected override void Start()
    {
        
    }

    public void UpdatePoint()
    {
        feedbackBehavior.UpdatePos(transform.position, Coordinates.ConvertVector2ToWorld(linkedEntity.coords.position));

        if (updateWithMove) StartCoroutine(UpdateLerp());
        else
        {
            transform.position = Coordinates.ConvertVector2ToWorld(linkedEntity.coords.position);
            SetCoords();

            expirationTimer = 0;

            if(expirationState != ExpirationValue.Revealed)
            {
                expirationState = ExpirationValue.Fresh;
                feedbackBehavior.UpdateColor(ExpirationValue.Fresh);
            }            
        }

        if(detectionState == DetectionState.revealedDetection)
        {
            feedbackBehavior.DisplayReveal(linkedEntity.detectFeedback.globalRevealIcon, linkedEntity.detectFeedback.globalRevealPointer, revealDuration);
        }
    }

    protected override void RefreshFeedBack(DetectionState newState)
    {
        base.RefreshFeedBack(newState);

        if (newState == DetectionState.revealedDetection)
        {
            feedbackBehavior.DisplayReveal(linkedEntity.detectFeedback.globalRevealIcon, linkedEntity.detectFeedback.globalRevealPointer, revealDuration);
            expirationState = ExpirationValue.Revealed;
            feedbackBehavior.UpdateColor(ExpirationValue.Revealed);
        }            
    }

    protected override void Update()
    {
        debugDetected = detectedEntities.ToList();

        if (inMadRange && detectionState == DetectionState.unknownDetection && expirationState != ExpirationValue.Expired) detectionState = DetectionState.revealedDetection;

        if (activated && expirationState != ExpirationValue.Expired && expirationState != ExpirationValue.Revealed)
        {
            if (expirationTimer <= expirationDuration) expirationTimer += Time.deltaTime;
            else
            {
                expirationState = ExpirationValue.Expired;
                feedbackBehavior.UpdateColor(ExpirationValue.Expired);
            }

            if (expirationTimer >= expirationWarningRatio * expirationDuration && expirationState == ExpirationValue.Fresh)
            {
                expirationState = ExpirationValue.NearExpiration;
                feedbackBehavior.UpdateColor(ExpirationValue.NearExpiration);
            }
        }
    }

    public IEnumerator UpdateLerp()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = Coordinates.ConvertVector2ToWorld(linkedEntity.coords.position);


        while ( lerpTimer <= lerpUpdateDuration)
        {
            if(lerpTimer/lerpUpdateDuration != 0)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, lerpTimer / lerpUpdateDuration);
                SetCoords();
            }
              
            yield return null;
            lerpTimer += Time.deltaTime;
        }

        transform.position = targetPos;
        SetCoords();

        lerpTimer = 0;
        expirationTimer = 0;

        if(expirationState != ExpirationValue.Revealed)
        {
            expirationState = ExpirationValue.Fresh;
            feedbackBehavior.UpdateColor(ExpirationValue.Fresh);
        }        
    }

    public void SetCoords()
    {
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);
    }
}