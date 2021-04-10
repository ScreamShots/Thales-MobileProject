using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ZoneEditable : MonoBehaviour
{
    [HideInInspector] public GameObject pointTemplate;
    [HideInInspector] public int zoneNbr;

    public Color color = Color.white;
    [Space(10)]
    [ReorderableList]
    public List<Transform> points = new List<Transform>();
    [Button("add a point")]
    public void AddTransformToZone()
    {
        GameObject newZone = Instantiate(pointTemplate, transform);
        newZone.name = "Zone-"+ zoneNbr +"-"+ points.Count;
        points.Add(newZone.transform);
    }

    private void OnDrawGizmos()
    {
        if(points.Count != 0)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(points[points.Count-1].position, points[0].position);
            for (int i = 0; i < points.Count-1; i++)
            {
                Gizmos.DrawLine(points[i].position, points[i+1].position);
            }
        }

    }
}
