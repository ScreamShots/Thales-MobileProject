using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Thales.Tool.LevelDesign
{
    [ExecuteInEditMode]
    public class Tool_LevelDesign : MonoBehaviour
    {
        [Header("Environnement")]
        public Environnement enviro;

        [Header("Template GameObject")]
        public GameObject pointTemplate;

        [Header("Map Info")]
        public Color limitColor = Color.red;
        public Boundary limit = new Boundary(new Vector2(20, 40));
        public float height = 0f;

        [Header("Zones")]
        public List<ZoneEditable> editZones;

        [Button("Add a zone")]
        public void AddZone()
        {
            ZoneEditable newZone = new ZoneEditable("Zone-" + editZones.Count);
            newZone.points = new List<Transform>();

            editZones.Add(newZone);

            zoneNbr = editZones.Count - 1;
        }

        public int zoneNbr;
        [Button("Add a point")]
        public void AddPoint()    //int zoneNbr
        {
            Vector3 pos = new Vector3(limit.offSet.x, height, limit.offSet.y);
            if (editZones[zoneNbr].points.Count > 0)
            {
                pos =
                    editZones[zoneNbr].points[editZones[zoneNbr].points.Count - 1].position
                    + new Vector3(
                        Mathf.Cos(editZones[zoneNbr].points.Count - 1) * Mathf.FloorToInt(editZones[zoneNbr].points.Count - 1 * 0.125f),
                        0,
                        Mathf.Sin(editZones[zoneNbr].points.Count - 1) * Mathf.FloorToInt(editZones[zoneNbr].points.Count - 1 * 0.125f)
                        );
            }
            GameObject newPoint = Instantiate(pointTemplate, pos, Quaternion.identity, transform);

            newPoint.name = "Zone-" + zoneNbr + "_Point-" + editZones[zoneNbr].points.Count;
            newPoint.AddComponent(typeof(LinkedPoints));

            LinkedPoints lp = newPoint.GetComponent<LinkedPoints>();

            lp.lastPos = newPoint.transform.position;
            lp.linkedPoints = new List<Transform>();

            editZones[zoneNbr].points.Add(newPoint.transform);
        }

        [ContextMenu("Push in Enviro")]
        public void PushIntoEnvironnement()
        {
            //Change Map Limit
            enviro.limit = limit;

            if (editZones.Count != 0)
            {
                //Create Each Zone storage space
                enviro.zones = new Zone[editZones.Count];
                //For each Zone
                for (int i = 0; i < editZones.Count - 1; i++)
                {
                    if (editZones[i].points.Count != 0)
                    {
                        //Create Points storage space
                        enviro.zones[i].points = new Vector3[editZones[i].points.Count];

                        for (int j = 0; j < editZones[i].points.Count - 1; j++)
                        {
                            enviro.zones[i].points[j] = editZones[i].points[j].position;
                        }
                    }
                    else
                    {
                        Debug.LogError("Il n'y a pas de point dans la Zone" + i);
                    }
                }
            }
            else
            {
                Debug.LogError("Il n'y a pas de zone");
            }

        }

        private void Update()
        {
            transform.position = new Vector3(limit.offSet.x, height, limit.offSet.y);
        }

        private void OnDrawGizmos()
        {
            //Dessine bords
            DrawBoundary(8, Color.red);

            //Clear Shape
            RemoveDeletePoint();

            //Dessine Zone
            if (editZones.Count != 0)
            {
                for (int i = 0; i < editZones.Count; i++)
                {
                    DrawZone(editZones[i]);
                }
            }
        }

        private void RemoveDeletePoint()
        {
            //Pour Chaque Zone
            if (editZones.Count != 0)
            {
                for (int i = 0; i < editZones.Count; i++)
                {
                    //Pour chaque points d'une zone
                    if (editZones[i].points.Count != 0)
                    {
                        for (int j = 0; j < editZones[i].points.Count - 1; j++)
                        {
                            //S'il n'est plus là
                            if (editZones[i].points[j] == null)
                            {
                                editZones[i].points.RemoveAt(j);
                            }

                        }
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
                Vector3 drawPos = new Vector3(limit.leftBorder - tempExtrude, transform.position.y + 0.1f, limit.upBorder + tempExtrude);
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
        private void DrawZone(ZoneEditable zone)
        {
            if (zone.points.Count != 0)
            {
                Gizmos.color = zone.color;

                Gizmos.DrawLine(zone.points[zone.points.Count - 1].position, zone.points[0].position);
                for (int i = 0; i < zone.points.Count - 1; i++)
                {
                    Gizmos.DrawLine(zone.points[i].position, zone.points[i + 1].position);
                }
            }
        }
    }
}
