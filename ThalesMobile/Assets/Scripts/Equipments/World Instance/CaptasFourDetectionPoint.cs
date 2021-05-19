using System.Collections;
using UnityEngine;
using PlayerEquipement;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Rémi Sécher - 08/04/2021 - Behaviour for point used for detection through Capatas4. Not inherited from Detection Objets cause no interaction with M.A.D. equipement
/// </summary>

public class CaptasFourDetectionPoint : DetectionObject
{
    [HideInInspector]
    public float fadeDuration;

    CaptasFour source;

    [SerializeField]
    Image dotRenderer;
    [SerializeField, Range(0,1)]
    float startFadeRatio;

    //Get the point out of the unused state in the pool and place it on the map
    public void ActivatePoint(DetectableOceanEntity target, float _fadeDuration, CaptasFour _source)
    {
        levelManager = GameManager.Instance.levelManager;

        transform.position = Coordinates.ConvertVector2ToWorld(target.coords.position);
        coords.position = Coordinates.ConvertWorldToVector2(transform.position);

        fadeDuration = _fadeDuration;
        source = _source;
        dotRenderer.color = new Color(dotRenderer.color.r, dotRenderer.color.g, dotRenderer.color.b, 1f);

        AddDetectable(target);

        gameObject.SetActive(true);

        StartCoroutine(Fade());
    }

    //Get the point back in the unused state in the pool and setting his pos back to (0,0)
    public void DesactivatePoint()
    {
        gameObject.SetActive(false);

        dotRenderer.color = new Color(dotRenderer.color.r, dotRenderer.color.g, dotRenderer.color.b, 1f);

        foreach (DetectableOceanEntity entity in detectedEntities.ToList())
        {
            RemoveDetectable(entity);
        }
        detectedEntities.Clear();
        
        source.availableDetectionPoints.Add(this);
        source.usedDetectionPoints.Remove(this);

        transform.localPosition = Vector3.zero;

        StopAllCoroutines();
    }

    //Slow Fade of the point since it's apparaition
    IEnumerator Fade()
    {
        float timer = 0;
        float startFadeTime = fadeDuration * startFadeRatio;
        Color baseColor = dotRenderer.color;
        Color transparentColor = new Color(dotRenderer.color.r, dotRenderer.color.g, dotRenderer.color.b, 0f);

        while (timer < fadeDuration)
        {
            if (timer >= startFadeTime) dotRenderer.color = Color.Lerp(baseColor, transparentColor, timer - startFadeTime / fadeDuration - startFadeTime);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }

        DesactivatePoint();
    }
}
