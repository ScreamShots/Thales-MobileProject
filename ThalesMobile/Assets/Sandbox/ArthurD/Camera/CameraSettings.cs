using UnityEngine;
using System;

[Serializable]
public class CameraSettings
{
    public float minAngle;
    public float maxAngle;
    public AnimationCurve angleEvolve;
    [Space(10)]
    public float minHeight;
    public float maxHeight;
    public AnimationCurve heightEvolve;
    [Space(10)]
    public float minTargetProximity;
    public float maxTargetProximity;
    public AnimationCurve proximityEvolve;
    [Space(10)]
    public float minFov;
    public float maxFov;
    public AnimationCurve fovEvolve;

    public CameraSettings()
    {
        minAngle = 45f;
        maxAngle = 90f;
        angleEvolve = AnimationCurve.Linear(0, 0, 1, 1);

        minHeight = 3f;
        maxHeight = 10f;
        heightEvolve = AnimationCurve.Linear(0, 0, 1, 1);

        minTargetProximity = -5f;
        maxTargetProximity = 0f;
        proximityEvolve = AnimationCurve.Linear(0, 0, 1, 1);

        minFov = 60f;
        maxFov = 90f;
        fovEvolve = AnimationCurve.Linear(0, 0, 1, 1);
    }

    public float EvalAngle(float time)
    {
        time = Mathf.Clamp01(time);
        return minAngle + angleEvolve.Evaluate(time) * (maxAngle - minAngle);
    }
    public float EvalHeight(float time)
    {
        time = Mathf.Clamp01(time);
        return minHeight + heightEvolve.Evaluate(time) * (maxHeight - minHeight);
    }
    public float EvalPromimity(float time)
    {
        time = Mathf.Clamp01(time);
        return minTargetProximity + proximityEvolve.Evaluate(time) * (maxTargetProximity - minTargetProximity);
    }
    public float EvalFieldOfView(float time)
    {
        time = Mathf.Clamp01(time);
        return minFov + fovEvolve.Evaluate(time) * (maxFov - minFov);
    }
}
