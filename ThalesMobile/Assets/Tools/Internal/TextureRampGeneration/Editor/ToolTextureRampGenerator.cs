using UnityEngine;
using UnityEditor;

namespace Karprod
{
    public class ToolTextureRampGenerator : EditorWindow
    {
        private string textureName = "new_TextureRamp";

        #region defaultGradient
        private static readonly GradientColorKey[] defaultGradient =  new GradientColorKey[5] 
            {
                new GradientColorKey(Color.red, 0),
                new GradientColorKey(Color.yellow, 0.25f),
                new GradientColorKey(Color.green, 0.5f),
                new GradientColorKey(Color.cyan, 0.75f),
                new GradientColorKey(Color.blue, 1)
            };
        #endregion
        private Gradient gradient = new Gradient() {colorKeys = defaultGradient};

        private bool isSquare = false;
        private int size = 256;

        private int xSize = 256;
        private int ySize = 256;

        [MenuItem("Tools/TextureRampGenerator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ToolTextureRampGenerator));
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            GUILayout.Label("Texture Ramp", EditorStyles.boldLabel);
            textureName = EditorGUILayout.TextField("Name", textureName);
            gradient = EditorGUILayout.GradientField("Gradient", gradient);

            GUILayout.Space(15);
            GUILayout.Label("Texture Size", EditorStyles.boldLabel);
            isSquare = EditorGUILayout.Toggle("Texture is Square",isSquare);
            GUILayout.Space(5);
            if (isSquare)
            {
                size = EditorGUILayout.IntField("Size", size);
            }
            else
            {
                xSize = EditorGUILayout.IntField("Lenght", xSize);
                ySize = EditorGUILayout.IntField("Height", ySize);
            }

            GUILayout.Space(15);
            if (GUILayout.Button("Generate Texture Ramp"))
            {
                CreateGradient();
            }
        }

        private void CreateGradient()
        {
            if (isSquare)
            {
                TextureRamp.Create(gradient, textureName, size);
            }
            else
            {
                TextureRamp.Create(gradient, textureName, xSize, ySize);
            }
        }
    }
}
