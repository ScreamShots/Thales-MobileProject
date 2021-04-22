using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobePointManager : MonoBehaviour
{
    [Header("Globe Parameter")]
    public Transform globe;
    public float globeRadius;

    public Vector2 testPoint;

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
        Matrix4x4 tranforMatrix = Matrix4x4.TRS(globe.position, Quaternion.Euler(coord.y, -coord.x, 0), Vector3.one);

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
        testPoint.x = testPoint.x % 360;
        testPoint.y = Mathf.Clamp(testPoint.y, -90, 90);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(SetGlobePos(testPoint), 0.2f);
    }
#endif
}
