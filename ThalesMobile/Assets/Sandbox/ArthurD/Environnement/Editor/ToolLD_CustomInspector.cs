using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Thales.Tool.LevelDesign
{
    [CustomEditor(typeof(Tool_LevelDesign))]
    public class ToolLD_CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            Tool_LevelDesign tool = (Tool_LevelDesign)target;

            //Environnement
            using (new GUILayout.VerticalScope())
            {
                //Selection
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Enviro", EditorStyles.boldLabel);
                    tool.enviro = (Environnement)EditorGUILayout.ObjectField(tool.enviro, typeof(Environnement), true);
                }
                //Button methode
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Push Data in Enviro"))
                    {

                    }
                    if (GUILayout.Button("Generate a new Enviro"))
                    {

                    }
                }
            }

            GUILayout.Space(10);
            /*
            //Boundary
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("Environnement Delimitation", EditorStyles.boldLabel);
                //Size
                tool.limit.size = EditorGUILayout.Vector2Field("Size", tool.limit.size);
                //OffSet
                using (new GUILayout.HorizontalScope())
                {
                    Vector3 decal = new Vector3(tool.limit.offSet.x, tool.limit.offSet.y, tool.height);
                    decal = EditorGUILayout.Vector3Field("OffSet", decal);
                    tool.limit.offSet = new Vector2(decal.x, decal.y);
                    tool.height = decal.z;
                    
                    //tool.limit.offSet = EditorGUILayout.Vector2Field("OffSet", tool.limit.offSet);
                    //tool.height = EditorGUILayout.FloatField(new GUIContent("height"),tool.height);
                    
                }
                //Color
                tool.limitColor = EditorGUILayout.ColorField("Color", tool.limitColor);
            }
            */

            GUILayout.Space(10);
            GUILayout.Label("Classique Draw");
            base.OnInspectorGUI();

            GUILayout.Space(10);
            //Environnement
            using (new GUILayout.VerticalScope())
            {
                //Selection
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Add Point In Zone"))
                    {

                    }

                    if (GUILayout.Button("<", GUILayout.Width(20)))
                    {
                        tool.zoneNbr = tool.zoneNbr > 0 ? tool.zoneNbr - 1 : 0;
                    }
                    tool.zoneNbr = EditorGUILayout.IntField(tool.zoneNbr, GUILayout.Width(20));
                    if (GUILayout.Button(">", GUILayout.Width(20)))
                    {
                        tool.zoneNbr = tool.zoneNbr < tool.editZones.Count ? tool.zoneNbr + 1 : tool.editZones.Count;
                    }

                }
                //Button methode
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Push Data in Enviro"))
                    {

                    }
                    if (GUILayout.Button("Generate a new Enviro"))
                    {

                    }
                }
            }

        }
    }
}
