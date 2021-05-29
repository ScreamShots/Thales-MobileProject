using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[ExecuteInEditMode]
public class GlobeCamera : MonoBehaviour
{
    [Header("Cam Parameter")]
    public Transform anchorCamera;
    public Transform anchorGlobe;
    public Camera cam;

    [Header("Globe Parameter")]
    public Vector2 aimPos;
    [Range(0,1)]public float zoom;
    public float minDist;
    public float maxDist;

    private void Update()
    {
        //Clear Value Time
        maxDist = Mathf.Max(minDist, maxDist);
        minDist = Mathf.Min(minDist, maxDist);

        if (aimPos.x < 0)
        {
            aimPos.x += 360f;
        }
        aimPos.x = aimPos.x % 360;
        aimPos.y = Mathf.Clamp(aimPos.y, -60, 60);

        //Set Globe Rotation
        anchorGlobe.rotation = Quaternion.Euler(0, aimPos.x, 0);
        //Set Camera Rotation
        anchorCamera.rotation = Quaternion.Euler(aimPos.y, 0, 0);
        //Set Camera Distance
        transform.localPosition = new Vector3(0, 0, -Mathf.Lerp(minDist, maxDist, zoom));

    }
}
