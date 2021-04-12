using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum ZoneState {LandCoast, SeaCalm, SeaWay, SeaTurbulent, WindyZone };

public class Environnement : MonoBehaviour
{
    public bool showDebug = false;
    public bool showBoundary = true;
    public bool drawWind = true;
    [ReadOnly]
    public Boundary limit = new Boundary(new Vector2(20,40));
    [Space(20)]
    public Zone[] zones;

    private void OnDrawGizmos()
    {
        transform.position = new Vector3(limit.offSet.x, transform.position.y, limit.offSet.y);

        if (showDebug)
        {
            if (showBoundary)
            {
                //Dessine bords
                DrawBoundary(8, Color.red);
            }

            //Dessine Zone
            if (zones.Length != 0)
            {
                for (int i = 0; i < zones.Length; i++)
                {
                    DrawZone(zones[i]);
                }
            }
        }
    }
    private void DrawBoundary(int step, Color color)
    {
        step = step >= 1 ? step : 1;

        Gizmos.color = new Color(color.r, color.b, color.b, color.a * 0.5f);

        //Top
        Gizmos.DrawCube(new Vector3(transform.position.x, 0, transform.position.z + (0.5f * limit.size.y) + (0.5f * step)), new Vector3(limit.size.x + (2 * step), 0, step));
        //Bottom
        Gizmos.DrawCube(new Vector3(transform.position.x, 0, transform.position.z - (0.5f * limit.size.y) - (0.5f * step)), new Vector3(limit.size.x + (2 * step), 0, step));
        //Left
        Gizmos.DrawCube(new Vector3(transform.position.x - (0.5f * limit.size.x) - (0.5f * step), 0, transform.position.z), new Vector3(step, 0, limit.size.y));
        //Right
        Gizmos.DrawCube(new Vector3(transform.position.x + (0.5f * limit.size.x) + (0.5f * step), 0, transform.position.z), new Vector3(step, 0, limit.size.y));


        for (float tempExtrude = 0; tempExtrude <= step; tempExtrude++)
        {
            //Draw the rectangle
            // _  
            Vector3 drawPos = new Vector3(limit.leftBorder - tempExtrude, transform.position.y + tempExtrude * 0.0f, limit.upBorder + tempExtrude);
            Debug.DrawRay(drawPos, Vector3.right * limit.size.x + Vector3.right * 2 * tempExtrude, Color.red);
            // _
            //  |
            drawPos += Vector3.right * limit.size.x + Vector3.right * 2 * tempExtrude;
            Debug.DrawRay(drawPos, Vector3.back * limit.size.y + Vector3.back * 2 * tempExtrude, Color.red);
            // _
            // _|
            drawPos += Vector3.back * limit.size.y + Vector3.back * 2 * tempExtrude;
            Debug.DrawRay(drawPos, Vector3.left * limit.size.x + Vector3.left * 2 * tempExtrude, Color.red);
            // _
            //|_|
            drawPos += Vector3.left * limit.size.x + Vector3.left * 2 * tempExtrude;
            Debug.DrawRay(drawPos, Vector3.forward * limit.size.y + Vector3.forward * 2 * tempExtrude, Color.red);
        }

    }
    private void DrawZone(Zone zone)
    {
        if (zone.points.Length != 0)
        {
            Gizmos.color = zone.debugColor;

            Gizmos.DrawLine(zone.points[zone.points.Length - 1], zone.points[0]);
            if (drawWind)
            {
                DrawWind(zone.points[zone.points.Length - 1], zone.windDir, zone.debugColor);
            }
            for (int i = 0; i < zone.points.Length - 1; i++)
            {
                Gizmos.DrawLine(zone.points[i], zone.points[i + 1]);
                if (drawWind)
                {
                    DrawWind(zone.points[i], zone.windDir, zone.debugColor);
                }
            }
        }
    }
    private void DrawWind(Vector3 point, float windDot, Color zoneColor)
    {
        Gizmos.color = Color.white;

        Gizmos.DrawRay(point, Quaternion.Euler(new Vector3(0, windDot * 180, 0)) * Vector3.forward);

        Gizmos.color = zoneColor;
    }

}
