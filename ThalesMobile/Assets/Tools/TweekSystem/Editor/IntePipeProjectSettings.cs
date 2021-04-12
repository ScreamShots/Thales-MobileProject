using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// S'il y a un problème, c'est la faute de Karp
/// </summary>
static class IntePipeProjectSettings
{
    [SettingsProvider]
    public static SettingsProvider CreateThalesSettingsProvider()
    {
        float valuesGap = 5; //in pixel
        float categoriesGap = 15; //in pixel

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
                        TweekCore.scenesPath = GUILayout.TextField(TweekCore.scenesPath, EditorStyles.textArea);
                    }
                    GUILayout.Space(valuesGap);
                    
                    //Prefab
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("PrefabFolderPath", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        TweekCore.prefabsPath = GUILayout.TextField(TweekCore.prefabsPath, EditorStyles.textArea);
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
                        TweekCore.soundAssetsDirectory = GUILayout.TextField(TweekCore.soundAssetsDirectory, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Graphic
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("FolderPath_GraphicAssets", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        TweekCore.graphicAssetsDirectory = GUILayout.TextField(TweekCore.graphicAssetsDirectory, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Gameplay
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("FolderPath_GameplayAssets", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        TweekCore.gameplayAssetsDirectory = GUILayout.TextField(TweekCore.gameplayAssetsDirectory, EditorStyles.textField);
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
                        TweekCore.soundScoPath = GUILayout.TextField(TweekCore.soundScoPath, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Graphic
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("ScoScriptPath_Graphic", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        TweekCore.graphicScoPath = GUILayout.TextField(TweekCore.graphicScoPath, EditorStyles.textField);
                    }
                    GUILayout.Space(valuesGap);

                    //Gameplay
                    using (new GUILayout.HorizontalScope())
                    {
                        //Nom
                        GUILayout.Label("ScoScriptPath_Gameplay", EditorStyles.boldLabel, GUILayout.Width(160));
                        //Champs
                        TweekCore.gameplayScoPath = GUILayout.TextField(TweekCore.gameplayScoPath, EditorStyles.textField);
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