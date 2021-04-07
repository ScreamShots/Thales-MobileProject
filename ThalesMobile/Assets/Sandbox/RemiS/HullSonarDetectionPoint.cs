using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullSonarDetectionPoint : DetectionObject
{
    public Coordinates coords;
    [SerializeField]
    float fadeDuration;

    public void ActivatePoint(DetectableOceanEntity detectedElement)
    {
        levelManager.activatedDetectionObjects.Add(this);
        detectedEntities.Add(detectedElement);
        transform.position = Coordinates.ConvertVector2ToWorld(detectedElement.coords.position);
        gameObject.SetActive(true);
        StartCoroutine(Fade());
    }

    public void DesactivatePoint()
    {
        gameObject.SetActive(false);
        levelManager.activatedDetectionObjects.Remove(this);
        detectedEntities.Clear();
        detectionState = DetectionState.noDetection;
        transform.localPosition = Vector3.zero;
        StopAllCoroutines();
    }

    IEnumerator Fade()
    {
        float timer = 0;

        while (timer < fadeDuration)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        DesactivatePoint();
    }
}
