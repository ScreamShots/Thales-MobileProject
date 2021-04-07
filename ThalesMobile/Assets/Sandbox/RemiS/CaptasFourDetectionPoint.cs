using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEquipement;

public class CaptasFourDetectionPoint : MonoBehaviour
{
    [HideInInspector]
    public Coordinates coords;
    [HideInInspector]
    public float fadeDuration;

    CaptasFour source;

    public void ActivatePoint(Coordinates targetCoors, float _fadeDuration, CaptasFour _source)
    {
        transform.position = Coordinates.ConvertVector2ToWorld(targetCoors.position);
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);
        fadeDuration = _fadeDuration;
        source = _source;
        gameObject.SetActive(true);
        StartCoroutine(Fade());
    }

    public void DesactivatePoint()
    {
        gameObject.SetActive(false);
        source.availableDetectionPoints.Add(this);
        source.usedDetectionPoints.Remove(this);
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
