#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Karprod;

namespace Karprod
{
    public class ToolPlaneMeshGenerator : EditorWindow
    {
        [ContextMenu("Tools/PlaneMeshGenerator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ToolPlaneMeshGenerator));
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            GUILayout.Label("Texture Ramp", EditorStyles.boldLabel);
            //textureName = EditorGUILayout.TextField("Name", textureName);
            //gradient = EditorGUILayout.GradientField("Gradient", gradient);

            GUILayout.Space(15);
            GUILayout.Label("Texture Size", EditorStyles.boldLabel);
            //xSize = EditorGUILayout.IntField("Lenght", xSize);
            //ySize = EditorGUILayout.IntField("Height", ySize);

            GUILayout.Space(5);
            GUILayout.Label("Ex : Assets/Folder/SubFolder/...", EditorStyles.boldLabel);
            //filePath = EditorGUILayout.TextField("File Path", filePath);

            GUILayout.Space(15);
            if (GUILayout.Button("Generate Texture Ramp"))
            {
                //CreateGradient();
            }
        }
    }
}
#endif
