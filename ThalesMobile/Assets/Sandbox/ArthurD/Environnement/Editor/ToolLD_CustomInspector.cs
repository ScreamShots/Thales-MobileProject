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
            GUILayout.Space(10);

            Tool_LevelDesign tool = (Tool_LevelDesign)target;

            #region Environnement
            using (new GUILayout.VerticalScope())
            {
                //Selection
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Enviro", EditorStyles.boldLabel);
                    tool.enviro = (Environnement)EditorGUILayout.ObjectField(tool.enviro, typeof(Environnement), true);
                }
                GUILayout.Space(3);
                //Button methode
                using (new GUILayout.HorizontalScope())
                {
                    float h = 40;
                    if (GUILayout.Button("Push Data in Enviro",GUILayout.Height(h)))
                    {
                        tool.PushIntoEnvironnement();
                    }
                    if (GUILayout.Button("Generate a new Enviro", GUILayout.Height(h)))
                    {
                        tool.GenerateNewEnvironnement();
                    }
                }
            }
            #endregion

            #region Boundary
            GUILayout.Space(10);

            //Boundary
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("Environnement Delimitation", EditorStyles.boldLabel);
                //Size
                using (new GUILayout.HorizontalScope())
                {
                    tool.limit.size.x = EditorGUILayout.IntSlider((int)tool.limit.size.x, 32, 128);
                    tool.limit.size.y = EditorGUILayout.IntSlider((int)tool.limit.size.y, 32, 128);
                    //tool.limit.size = EditorGUILayout.Vector2Field("Size", tool.limit.size);
                }
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
            }
            #endregion

            base.OnInspectorGUI();

            GUILayout.Space(25);
            //Environnement
            using (new GUILayout.VerticalScope())
            {
                //Add Point
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Add Point In Zone"))
                    {
                        tool.AddPoint();
                    }
                    #region Zone Cible Number
                    if (GUILayout.Button("<", GUILayout.Width(20)))
                    {
                        tool.zoneNbr = tool.zoneNbr > 0 ? tool.zoneNbr - 1 : 0;
                    }
                    tool.zoneNbr = EditorGUILayout.IntField(tool.zoneNbr, GUILayout.Width(20));
                    if (GUILayout.Button(">", GUILayout.Width(20)))
                    {
                        tool.zoneNbr = tool.zoneNbr < tool.editZones.Count - 1 ? tool.zoneNbr + 1 : tool.editZones.Count - 1;
                    }
                    #endregion
                }
                //Add Zone
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Add Zone In Environnement"))
                    {
                        tool.AddZone();
                    }
                    if (GUILayout.Button(" ", EditorStyles.helpBox, GUILayout.Width(66)))
                    {

                    }
                }
                GUILayout.Space(10);
                //Button methode
                using (new GUILayout.VerticalScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Push Data in Enviro", GUILayout.Height(20)))
                        {
                            tool.PushIntoEnvironnement();
                        }
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Generate EnviroColor Texture", GUILayout.Height(20)))
                        {
                            tool.GenerateTextureColor();
                        }
                        if (GUILayout.Button("Generate EnviroData Texture", GUILayout.Height(20)))
                        {
                            
                        }
                    }
                }
            }

            EditorUtility.SetDirty(target);

        }
    }
}
