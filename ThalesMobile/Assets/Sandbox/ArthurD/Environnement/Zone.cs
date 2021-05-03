using NaughtyAttributes;
using UnityEngine;
using System;

[Serializable]
public struct Zone
{
    [HideInInspector]
    public string name;

    [Header("Parameter")]
    public ZoneState state;
    [Range(0f, 2f)] public float windDir;

    [Header("Info")]
    public Color debugColor;
    [ReorderableList] public Vector2[] points;

    public bool PointInZone(Vector2 pointTest)
    {
        if (points.Length != 0)
        {
            int lastPoint = points.Length - 1;
            // Get the angle between the point and the first and last point.  FirstPoint-Testpoint-LastPoint
            float total_angle = CalcAngle(points[lastPoint], pointTest, points[0]);

            // Add l'angles from the point to each other consecutive pair of point.
            for (int i = 0; i < points.Length - 1; i++)
            {
                total_angle += CalcAngle(points[i], pointTest, points[i + 1]);
            }

            // If the point is inside the result wil be 2PI or -2PI
            // (depending of points in clock sens or counter clock sens)
            // If the point is outside the result wil be 0
            return (Mathf.Abs(total_angle) > 3.14f);
        }
        return false;
    }

    private float CalcAngle(Vector3 a, Vector3 b, Vector3 c)
    {
        // Get the dot product.
        float dot_product = Vector3.Dot(a - b, c - b);

        // Get the cross product.
        float cross_product = Vector3.Cross(a - b, c - b).z;

        // Calculate the angle.
        return (float)Mathf.Atan2(cross_product, dot_product);
    }
}
