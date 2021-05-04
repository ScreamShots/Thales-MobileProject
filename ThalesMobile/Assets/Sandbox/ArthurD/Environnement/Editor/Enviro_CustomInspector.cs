using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Environnement))]
public class Enviro_CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Environnement enviro = (Environnement)target;

        base.OnInspectorGUI();

        GUILayout.Space(20);

        //Add Point
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Generate Point Of Zone"))
            {
                enviro.GeneratePointOfZone();
            }
            #region Zone Cible Number
            //Left or Previous
            if (GUILayout.Button("<", GUILayout.Width(20)))
            {
                enviro.zoneAim = enviro.zoneAim > 0 ? enviro.zoneAim - 1 : 0;
            }

            //Data
            enviro.zoneAim = EditorGUILayout.IntField(enviro.zoneAim, GUILayout.Width(20));
            
            //Right or Next
            if (GUILayout.Button(">", GUILayout.Width(20)))
            {
                enviro.zoneAim = enviro.zoneAim < enviro.zones.Length - 1 ? enviro.zoneAim + 1 : enviro.zones.Length - 1;
            }
            #endregion
        }

        GUILayout.Space(20);


        //reconstructZone
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Reconstruct Zone n°"))
            {
                enviro.GeneratePointOfZone();
            }
            #region Zone Cible Number
            //Left or Previous
            if (GUILayout.Button("<", GUILayout.Width(20)))
            {
                enviro.zoneAim = enviro.zoneAim > 0 ? enviro.zoneAim - 1 : 0;
            }

            //Data
            enviro.zoneAim = EditorGUILayout.IntField(enviro.zoneAim, GUILayout.Width(20));

            //Right or Next
            if (GUILayout.Button(">", GUILayout.Width(20)))
            {
                enviro.zoneAim = enviro.zoneAim < enviro.zones.Length - 1 ? enviro.zoneAim + 1 : enviro.zones.Length - 1;
            }
            #endregion

        }
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Line Renderer For all"))
            {
                enviro.LineRenderSetUp();
            }
        }
        //Line Renderer
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Line Renderer Of Zone n°"))
            {
                enviro.ZoneLineRender(enviro.zoneAim);
            }
            if (GUILayout.Button(" ", EditorStyles.helpBox, GUILayout.Width(66)))
            {
                if (GUILayout.Button("<", GUILayout.Width(20)))
                {
                    enviro.zoneAim = enviro.zoneAim > 0 ? enviro.zoneAim - 1 : 0;
                }

                //Data
                enviro.zoneAim = EditorGUILayout.IntField(enviro.zoneAim, GUILayout.Width(20));

                //Right or Next
                if (GUILayout.Button(">", GUILayout.Width(20)))
                {
                    enviro.zoneAim = enviro.zoneAim < enviro.zones.Length - 1 ? enviro.zoneAim + 1 : enviro.zones.Length - 1;
                }
            }
        }
        GUILayout.Space(20);

        //Environnement
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Generate EnviroColor Texture", GUILayout.Height(40)))
            {
                enviro.GenerateTextureColor();
            }
            if (GUILayout.Button("Generate EnviroData Texture", GUILayout.Height(40)))
            {
                enviro.GenerateTextureData();
            }
        }
    }
}


