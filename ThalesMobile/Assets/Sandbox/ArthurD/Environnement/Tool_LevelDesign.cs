using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Tool_LevelDesign : MonoBehaviour
{
    //public Environnement enviro;
    public GameObject zone;
    public GameObject point;

    [Space(20)]
    public Boundary limit = new Boundary(new Vector2(20, 40));
    [Space(20)]
    public List<ZoneEditable> editZones;
    [Button("add a zone")]
    public void AddTransformToZone()
    {
        GameObject newZone = Instantiate(zone, transform);
        newZone.name = "Zone-" + editZones.Count;
        newZone.AddComponent(typeof(ZoneEditable));
        ZoneEditable newzZoneComp = newZone.GetComponent<ZoneEditable>();
        newzZoneComp.points = new List<Transform>();
        newzZoneComp.pointTemplate = point;
        newzZoneComp.zoneNbr = editZones.Count;
        editZones.Add(newZone.GetComponent<ZoneEditable>());
    }

    public void PushIntoEnvironnement()
    {
        
    }

    private void OnDrawGizmos()
    {
        DebugBoundary(3,3);
    }

    private void DebugBoundary(float height, int step)
    {
        float ratio = height / step;

        for (float tempHeight = 0; tempHeight <= height; tempHeight += ratio)
        {
            //Draw the rectangle
            // _  
            Vector3 drawPos = new Vector3(limit.leftBorder, tempHeight, limit.upBorder);
            Debug.DrawRay(drawPos, Vector3.right * limit.size.x, Color.red);
            // _
            //  |
            drawPos += Vector3.right * limit.size.x;
            Debug.DrawRay(drawPos, Vector3.back * limit.size.y, Color.red);
            // _
            // _|
            drawPos += Vector3.back * limit.size.y;
            Debug.DrawRay(drawPos, Vector3.left * limit.size.x, Color.red);
            // _
            //|_|
            drawPos += Vector3.left * limit.size.x;
            Debug.DrawRay(drawPos, Vector3.forward * limit.size.y, Color.red);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(limit.offSet.x, height * 0.5f, limit.offSet.y), new Vector3(limit.size.x, height, limit.size.y));
    }
}
