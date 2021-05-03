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


