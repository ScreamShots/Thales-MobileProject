using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// S'il y a un problème, c'est la faute de Karp
/// </summary>
static class IntePipeProjectSettings
{
    private static IntePipeSettingsData data;

    [SettingsProvider]
    public static SettingsProvider CreateThalesSettingsProvider()
    {
        float valuesGap = 5; //in pixel
        float categoriesGap = 15; //in pixel

        data = IntePipeSettingsData.GetOrCreateSettings();

        // (path in Settings window, scope of setting [User or Project])
        var provider = new SettingsProvider("_Thales/IntegrationPipeline", SettingsScope.Project)
        {
            // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
            guiHandler = (searchContext) =>
            {
                //Scene et Prefab Path
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("Folder Path for Scene & Prefab", EditorStyles.whiteLargeLabel);
                    GUILayout.Space(valuesGap);

                    //Scene
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("ScenesFolderPath", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.scenesPath = GUILayout.TextField(data.scenesPath, EditorStyles.textArea);
                    }
                    GUILayout.Space(valuesGap);
                    
                    //Prefab
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("PrefabFolderPath", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.prefabsPath = GUILayout.TextField(data.prefabsPath, EditorStyles.textArea);
                    }
                    GUILayout.Space(valuesGap);
                }
                GUILayout.Space(categoriesGap);

                //AssetsDirectory
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("Folder Path for Graphic, Sound and Gameplay assets", EditorStyles.whiteLargeLabel);
                    GUILayout.Space(valuesGap);

                    //Sound
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("FolderPath_SoundAssets", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.soundAssetsDirectory = GUILayout.TextField(data.soundAssetsDirectory, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Graphic
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("FolderPath_GraphicAssets", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.graphicAssetsDirectory = GUILayout.TextField(data.graphicAssetsDirectory, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Gameplay
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("FolderPath_GameplayAssets", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.gameplayAssetsDirectory = GUILayout.TextField(data.gameplayAssetsDirectory, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);
                }
                GUILayout.Space(categoriesGap);

                //ScriptPath_SCO
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("Folder Path for Graphic, Sound and Gameplay SCO script", EditorStyles.whiteLargeLabel);
                    GUILayout.Space(valuesGap);

                    //Sound
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("ScoScriptPath_Sound", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.soundScoPath = GUILayout.TextField(data.soundScoPath, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Graphic
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("ScoScriptPath_Graphic", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.graphicScoPath = GUILayout.TextField(data.graphicScoPath, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Gameplay
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("ScoScriptPath_Gameplay", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        data.gameplayScoPath = GUILayout.TextField(data.gameplayScoPath, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);
                }
            },

            // Search keywords for people using the search bar (how are you ?)
            keywords = new HashSet<string>(new[] { "Thales", "Integration", "SCO" })
        };
        return provider;
    }
}

class IntePipeSettingsData : ScriptableObject
{
    public const string meshToolSettingsDataPath = "Assets/Tools/Internal/TweekSystem/Editor/IntegrationPipeSettingsData.asset";

    public string scenesPath;
    public string prefabsPath;

    public string soundScoPath;
    public string graphicScoPath;
    public string gameplayScoPath;

    public string soundAssetsDirectory;
    public string graphicAssetsDirectory;
    public string gameplayAssetsDirectory;

    internal static IntePipeSettingsData GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<IntePipeSettingsData>(meshToolSettingsDataPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<IntePipeSettingsData>();
            settings.scenesPath = "Assets/_PaslesScenesDuTout";
            settings.prefabsPath = "Assets/Prefabs";

            settings.soundScoPath = "Assets/Tools/Internal/TweekSystem/SoundTweekScriptableObject.cs";
            settings.graphicScoPath = "Assets/Tools/Internal/TweekSystem/ArtTweekScriptableObject.cs";
            settings.gameplayScoPath = "Assets/Tools/Internal/TweekSystem/GameplayTweekScriptableObject.cs";

            settings.soundAssetsDirectory = "Assets/IntegrationSCO/Sound";
            settings.graphicAssetsDirectory = "Assets/IntegrationSCO/Art";
            settings.gameplayAssetsDirectory = "Assets/IntegrationSCO/Gameplay"; 
            
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