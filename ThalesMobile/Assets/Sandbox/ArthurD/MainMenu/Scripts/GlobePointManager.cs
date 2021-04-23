using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobePointManager : MonoBehaviour
{
    public Camera cam;

    [Header("Globe Parameter")]
    public Transform globe;
    public float globeRadius;

    public GlobePoint[] missionPoints = new GlobePoint[0];
    public bool showDebug;

    Vector3 screenPoint;
    float dot;

    private void Update()
    {
        for (int i = 0; i < missionPoints.Length; i++)
        {
            Vector3 globePos = SetGlobePos(missionPoints[i].pointCoord);

            //Calcul pos in screen
            screenPoint = cam.WorldToScreenPoint(globePos);
            screenPoint += new Vector3(-Screen.width * 0.5f, -Screen.height * 0.5f);

            missionPoints[i].buttonrectTrans.localPosition = screenPoint;

            dot = Vector3.Dot(
                (globe.position - cam.transform.position).normalized,
                (globePos - globe.position).normalized);

            //Debug
            /*
            Debug.DrawLine(globe.position, cam.transform.position, Color.red);
            Debug.DrawLine(globePos, globe.position, Color.blue);
            */

            missionPoints[i].button.SetActive(dot <= 0);
        }
    }

    public Vector3 SetGlobePos(Vector2 coord)
    {
        Vector3 result;

        //Clean Value
        coord.x = coord.x % 360;
        coord.y = Mathf.Clamp(coord.y, -90, 90);

        //Matrix opération #CounterGimbleLock
        Matrix4x4 tranforMatrix = Matrix4x4.TRS(globe.position, globe.rotation * Quaternion.Euler(coord.y, -coord.x, 0), Vector3.one);

        Vector3 sens = Vector3.back * globeRadius;

        result = tranforMatrix.MultiplyPoint(sens);

        return result;
    }
    public Vector3 SetGlobePos(float longitude, float latitude)
    {
        return SetGlobePos(new Vector2(longitude, latitude));
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        for (int i = 0; i < missionPoints.Length; i++)
        {
            missionPoints[i].pointCoord.x = missionPoints[i].pointCoord.x % 360;
            missionPoints[i].pointCoord.y = Mathf.Clamp(missionPoints[i].pointCoord.y, -90, 90);
        }
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < missionPoints.Length; i++)
            {
                Gizmos.DrawSphere(SetGlobePos(missionPoints[i].pointCoord), 0.1f);
            }

            Gizmos.color = Color.white;
        }
    }
#endif
}
