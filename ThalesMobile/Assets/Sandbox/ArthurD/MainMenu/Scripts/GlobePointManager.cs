using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobePointManager : MonoBehaviour
{
    [Header("Globe Parameter")]
    public Transform globe;
    public float globeRadius;

    public Vector2[] missionPoints = new Vector2[0];


    void Start()
    {
        
    }

    private void Update()
    {

    }

    public Vector3 SetGlobePos(Vector2 coord)
    {
        Vector3 result;

        //Clean Value
        coord.x = coord.x % 360;
        coord.y = Mathf.Clamp(coord.y, -90, 90);

        //Matrix opération #CounterGimbleLock
        Matrix4x4 tranforMatrix = Matrix4x4.TRS(globe.position, globe.rotation * Quaternion.Euler(coord.y, -coord.x, 0), Vector3.one);

        //Sur quelle axe * distance je vais projetter la matrice
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
            missionPoints[i].x = missionPoints[i].x % 360;
            missionPoints[i].y = Mathf.Clamp(missionPoints[i].y, -90, 90);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < missionPoints.Length; i++)
        {
            Gizmos.DrawSphere(SetGlobePos(missionPoints[i]), 0.2f);
        }
    }
#endif
}
