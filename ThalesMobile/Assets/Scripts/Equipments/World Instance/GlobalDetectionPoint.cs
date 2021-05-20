using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ExpirationValue {Fresh, NearExpiration, Expired }

public class GlobalDetectionPoint : DetectionObject
{
    public DetectableOceanEntity linkedEntity;
    [SerializeField]
    GlobalPointFeedBack feedbackBehavior;

    [Header("Expiration Params")]
    public ExpirationValue expirationState;
    public float expirationDuration;
    public float expirationWarningTime;

    [Header("Update Params")]
    public float lerpUpdateDuration;

    public bool activated;
    float expirationTimer;
    float lerpTimer;

    public void InitPoint(float _expirationDuration)
    {
        transform.position = Coordinates.ConvertVector2ToWorld(linkedEntity.coords.position);
        levelManager.activatedDetectionObjects.Add(this);
        expirationDuration = _expirationDuration;
        expirationTimer = 0;
        activated = true;
    }

    public void UpdatePoint(float _expirationDuration, bool lerpMove)
    {
        if (lerpMove) StartCoroutine(UpdateLerp(_expirationDuration));
        else
        {
            transform.position = Coordinates.ConvertVector2ToWorld(linkedEntity.coords.position);
            expirationDuration = _expirationDuration;
            expirationTimer = 0;
            expirationState = ExpirationValue.Fresh;
            feedbackBehavior.UpdateColor(ExpirationValue.Fresh);
        }
    }

    protected override void RefreshFeedBack(DetectionState newState)
    {
        base.RefreshFeedBack(newState);

        if (newState == DetectionState.revealedDetection) feedbackBehavior.DisplayReveal(linkedEntity.detectFeedback.globalRevealIcon, linkedEntity.detectFeedback.globalRevealPointer);
    }

    protected override void Update()
    {
        debugDetected = detectedEntities.ToList();

        if (inMadRange && detectionState == DetectionState.unknownDetection) detectionState = DetectionState.revealedDetection;

        if (activated && expirationState != ExpirationValue.Expired)
        {
            if (expirationTimer <= expirationDuration) expirationTimer += Time.deltaTime;
            else
            {
                expirationState = ExpirationValue.Expired;
                feedbackBehavior.UpdateColor(ExpirationValue.Expired);
            }

            if (expirationTimer >= expirationWarningTime && expirationState == ExpirationValue.Fresh)
            {
                expirationState = ExpirationValue.NearExpiration;
                feedbackBehavior.UpdateColor(ExpirationValue.NearExpiration);
            }
        }
    }

    public IEnumerator ExpirationProgress()
    {
        while (expirationTimer <= expirationDuration)
        {
            yield return new WaitForFixedUpdate();
            expirationTimer += Time.fixedDeltaTime;
        }
    }

    public IEnumerator UpdateLerp(float _expirationDuration)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = Coordinates.ConvertVector2ToWorld(linkedEntity.coords.position);

        while (lerpUpdateDuration <= lerpTimer)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, lerpTimer / lerpUpdateDuration);
            yield return new WaitForFixedUpdate();
            lerpTimer += Time.fixedDeltaTime;
        }

        transform.position = targetPos;
        expirationDuration = _expirationDuration;
        expirationTimer = 0;
        expirationState = ExpirationValue.Fresh;
        feedbackBehavior.UpdateColor(ExpirationValue.Fresh);
    }
}