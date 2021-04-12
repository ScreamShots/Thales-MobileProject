using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MeshToolProjectSettings
{
    private static MeshToolSettingsData data;

    [SettingsProvider]
    public static SettingsProvider CreateThalesSettingsProvider()
    {
        data = MeshToolSettingsData.GetOrCreateSettings();

        float valuesGap = 5; //in pixel
        float categoriesGap = 15; //in pixel

        // (path in Settings window, scope of setting [User or Project])
        var provider = new SettingsProvider("_Thales/MeshTool", SettingsScope.Project)
        {
            // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
            guiHandler = (searchContext) =>
            {
                //Saving Path
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("Saving Path", EditorStyles.whiteLargeLabel);
                    GUILayout.Space(valuesGap);

                    //Scene
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("Mesh Saving Path", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.meshSavingPath = GUILayout.TextField(data.meshSavingPath);
                    }
                    GUILayout.Space(valuesGap);
                }
                GUILayout.Space(categoriesGap);

            },

            // Search keywords for people using the search bar (how are you ?)
            keywords = new HashSet<string>(new[] { "Thales", "Integration", "SCO" })
        };
        return provider;
    }
}

class MeshToolSettingsData : ScriptableObject
{
    public const string meshToolSettingsDataPath = "Assets/Sandbox/ThomasD/MeshToolSettingsData.asset";

    [SerializeField]
    public string meshSavingPath;

    internal static MeshToolSettingsData GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<MeshToolSettingsData>(meshToolSettingsDataPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<MeshToolSettingsData>();
            settings.meshSavingPath = "Assets/";
            AssetDatabase.CreateAsset(settings, meshToolSettingsDataPath);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }
}