using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Thales.Tool.LevelDesign
{
#if UNITY_EDITOR

    [ExecuteInEditMode]
    public class Tool_LevelDesign : MonoBehaviour
    {
        [Header("Environnement"), HideInInspector]
        public Environnement enviro;

        [Header("Template GameObject")]
        public GameObject pointTemplate;

        [Header("Map Info")]
        [HideInInspector] public Boundary limit = new Boundary(new Vector2(20, 40));
        private Boundary oldLimit = new Boundary(new Vector2(20, 40));
        [HideInInspector] public float height = 0f;

        [Header("Zones")]
        public List<ZoneEditable> editZones;
        [HideInInspector] public int zoneNbr;

        [Header("Debug")]
        public Tool_LD_DebugOption debugOption = new Tool_LD_DebugOption();

        #region Button
        public void AddZone()
        {
            ZoneEditable newZone = new ZoneEditable("Zone-" + editZones.Count);
            newZone.points = new List<Transform>();

            editZones.Add(newZone);

            zoneNbr = editZones.Count - 1;

            float size = 8;
            AddPoint(zoneNbr, new Vector2(-size, -size));
            AddPoint(zoneNbr, new Vector2(-size, size));
            AddPoint(zoneNbr, new Vector2(size, size));
            AddPoint(zoneNbr, new Vector2(size, -size));
        }
        public void AddPoint()
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
                        ) * 4;
            }
            GameObject newPoint = Instantiate(pointTemplate, pos, Quaternion.identity, transform);

            newPoint.name = "Zone-" + zoneNbr + "_Point-" + editZones[zoneNbr].points.Count;
            newPoint.AddComponent(typeof(LinkedPoints));

            LinkedPoints lp = newPoint.GetComponent<LinkedPoints>();

            lp.lastPos = newPoint.transform.position;
            lp.linkedPoints = new List<Transform>();
            lp.limit = limit;
            editZones[zoneNbr].points.Add(newPoint.transform);
        }
        public void AddPoint(int zone, Vector2 pos2D)
        {
            Vector3 pos = new Vector3(limit.offSet.x + pos2D.x, height, limit.offSet.y + pos2D.y);

            GameObject newPoint = Instantiate(pointTemplate, pos, Quaternion.identity, transform);

            newPoint.name = "Zone-" + zone + "_Point-" + editZones[zone].points.Count;
            newPoint.AddComponent(typeof(LinkedPoints));

            LinkedPoints lp = newPoint.GetComponent<LinkedPoints>();

            lp.lastPos = newPoint.transform.position;
            lp.linkedPoints = new List<Transform>();
            lp.limit = limit;
            editZones[zone].points.Add(newPoint.transform);
        }
        #endregion

        #region ContextMenue
        [ContextMenu("Push Into Environnement")]
        public void PushIntoEnvironnement()
        {
            //Change Map Limit
            enviro.limit = limit;

            if (editZones.Count != 0)
            {
                //Create Each Zone storage space
                enviro.zones = new Zone[editZones.Count];
                //For each Zone
                for (int i = 0; i < editZones.Count; i++)
                {
                    //Zone Info
                    enviro.zones[i].name = editZones[i].name;
                    enviro.zones[i].windDir = editZones[i].windDir;
                    enviro.zones[i].debugColor = editZones[i].color;

                    if (editZones[i].points.Count != 0)
                    {
                        //Create Points storage space
                        enviro.zones[i].points = new Vector2[editZones[i].points.Count];

                        for (int j = 0; j < editZones[i].points.Count; j++)
                        {
                            enviro.zones[i].points[j] = new Vector2(editZones[i].points[j].position.x, editZones[i].points[j].position.z);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Il n'y a pas de point dans la Zone" + i);
                    }
                }
            }
            else
            {
                Debug.LogError("Il n'y a pas de zone");
            }

        }
        
        [ContextMenu("Generate New Environnement")]
        public void GenerateNewEnvironnement()
        {
            GameObject go = new GameObject();

            go.name = "(new)Generated Enviro";

            go.AddComponent(typeof(Environnement));
            Environnement enviro = go.GetComponent<Environnement>();

            //Change Map Limit
            enviro.limit = limit;

            if (editZones.Count != 0)
            {
                //Create Each Zone storage space
                enviro.zones = new Zone[editZones.Count];
                //For each Zone
                for (int i = 0; i < editZones.Count; i++)
                {
                    //Zone Info
                    enviro.zones[i].name = editZones[i].name;
                    enviro.zones[i].windDir = editZones[i].windDir;
                    enviro.zones[i].debugColor = editZones[i].color;

                    if (editZones[i].points.Count != 0)
                    {
                        //Create Points storage space
                        enviro.zones[i].points = new Vector2[editZones[i].points.Count];

                        for (int j = 0; j < editZones[i].points.Count; j++)
                        {
                            enviro.zones[i].points[j] = new Vector2(editZones[i].points[j].position.x, editZones[i].points[j].position.z);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Il n'y a pas de point dans la Zone" + i);
                    }
                }
            }
            else
            {
                Debug.LogError("Il n'y a pas de zone");
            }

            this.enviro = enviro;
        }
        #endregion
#endif
        private void Update()
        {
            LimitPos();

            #region Point Correction on Map Scaling
            /*
            if (oldLimit.size != limit.size)
            {
                ScalePointCorrection();
            }
            oldLimit = limit;
            */
            #endregion
        }

        private void OnValidate()
        {
            transform.position = new Vector3(limit.offSet.x, height, limit.offSet.y);

            if (editZones.Count != 0)
            {
                for (int i = 0; i < editZones.Count; i++)
                {
                    RemoveDeletePointFromZone(editZones[i]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (debugOption.showDebug)
            {
                //Dessine bords
                DrawBoundary(debugOption.borderThickness);

                //Dessine Zone
                if (editZones.Count != 0)
                {
                    for (int i = 0; i < editZones.Count; i++)
                    {
                        RemoveDeletePointFromZone(editZones[i]);
                        DrawZone(editZones[i]);
                    }
                }
            }
        }

        #region Méthodes
        private void LimitPos()
        {
            transform.position = new Vector3(limit.offSet.x, height, limit.offSet.y);
        }
        private void ScalePointCorrection()
        {
            //Pour ChaqueZone
            if (editZones.Count != 0)
            {
                for (int i = 0; i < editZones.Count; i++)
                {
                    //Pour ChaquePoint
                    if (editZones[i].points.Count != 0)
                    {
                        for (int j = 0; j < editZones[i].points.Count; j++)
                        {
                            editZones[i].points[j].position = new Vector3(
                                editZones[i].points[j].position.x * (limit.size.x / oldLimit.size.x),
                                editZones[i].points[j].position.y,
                                editZones[i].points[j].position.z * (limit.size.y / oldLimit.size.y)
                                );
                        }
                    }
                }
            }
        }
        public Environnement NewEnvironnement()
        {
            Environnement enviro = new Environnement();

            //Change Map Limit
            enviro.limit = limit;

            if (editZones.Count != 0)
            {
                //Create Each Zone storage space
                enviro.zones = new Zone[editZones.Count];
                //For each Zone
                for (int i = 0; i < editZones.Count; i++)
                {
                    //Zone Info
                    enviro.zones[i].name = editZones[i].name;
                    enviro.zones[i].windDir = editZones[i].windDir;
                    enviro.zones[i].debugColor = editZones[i].color;

                    if (editZones[i].points.Count != 0)
                    {
                        //Create Points storage space
                        enviro.zones[i].points = new Vector2[editZones[i].points.Count];

                        for (int j = 0; j < editZones[i].points.Count; j++)
                        {
                            enviro.zones[i].points[j] = new Vector2(editZones[i].points[j].position.x, editZones[i].points[j].position.z);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Il n'y a pas de point dans la Zone" + i);
                    }
                }
            }
            else
            {
                Debug.LogError("Il n'y a pas de zone");
            }

            if (this.enviro != null)
            {
                this.enviro = enviro;
            }

            return enviro;
        }
        #endregion

        #region Debug/Gizmo méthodes
        private void DrawBoundary(int step)
        {
            step = step >= 1 ? step : 1;


            if (debugOption.showHardBorder)
            {
                Gizmos.color = debugOption.borderColor;

                //Top
                Gizmos.DrawCube(new Vector3(transform.position.x, 0, transform.position.z + (0.5f * limit.size.y) + (0.5f * step)), new Vector3(limit.size.x + (2 * step), 0, step));
                //Bottom
                Gizmos.DrawCube(new Vector3(transform.position.x, 0, transform.position.z - (0.5f * limit.size.y) - (0.5f * step)), new Vector3(limit.size.x + (2 * step), 0, step));
                //Left
                Gizmos.DrawCube(new Vector3(transform.position.x - (0.5f * limit.size.x) - (0.5f * step), 0, transform.position.z), new Vector3(step, 0, limit.size.y));
                //Right
                Gizmos.DrawCube(new Vector3(transform.position.x + (0.5f * limit.size.x) + (0.5f * step), 0, transform.position.z), new Vector3(step, 0, limit.size.y));

            }

            if (debugOption.showLineBorder)
            {
                for (float tempExtrude = 0; tempExtrude <= step; tempExtrude++)
                {
                    //Draw the rectangle
                    // _  
                    Vector3 drawPos = new Vector3(limit.leftBorder - tempExtrude, transform.position.y + tempExtrude * 0.0f, limit.upBorder + tempExtrude);
                    Debug.DrawRay(drawPos, Vector3.right * limit.size.x + Vector3.right * 2 * tempExtrude, debugOption.borderColor);
                    // _
                    //  |
                    drawPos += Vector3.right * limit.size.x + Vector3.right * 2 * tempExtrude;
                    Debug.DrawRay(drawPos, Vector3.back * limit.size.y + Vector3.back * 2 * tempExtrude, debugOption.borderColor);
                    // _
                    // _|
                    drawPos += Vector3.back * limit.size.y + Vector3.back * 2 * tempExtrude;
                    Debug.DrawRay(drawPos, Vector3.left * limit.size.x + Vector3.left * 2 * tempExtrude, debugOption.borderColor);
                    // _
                    //|_|
                    drawPos += Vector3.left * limit.size.x + Vector3.left * 2 * tempExtrude;
                    Debug.DrawRay(drawPos, Vector3.forward * limit.size.y + Vector3.forward * 2 * tempExtrude, debugOption.borderColor);
                }
            }
        }
        private void DrawZone(ZoneEditable zone)
        {
            if (zone.points.Count != 0)
            {
                Gizmos.color = zone.color;

                Gizmos.DrawLine(zone.points[zone.points.Count - 1].position, zone.points[0].position);
                if (zone.drawWind)
                {
                    DrawWind(zone.points[zone.points.Count - 1].position, zone.windDir, zone);
                }
                for (int i = 0; i < zone.points.Count - 1; i++)
                {
                    Gizmos.DrawLine(zone.points[i].position, zone.points[i + 1].position);
                    if (zone.drawWind)
                    {
                        DrawWind(zone.points[i].position, zone.windDir, zone);
                    }
                }

                //Ce serait cool de générer un mesh et le draw
                //Gizmos.DrawMesh
            }
        }
        private void DrawWind(Vector3 point, float windDot, ZoneEditable zone)
        {
            Gizmos.color = Color.white;

            Gizmos.DrawRay(point, Quaternion.Euler(new Vector3(0, windDot * 180, 0)) * Vector3.forward * 5f );

            Gizmos.color = zone.color;
        }
        private void RemoveDeletePointFromZone(ZoneEditable zone)
        {
            //Pour chaque points d'une zone
            if (zone.points.Count != 0)
            {
                for (int j = 0; j < zone.points.Count; j++)
                {
                    //S'il n'est plus là
                    if (zone.points[j] == null)
                    {
                        zone.points.RemoveAt(j);
                    }

                }
            }
        }
        #endregion
    }

    [System.Serializable]
    public class Tool_LD_DebugOption
    {
        public bool showDebug = true;
        public bool showHardBorder = true;
        public bool showLineBorder = true;
        public int borderThickness = 4;
        public Color borderColor = Color.red;
    }
#endif
}
