using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobePointManager : MonoBehaviour
{
    public Camera cam;
    public GlobeCamera camController;

    public GameObject missionMenu;
    public MissionLoader missionLoader;

    [Header("Globe Parameter")]
    public Transform globe;
    public float globeRadius;
    public float focusSpeed = 6;
    public bool showDebug;
    [Space(10)]
    public GlobePoint[] missionPoints = new GlobePoint[0];

    Vector3 screenPoint;
    float dot;

    private void Update()
    {
        //Show Mission if visible
        for (int i = 0; i < missionPoints.Length; i++)
        {
            Vector3 globePos = SetGlobePos(missionPoints[i].pointCoord);

            //Calcul pos in screen
            screenPoint = globePos;
            //screenPoint = cam.WorldToScreenPoint(globePos);
            //screenPoint += new Vector3(-Screen.width * 0.5f, -Screen.height * 0.5f);

            missionPoints[i].button.transform.position = screenPoint;
            missionPoints[i].button.transform.LookAt(cam.transform, Vector3.up);

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

    public void FocusOnPoint(int pointInList)
    {
        StopAllCoroutines();
        StartCoroutine(FocusingOnPoint(pointInList));
    }

    public IEnumerator FocusingOnPoint(int pointInList)
    {
        Vector2 pointPos = missionPoints[pointInList].pointCoord;

        while((pointPos - camController.aimPos).magnitude > 1)
        {
            camController.aimPos = Vector2.Lerp(camController.aimPos, pointPos, focusSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        //Send Data
        SendDataInMissionScreen(pointInList);

        missionMenu.SetActive(true);
    }

    private void SendDataInMissionScreen(int missionNbr)
    {
        //Send Mission to loader
        missionLoader.LoadMission(missionPoints[missionNbr]);
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
