using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using Tweek.ScoAttributes;
using Tweek.FlagAttributes;
using Tweek.System;
using UnityEditor.Compilation;

public class TweekCoreUtilities : UnityEngine.Object
{
    public static string scenesPath = "Assets/_Scenes";
    public static string prefabsPath = "Assets/Prefabs";

    public static string soundScoPath = "Assets/Tools/TweekSystem/SoundTweekScriptableObject.cs";
    public static string graphicScoPath = "Assets/Tools/TweekSystem/ArtTweekScriptableObject.cs";
    public static string gameplayScoPath = "Assets/Tools/TweekSystem/GameplayTweekScriptableObject.cs";

    public static string soundAssetsDirectory = "Assets/IntegrationSCO/Sound";
    public static string graphicAssetsDirectory = "Assets/IntegrationSCO/Art";
    public static string gameplayAssetsDirectory = "Assets/IntegrationSCO/Gameplay";

    public enum ScoUpdateMode { Global, Gameplay, Art, Sound }

    [MenuItem("Tools/Show static info")]
    public static void DebugStatic()
    {
        Debug.Log(gameplayScoPath);
        Debug.Log(graphicScoPath);
        Debug.Log(soundScoPath);
        Debug.Log(string.Empty);
        Debug.Log(gameplayAssetsDirectory);
        Debug.Log(graphicAssetsDirectory);
        Debug.Log(soundAssetsDirectory);
        Debug.Log(string.Empty);
        Debug.Log(soundScoPath);
        Debug.Log(graphicScoPath);
        Debug.Log(gameplayScoPath);
    }


    #region Editor Commands
    /*
    [MenuItem("Tweek Operations/Update SCO Classes/project data -> All SCO ")]
    public static void UpdateAllScoClass()
    {
        LaunchScoUpdate(ScoUpdateMode.Global);
    }

    [MenuItem("Tweek Operations/Update SCO Classes/project Gameplay data-> Gameplay SCO")]
    public static void UpdateGameplayScoClass()
    {
        LaunchScoUpdate(ScoUpdateMode.Gameplay);
    }

    [MenuItem("Tweek Operations/Update SCO Classes/project Graphic data -> Graphic SCO")]
    public static void UpdateGraphicScoClass()
    {
        LaunchScoUpdate(ScoUpdateMode.Art);
    }

    [MenuItem("Tweek Operations/Update SCO Classes/project Sound data -> Sound SCO")]
    public static void UpdateSoundScoClass()
    {
        LaunchScoUpdate(ScoUpdateMode.Sound);
    }
    */
    #endregion

    /// <summary>
    /// Update SCO Classes with flagged field across the project (overide file or create one if not already existing)
    /// </summary>

    //manage the SCO update protocole (handle progress bar)
    public static void LaunchScoUpdate(ScoUpdateMode updateMode)
    {
        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Start " + updateMode.ToString() + " update operation", 0f);

        Dictionary<string, List<TweekObj>> sceneObjs;
        Dictionary<string, List<TweekObj>> prefabObjs;

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Referencing values from scene's objects", 0.20f);

        sceneObjs = GetObjectsFromScenes();

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Referencing values from prefabs", 0.40f);

        prefabObjs = GetObjectsFromPrefabs();

        if (File.Exists("Assets/Tools/TweekSystem/.tempChecker.txt")) File.Delete("Assets/Tools/TweekSystem/.tempChecker.txt");

        FileStream fs = File.Open("Assets/Tools/TweekSystem/.tempChecker.txt", FileMode.Create);
        using (StreamWriter outfile = new StreamWriter(fs))
        {
            outfile.WriteLine("temp file");
        }

        if (updateMode == ScoUpdateMode.Global)
        {
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing Gameplay SCO Class", 0.50f);
            WriteScoClass(sceneObjs, prefabObjs, ScoUpdateMode.Gameplay);
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing Art SCO Class", 0.60f);
            WriteScoClass(sceneObjs, prefabObjs, ScoUpdateMode.Art);
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing Sound SCO Class", 0.70f);
            WriteScoClass(sceneObjs, prefabObjs, ScoUpdateMode.Sound);
        }
        else
        {
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing" + updateMode.ToString() + "SCO Class", 0.60f);
            WriteScoClass(sceneObjs, prefabObjs, updateMode);
        }

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Compiling", 0.80f);
        AssetDatabase.Refresh();
        CompilationPipeline.RequestScriptCompilation();
    }

    //find all flaged components on scene's obj
    static Dictionary<string, List<TweekObj>> GetObjectsFromScenes()
    {
        Dictionary<string, List<TweekObj>> objectsFromScenes = new Dictionary<string, List<TweekObj>>();
        TweekObj tempTObj = new TweekObj();
        TweekComponent tempTComp = new TweekComponent();
        List<TweekField> tempTFields = new List<TweekField>();

        //store openned scene at the start so we can load it back at the end
        string originScene = EditorSceneManager.GetActiveScene().path;

        //ask user if he wants to save the scenes if modifications are detected
        if (EditorSceneManager.GetActiveScene().isDirty) EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        //get all scenes' paths in the referenced directory and rewrite them so they are Unity formated with / and no \
        string[] allScenesPaths = Directory.GetFiles(scenesPath);
        for (int i = 0; i < allScenesPaths.Length; i++) allScenesPaths[i] = PathWritter(allScenesPaths[i]);

        //going through each scenes in the directory
        foreach (string path in allScenesPaths)
        {
            if (Path.GetExtension(path) == ".unity" || Path.GetExtension(path) == ".UNITY")
            {
                EditorSceneManager.OpenScene(path);

                //find every component inherited from TweekMonoBehaviour
                object[] allCompInScene = FindObjectsOfType(typeof(TweekMonoBehaviour));
                //if their is components in the scene add a entry to the dictionary for this scenes
                if (allCompInScene.Length != 0) objectsFromScenes.Add(path, new List<TweekObj>());
                //if their is no components available in the scenes go to next scene
                else continue;

                //going through each finded component
                foreach (TweekMonoBehaviour comp in allCompInScene)
                {
                    //test if component is configured to be affected by value's modification
                    if (comp.valuesTweekState == TweekMonoBehaviour.TweekState.ProperValues)
                    {
                        bool existingObj = false;

                        //create a support TweekComponent to store fields from current processed component
                        tempTComp = new TweekComponent(NameWritter(comp.GetType().ToString()), comp.serializedGuid);

                        tempTFields = FindFields(comp);
                        foreach(TweekField field in tempTFields)
                        {
                            tempTComp.AddField(field);
                        }

                        //loop through already created TweekObj to verify if one is already created for the go of actual processed component
                        foreach (TweekObj obj in objectsFromScenes[path])
                        {
                            if (obj.tempID == comp.gameObject.GetInstanceID())
                            {
                                obj.AddComp(tempTComp);
                                existingObj = true;
                                break;
                            }
                        }

                        //if no existing TweekObj was founded, create a new one
                        if (!existingObj)
                        {
                            tempTObj = new TweekObj(NameWritter(comp.gameObject.name), path, comp.gameObject.GetInstanceID());
                            tempTObj.AddComp(tempTComp);
                            objectsFromScenes[path].Add(tempTObj);
                        }
                    }
                }
            }
        }

        EditorSceneManager.OpenScene(originScene);
        return objectsFromScenes;
    }

    //fin all flaged components on prefab
    static Dictionary<string, List<TweekObj>> GetObjectsFromPrefabs()
    {
        Dictionary<string, List<TweekObj>> objectsFromPrefabs = new Dictionary<string, List<TweekObj>>();

        TweekObj tempTObj = new TweekObj();
        TweekComponent tempTComp = new TweekComponent();
        List<TweekField> tempTFields = new List<TweekField>(); 
        GameObject tempPrefab;
        object[] allCompOnPrefab;

        string[][] allPrefabsPaths = new string[0][];
        string[] subDirectoriesPaths = new string[0];
        string indexPath;

        //test if their is sub directories to the main path referenced
        if (Directory.GetDirectories(prefabsPath).Length > 0)
        {
            //store all subdirectories path
            subDirectoriesPaths = Directory.GetDirectories(prefabsPath);
            for (int i = 0; i < subDirectoriesPaths.Length; i++) subDirectoriesPaths[i] = PathWritter(subDirectoriesPaths[i]);

            //store all file in all subdirectories and on main directories in a jagged array (linking directoriy path to files)
            allPrefabsPaths = new string[subDirectoriesPaths.Length + 1][];
            allPrefabsPaths[0] = Directory.GetFiles(prefabsPath);
            for (int i = 1; i < allPrefabsPaths.Length; i++) allPrefabsPaths[i] = Directory.GetFiles(prefabsPath + "/" + subDirectoriesPaths[i - 1]);
            for (int i = 0; i < allPrefabsPaths.Length; i++) for (int z = 0; z < allPrefabsPaths[i].Length; z++) allPrefabsPaths[i][z] = PathWritter(allPrefabsPaths[i][z]);
        }
        else
        {
            //if no subdirectories
            allPrefabsPaths = new string[1][];
            allPrefabsPaths[0] = Directory.GetFiles(prefabsPath);
            for (int i = 0; i < allPrefabsPaths[0].Length; i++) allPrefabsPaths[0][i] = PathWritter(allPrefabsPaths[0][i]);
        }

        //tell to all components inherited from TweekMonoBehaviour that we are checking to prefab
        //it by-pass somme checks at awake that are not working for prefab assets 
        TweekMonoBehaviour.prefabEdition = true;

        //loop through all directories (if no subdirectories, only 1 iteration)
        for (int x = 0; x < allPrefabsPaths.Length; x++)
        {
            //create new entry in the dictionary for all prefabs in current directory
            if (x == 0) indexPath = PathWritter(prefabsPath);
            else indexPath = PathWritter(prefabsPath + "/" + subDirectoriesPaths[x - 1]);
            objectsFromPrefabs.Add(indexPath, new List<TweekObj>());

            //loop through all prefab assets in the current directory
            for (int y = 0; y < allPrefabsPaths[x].Length; y++)
            {
                if (Path.GetExtension(allPrefabsPaths[x][y]) == ".prefab" || Path.GetExtension(allPrefabsPaths[x][y]) == ".PREFAB")
                {
                    //Load prefab and extract all components inherited from TweekMonoBehaviour
                    tempPrefab = PrefabUtility.LoadPrefabContents(allPrefabsPaths[x][y]);
                    allCompOnPrefab = tempPrefab.GetComponents<TweekMonoBehaviour>();

                    //if their is components inhertied from TweekMonoBehaviour create a TweekObj matching current processed prefab
                    if (allCompOnPrefab.Length > 0) tempTObj = new TweekObj(NameWritter(tempPrefab.name), indexPath);
                    //if no components inhertied from TweekMonoBehaviour finded go to next prefab
                    else continue;

                    //loop through all componentes inherited from TweekMonoBehaviour on current processed prefab
                    foreach (TweekMonoBehaviour comp in allCompOnPrefab)
                    {
                        comp.GuidInit();
                        tempTComp = new TweekComponent(NameWritter(comp.GetType().ToString()), comp.serializedGuid);

                        tempTFields = FindFields(comp);
                        foreach (TweekField field in tempTFields)
                        {
                            tempTComp.AddField(field);
                        }

                        tempTObj.AddComp(tempTComp);
                    }

                    objectsFromPrefabs[indexPath].Add(tempTObj);
                    PrefabUtility.SaveAsPrefabAsset(tempPrefab, allPrefabsPaths[x][y]);
                    PrefabUtility.UnloadPrefabContents(tempPrefab);
                }
            }
        }
        return objectsFromPrefabs;
    }

    //extract flagged fields from a given component
    static List<TweekField> FindFields(TweekMonoBehaviour comp)
    {
        List<TweekField> fields = new List<TweekField>();
        Type compType = comp.GetType();
        BindingFlags flags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        FieldInfo[] compFields = compType.GetFields(flags);

        foreach (FieldInfo field in compFields)
        {
            if (field.GetCustomAttributes().ToArray().Length > 0)
            {
                TweekFlagAttribute attribute = field.GetCustomAttribute<TweekFlagAttribute>();
                if (attribute != null)
                {
                    fields.Add(new TweekField(field.FieldType, field.Name, attribute.fieldUsage));
                }
            }
        }
        return fields;
    }

    //write a Sco class matching reasearched type of fields (Art, Gameplay, Sound)
    static void WriteScoClass(Dictionary<string, List<TweekObj>> scenesObjs, Dictionary<string, List<TweekObj>> prefabObjs, ScoUpdateMode updateMode)
    {
        string targetScoPath = string.Empty;
        string currentKey = string.Empty;

        TweekObj tempObj;
        TweekComponent tempComp;
        TweekField tempField;

        AttributeConstructor[] tempAttributes;
        int separatorCount = 0;

        bool writeThis = false;

        switch (updateMode)
        {
            case ScoUpdateMode.Art:
                targetScoPath = graphicScoPath;
                break;
            case ScoUpdateMode.Gameplay:
                targetScoPath = gameplayScoPath;
                break;
            case ScoUpdateMode.Sound:
                targetScoPath = soundScoPath;
                break;
        }

        FileStream fs = File.Open(targetScoPath, FileMode.Create);
        using (StreamWriter outfile = new StreamWriter(fs))
        {
            #region Class Construction
            outfile.WriteLine("//Random Generatated char value: " + Guid.NewGuid().ToString());
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using Tweek.ScoAttributes;");
            outfile.WriteLine("");
            outfile.WriteLine("[CreateAssetMenu(menuName =\"Tweek/" + updateMode.ToString() + " Asset\")]");
            outfile.WriteLine("public class " + updateMode.ToString() + "TweekScriptableObject : ScriptableObject");
            outfile.WriteLine("{");
            #endregion

            #region Write For Object From Scenes
            //outfile.WriteLine("[Header(\"Objects From Scenes\")]");
            outfile.WriteLine("");

            for (int a = 0; a < scenesObjs.Count; a++)
            {
                currentKey = scenesObjs.ElementAt(a).Key;

                foreach(TweekObj obj in scenesObjs[currentKey])
                {
                    switch (updateMode)
                    {
                        case ScoUpdateMode.Global:
                            writeThis = true;
                            break;
                        case ScoUpdateMode.Gameplay:
                            if (obj.gameplayFields) writeThis = true;
                            break;
                        case ScoUpdateMode.Art:
                            if (obj.artFields) writeThis = true;
                            break;
                        case ScoUpdateMode.Sound:
                            if (obj.soundFields) writeThis = true;
                            break;
                    }
                }

                if (writeThis)
                {
                    outfile.WriteLine("[Space]");
                    tempAttributes = new AttributeConstructor[] { new AttributeConstructor(SupportedAttributes.Path, "From") };
                    outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "_From", tempAttributes, "\"" + currentKey + "\""));
                    separatorCount += 1;
                    outfile.WriteLine("");
                    writeThis = false;
                }
                else continue;               

                for (int x = 0; x < scenesObjs[currentKey].Count; x++)
                {
                    tempObj = scenesObjs[currentKey][x];

                    switch (updateMode)
                    {
                        case ScoUpdateMode.Global:
                            writeThis = true;
                            break;
                        case ScoUpdateMode.Gameplay:
                            if (tempObj.gameplayFields) writeThis = true;
                            else writeThis = false;
                            break;
                        case ScoUpdateMode.Art:
                            if (tempObj.artFields) writeThis = true;
                            else writeThis = false;
                            break;
                        case ScoUpdateMode.Sound:
                            if (tempObj.soundFields) writeThis = true;
                            else writeThis = false;
                            break;
                    }

                    if (writeThis)
                    {
                        outfile.WriteLine("[Header(\"GameObject: " + tempObj.objName + "\")]");
                        outfile.WriteLine("");
                        writeThis = false;
                    }
                    else continue;

                    for (int y = 0; y < tempObj.attachedComponents.Count; y++)
                    {
                        tempComp = tempObj.attachedComponents[y];

                        switch (updateMode)
                        {
                            case ScoUpdateMode.Global:
                                writeThis = true;
                                break;
                            case ScoUpdateMode.Gameplay:
                                if (tempComp.gameplayFields) writeThis = true;
                                else writeThis = false;
                                break;
                            case ScoUpdateMode.Art:
                                if (tempComp.artFields) writeThis = true;
                                else writeThis = false;
                                break;
                            case ScoUpdateMode.Sound:
                                if (tempComp.soundFields) writeThis = true;
                                else writeThis = false;
                                break;
                        }

                        if (writeThis)
                        {
                            tempAttributes = new AttributeConstructor[] { new AttributeConstructor(SupportedAttributes.Id, tempComp.componentName + " Component - ID:") };
                            outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "Id", tempAttributes, "\"" + new Guid(tempComp.serializedGuid).ToString() + "\""));
                            separatorCount += 1;
                            outfile.WriteLine("");
                            writeThis = false;
                        }
                        else continue;
                        

                        for (int z = 0; z < tempComp.fields.Count; z++)
                        {
                            tempField = tempComp.fields[z];

                            switch (updateMode)
                            {
                                case ScoUpdateMode.Global:
                                    writeThis = true;
                                    break;
                                case ScoUpdateMode.Gameplay:
                                    if (tempField.fieldUsage == FieldUsage.Gameplay) writeThis = true;
                                    else writeThis = false;
                                    break;
                                case ScoUpdateMode.Art:
                                    if (tempField.fieldUsage == FieldUsage.Art) writeThis = true;
                                    else writeThis = false;
                                    break;
                                case ScoUpdateMode.Sound:
                                    if (tempField.fieldUsage == FieldUsage.Sound) writeThis = true;
                                    else writeThis = false;
                                    break;
                            }

                            if (writeThis)
                            {
                                tempAttributes = new AttributeConstructor[] { new AttributeConstructor(SupportedAttributes.Var, tempField.fieldName) };
                                outfile.WriteLine(WriteVar(tempField.fieldType, tempField.fieldName, tempAttributes, null, tempComp.serializedGuid));
                                writeThis = false;
                            }
                            else continue;
                            
                        }
                        outfile.WriteLine("");
                    }
                }
            }
            #endregion

            #region Write For Object From Prefabs
            //outfile.WriteLine("[Header(\"Objects From Prefabs\")]");
            outfile.WriteLine("");

            for (int a = 0; a < prefabObjs.Count; a++)
            {
                currentKey = prefabObjs.ElementAt(a).Key;
                writeThis = false;

                foreach (TweekObj obj in prefabObjs[currentKey])
                {
                    switch (updateMode)
                    {
                        case ScoUpdateMode.Global:
                            writeThis = true;
                            break;
                        case ScoUpdateMode.Gameplay:
                            if (obj.gameplayFields) writeThis = true;
                            break;
                        case ScoUpdateMode.Art:
                            if (obj.artFields) writeThis = true;
                            break;
                        case ScoUpdateMode.Sound:
                            if (obj.soundFields) writeThis = true;
                            break;
                    }
                }

                if (writeThis)
                {
                    outfile.WriteLine("[Space]");
                    tempAttributes = new AttributeConstructor[] { new AttributeConstructor(SupportedAttributes.Path, "From") };
                    outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "_From", tempAttributes, "\"" + currentKey + "\""));
                    separatorCount += 1;
                    outfile.WriteLine("");
                    writeThis = false;
                }
                else continue;

                for (int x = 0; x < prefabObjs[currentKey].Count; x++)
                {
                    tempObj = prefabObjs[currentKey][x];

                    switch (updateMode)
                    {
                        case ScoUpdateMode.Global:
                            writeThis = true;
                            break;
                        case ScoUpdateMode.Gameplay:
                            if (tempObj.gameplayFields) writeThis = true;
                            else writeThis = false;
                            break;
                        case ScoUpdateMode.Art:
                            if (tempObj.artFields) writeThis = true;
                            else writeThis = false;
                            break;
                        case ScoUpdateMode.Sound:
                            if (tempObj.soundFields) writeThis = true;
                            else writeThis = false;
                            break;
                    }

                    if (writeThis)
                    {
                        outfile.WriteLine("[Header(\"GameObject: " + tempObj.objName + "\")]");
                        outfile.WriteLine("");
                        writeThis = false;
                    }
                    else continue;                   

                    for (int y = 0; y < prefabObjs[currentKey][x].attachedComponents.Count; y++)
                    {
                        tempComp = tempObj.attachedComponents[y];

                        switch (updateMode)
                        {
                            case ScoUpdateMode.Global:
                                writeThis = true;
                                break;
                            case ScoUpdateMode.Gameplay:
                                if (tempComp.gameplayFields) writeThis = true;
                                else writeThis = false;
                                break;
                            case ScoUpdateMode.Art:
                                if (tempComp.artFields) writeThis = true;
                                else writeThis = false;
                                break;
                            case ScoUpdateMode.Sound:
                                if (tempComp.soundFields) writeThis = true;
                                else writeThis = false;
                                break;
                        }

                        if (writeThis)
                        {
                            tempAttributes = new AttributeConstructor[] { new AttributeConstructor(SupportedAttributes.Id, tempComp.componentName + " Component - ID:") };
                            outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "Id", tempAttributes, "\"" + new Guid(tempComp.serializedGuid).ToString() + "\""));
                            separatorCount += 1;
                            outfile.WriteLine("");
                            writeThis = false;
                        }
                       

                        for (int z = 0; z < prefabObjs[currentKey][x].attachedComponents[y].fields.Count; z++)
                        {
                            tempField = tempComp.fields[z];

                            switch (updateMode)
                            {
                                case ScoUpdateMode.Global:
                                    writeThis = true;
                                    break;
                                case ScoUpdateMode.Gameplay:
                                    if (tempField.fieldUsage == FieldUsage.Gameplay) writeThis = true;
                                    else writeThis = false;
                                    break;
                                case ScoUpdateMode.Art:
                                    if (tempField.fieldUsage == FieldUsage.Art) writeThis = true;
                                    else writeThis = false;
                                    break;
                                case ScoUpdateMode.Sound:
                                    if (tempField.fieldUsage == FieldUsage.Sound) writeThis = true;
                                    else writeThis = false;
                                    break;
                            }

                            if (writeThis)
                            {
                                tempAttributes = new AttributeConstructor[] { new AttributeConstructor(SupportedAttributes.Var, tempField.fieldName) };
                                outfile.WriteLine(WriteVar(tempField.fieldType, tempField.fieldName, tempAttributes, null, tempComp.serializedGuid));
                            }                           
                        }
                        outfile.WriteLine("");
                    }
                }
            }
            #endregion

            #region Class Footer
            outfile.WriteLine("}");
            #endregion
        }        
    }

    //when assembly is recompilled test if there was a recent update of SCO classes
    [UnityEditor.Callbacks.DidReloadScripts()]
    public static void TestRecentUpdate()
    {
        if (File.Exists("Assets/Tools/TweekSystem/.tempChecker.txt"))
        {
            File.Delete("Assets/Tools/TweekSystem/.tempChecker.txt");
            UpdateScoAssets();
        }
    }

    //update ID fiels of SCO Assets after a recent SCO Class update
    //[MenuItem("Tweek Operations / Update Assets")]
    public static void UpdateScoAssets()
    {
        string[] tempPaths = new string[0];

        List<object> gameplayScoAssets = new List<object>();
        List<object> artScoAssets = new List<object>();
        List<object> soundScoAssets = new List<object>();

        GameplayTweekScriptableObject gameplayScoRoot = ScriptableObject.CreateInstance<GameplayTweekScriptableObject>();
        ArtTweekScriptableObject artScoRoot = ScriptableObject.CreateInstance<ArtTweekScriptableObject>();
        SoundTweekScriptableObject soundScoRoot = ScriptableObject.CreateInstance<SoundTweekScriptableObject>();

        tempPaths = Directory.GetFiles(gameplayAssetsDirectory);

        if(tempPaths.Length != 0)
        {
            foreach(string path in tempPaths)
            {
                if(Path.GetExtension(path) == ".asset"|| Path.GetExtension(path) == ".ASSET")
                {
                    gameplayScoAssets.Add(AssetDatabase.LoadAssetAtPath(path, typeof(GameplayTweekScriptableObject)) as object);
                }
            }
        }

        tempPaths = Directory.GetFiles(graphicAssetsDirectory);

        if (tempPaths.Length != 0)
        {
            foreach (string path in tempPaths)
            {
                if (Path.GetExtension(path) == ".asset" || Path.GetExtension(path) == ".ASSET")
                {
                    artScoAssets.Add(AssetDatabase.LoadAssetAtPath(path, typeof(ArtTweekScriptableObject)) as object);
                }
            }
        }

        tempPaths = Directory.GetFiles(soundAssetsDirectory);

        if (tempPaths.Length != 0)
        {
            foreach (string path in tempPaths)
            {
                if (Path.GetExtension(path) == ".asset" || Path.GetExtension(path) == ".ASSET")
                {
                    soundScoAssets.Add(AssetDatabase.LoadAssetAtPath(path, typeof(SoundTweekScriptableObject)) as object);
                }
            }
        }

        List<object> scoAssets = null;
        object scoRoot = null;
        Type scoType = null;
        FieldInfo[] scoFields = new FieldInfo[0];

        BindingFlags flags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

        for (int i = 1; i < 3; i++)
        {
            switch (i)
            {
                case 1:
                    scoType = typeof(GameplayTweekScriptableObject);
                    scoRoot = gameplayScoRoot;
                    scoAssets = gameplayScoAssets;
                    break;
                case 2:
                    scoType = typeof(ArtTweekScriptableObject);
                    scoRoot = artScoRoot;
                    scoAssets = artScoAssets;
                    break;
                case 3:
                    scoType = typeof(SoundTweekScriptableObject);
                    scoRoot = soundScoRoot;
                    scoAssets = soundScoAssets;
                    break;
                default:
                    break;
            }

            foreach(object asset in scoAssets)
            {
                scoFields = scoType.GetFields(flags);

                foreach (FieldInfo field in scoFields)
                {
                    if (field.GetCustomAttributes().ToArray().Length > 0)
                    {
                        IdAttribute idAttribute = field.GetCustomAttribute<IdAttribute>();
                        if (idAttribute != null)
                        {
                            field.SetValue(asset, field.GetValue(scoRoot));
                        }
                    }
                }
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// Update field on prefab and scene's objects with value from a SCO Assets all across the project
    /// 
    /// if you don't want to update a part, give a null object
    /// </summary>

    //manage values application protocole (handle progress bar)
    public static void LaunchValuesApplication(object gameplayValues, object artValues, object soundValues)
    {
        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Launching protocole to unpack infos from SCO", 0f);

        Dictionary<string, List<TweekField>> fieldsToUpdate = new Dictionary<string, List<TweekField>>();
        int compOriginCount = 0;

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Unpack Gameplay Values", 0.10f);

        if (gameplayValues != null) fieldsToUpdate = UnpackScoObject(ScoUpdateMode.Gameplay, gameplayValues, fieldsToUpdate);

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Unpack Art Values", 0.20f);

        if (artValues != null) fieldsToUpdate = UnpackScoObject(ScoUpdateMode.Art, artValues, fieldsToUpdate);

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Unpack Sound Values", 0.30f);

        if (soundValues != null) fieldsToUpdate = UnpackScoObject(ScoUpdateMode.Sound, soundValues, fieldsToUpdate);
        compOriginCount = fieldsToUpdate.Count;

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Start applying new values to Scene's Objects", 0.40f);

        fieldsToUpdate = ApplySceneValues(fieldsToUpdate);

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Start applying new values to Prefabs", 0.50f);

        fieldsToUpdate = ApplyPrefabValues(fieldsToUpdate);

        if (fieldsToUpdate.Count != 0)
        {
            Debug.Log("Tweek System Operation to apply new values to referenced componenet has ended. " + fieldsToUpdate.Count + " components (on " + compOriginCount + ") were not found and so the linked values were not applied");
            Debug.Log("It could be due to a suppression of the fields or an error in the data collector system");
        }

        EditorUtility.ClearProgressBar();
    }

    //extract values from a specified sco assets
    static Dictionary<string, List<TweekField>> UnpackScoObject(ScoUpdateMode scoUpdateMode, object scoAsset, Dictionary<string, List<TweekField>> lastDictionary)
    {
        Dictionary<string, List<TweekField>> compsToUpdate = new Dictionary<string, List<TweekField>>();

        Type compType = null;
        BindingFlags flags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

        switch (scoUpdateMode)
        {
            case ScoUpdateMode.Gameplay:
                compType = typeof(GameplayTweekScriptableObject);
                break;
            case ScoUpdateMode.Art:
                compType = typeof(ArtTweekScriptableObject);
                break;
            case ScoUpdateMode.Sound:
                compType = typeof(SoundTweekScriptableObject);
                break;
        }

        FieldInfo[] compFields = compType.GetFields(flags);

        string[] tempSplitName;
        string tempId = string.Empty;
        string tempName = string.Empty;

        if (lastDictionary != null) compsToUpdate = lastDictionary;

        foreach (FieldInfo field in compFields)
        {
            if (field.GetCustomAttributes().ToArray().Length > 0)
            {
                IdAttribute idAttribute = field.GetCustomAttribute<IdAttribute>();

                if (idAttribute != null)
                {
                    if(!compsToUpdate.ContainsKey(field.GetValue(scoAsset).ToString())) compsToUpdate.Add(field.GetValue(scoAsset).ToString(), new List<TweekField>());
                }
            }
        }

        foreach (FieldInfo field in compFields)
        {
            if (field.GetCustomAttributes().ToArray().Length > 0)
            {
                VarAttribute varAttribute = field.GetCustomAttribute<VarAttribute>();

                if (varAttribute != null)
                {
                    tempId = string.Empty;
                    tempSplitName = field.Name.Split(new char[] { '_' });
                    tempName = tempSplitName[2];
                    tempSplitName = tempSplitName[1].Split(new char[] { 'µ' });

                    for (int i = 0; i < tempSplitName.Length; i++)
                    {
                        tempId += tempSplitName[i];
                        if (i != tempSplitName.Length - 1) tempId += "-";
                    }
                    compsToUpdate[tempId].Add(new TweekField(field.FieldType, tempName, field.GetValue(scoAsset)));
                }
            }
        }

        return compsToUpdate;
    }

    //Apply extracted values to object in Scene
    static Dictionary<string, List<TweekField>> ApplySceneValues(Dictionary<string, List<TweekField>> compsToUpdate)
    {
        int tempFieldCount = 0;
        int tempLeftField = 0;

        string originScene = EditorSceneManager.GetActiveScene().path;

        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        string[] allScenesPaths = Directory.GetFiles(scenesPath);

        foreach (string path in allScenesPaths)
        {
            if (Path.GetExtension(path) == ".unity" || Path.GetExtension(path) == ".UNITY")
            {
                EditorSceneManager.OpenScene(path);

                object[] allCompInScene = FindObjectsOfType(typeof(TweekMonoBehaviour));
                if (allCompInScene.Length == 0) continue;

                foreach (TweekMonoBehaviour comp in allCompInScene)
                {
                    if (comp.valuesTweekState == TweekMonoBehaviour.TweekState.ProperValues)
                    {
                        if (compsToUpdate.ContainsKey(new Guid(comp.serializedGuid).ToString()))
                        {
                            tempFieldCount = compsToUpdate[new Guid(comp.serializedGuid).ToString()].Count;
                            tempLeftField = UpdateFields(comp, compsToUpdate[new Guid(comp.serializedGuid).ToString()]);
                            compsToUpdate.Remove(new Guid(comp.serializedGuid).ToString());
                        }
                        if (tempLeftField != 0)
                        {
                            Debug.Log(tempLeftField + " stored field's values (on a total of: " + tempFieldCount + ") were impossible to apply to Component: " + comp.GetType() + " in Scenes: " + path);
                            Debug.Log("It could be due to a suppression of the fields or an error in the data collector system");
                        }
                    }
                }
                EditorSceneManager.SaveOpenScenes();
            }
        }

        EditorSceneManager.OpenScene(originScene);
        return compsToUpdate;
    }

    //Apply extracted values to prefab
    static Dictionary<string, List<TweekField>> ApplyPrefabValues(Dictionary<string, List<TweekField>> compsToUpdate)
    {
        int tempFieldCount = 0;
        int tempLeftField = 0;

        string[][] allPrefabsPaths = new string[0][];
        string[] subDirectoriesPaths = new string[0];
        string indexPath;

        GameObject tempPrefab;
        object[] allCompOnPrefab;


        if (Directory.GetDirectories(prefabsPath).Length > 0)
        {
            subDirectoriesPaths = Directory.GetDirectories(prefabsPath);
            for (int i = 0; i < subDirectoriesPaths.Length; i++) subDirectoriesPaths[i] = PathWritter(subDirectoriesPaths[i]);

            allPrefabsPaths = new string[subDirectoriesPaths.Length + 1][];
            allPrefabsPaths[0] = Directory.GetFiles(prefabsPath);
            for (int i = 1; i < allPrefabsPaths.Length; i++) allPrefabsPaths[i] = Directory.GetFiles(prefabsPath + "/" + subDirectoriesPaths[i - 1]);
            for (int i = 0; i < allPrefabsPaths.Length; i++) for (int z = 0; z < allPrefabsPaths[i].Length; z++) allPrefabsPaths[i][z] = PathWritter(allPrefabsPaths[i][z]);
        }
        else
        {
            allPrefabsPaths = new string[1][];
            allPrefabsPaths[0] = Directory.GetFiles(prefabsPath);
            for (int i = 0; i < allPrefabsPaths[0].Length; i++) allPrefabsPaths[0][i] = PathWritter(allPrefabsPaths[0][i]);
        }

        for (int x = 0; x < allPrefabsPaths.Length; x++)
        {
            if (x == 0) indexPath = PathWritter(prefabsPath);
            else indexPath = PathWritter(prefabsPath + "/" + subDirectoriesPaths[x - 1]);

            for (int y = 0; y < allPrefabsPaths[x].Length; y++)
            {
                if (Path.GetExtension(allPrefabsPaths[x][y]) == ".prefab" || Path.GetExtension(allPrefabsPaths[x][y]) == ".PREFAB")
                {
                    tempPrefab = PrefabUtility.LoadPrefabContents(allPrefabsPaths[x][y]);
                    allCompOnPrefab = tempPrefab.GetComponents<TweekMonoBehaviour>();
                    if (allCompOnPrefab.Length == 0) continue;

                    foreach (TweekMonoBehaviour comp in allCompOnPrefab)
                    {
                        if (comp.valuesTweekState == TweekMonoBehaviour.TweekState.ProperValues)
                        {
                            if (compsToUpdate.ContainsKey(new Guid(comp.serializedGuid).ToString()))
                            {
                                tempFieldCount = compsToUpdate[new Guid(comp.serializedGuid).ToString()].Count;
                                tempLeftField = UpdateFields(comp, compsToUpdate[new Guid(comp.serializedGuid).ToString()]);
                                PrefabUtility.SaveAsPrefabAsset(tempPrefab, allPrefabsPaths[x][y]);
                                PrefabUtility.UnloadPrefabContents(tempPrefab);
                                compsToUpdate.Remove(new Guid(comp.serializedGuid).ToString());
                            }
                            if (tempLeftField != 0)
                            {
                                Debug.Log(tempLeftField + " stored field's values (on a total of: " + tempFieldCount + ") were impossible to apply to Component: " + comp.GetType() + " in Prefab: " + allPrefabsPaths[x][y]);
                                Debug.Log("It could be due to a suppression of the fields or an error in the data collector system");
                            }
                        }
                    }
                }
            }
        }

        return compsToUpdate;
    }

    //Recurrent method to update fields value on a given component
    static int UpdateFields(TweekMonoBehaviour compToUpdate, List<TweekField> fieldInfosToApply)
    {
        Type compType = compToUpdate.GetType();
        BindingFlags flags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        FieldInfo[] compFields = compType.GetFields(flags);

        TweekField matchingField = new TweekField();

        foreach (FieldInfo field in compFields)
        {
            bool hasAMatchingField = false;

            if (field.GetCustomAttributes().ToArray().Length > 0)
            {
                TweekFlagAttribute attribute = field.GetCustomAttribute<TweekFlagAttribute>();
                if (attribute != null)
                {
                    foreach (TweekField potentialField in fieldInfosToApply)
                    {
                        if (potentialField.fieldName == field.Name)
                        {
                            matchingField = potentialField;
                            hasAMatchingField = true;
                        }
                    }

                    if (hasAMatchingField == true)
                    {
                        field.SetValue(compToUpdate, matchingField.fieldValue);
                        EditorUtility.SetDirty(compToUpdate);
                        fieldInfosToApply.Remove(matchingField);
                    }
                }
            }
        }

        if (fieldInfosToApply.Count != 0) return fieldInfosToApply.Count;
        else return 0;
    }


    /// <summary>
    /// Tools methods used for recurent action in Tweek System
    /// </summary>
    //rewrite path string in Unity's way (without \)
    static string PathWritter(string path)
    {
        string newPath = string.Empty;
        string[] splitPath = path.Split(new char[] { '\\', '/' });

        for (int i = 0; i < splitPath.Length; i++)
        {
            if (i != splitPath.Length - 1)
            {
                newPath += splitPath[i];
                newPath += "/";
            }
            else newPath += splitPath[i];
        }

        return newPath;
    }

    //rewrite string to replace unsupported character with '_'
    static string NameWritter(string name)
    {
        string newName = string.Empty;
        string[] splitName = name.Split(new char[] { '\\', '/', '.', '-' });

        for (int i = 0; i < splitName.Length; i++)
        {
            if (i != splitName.Length - 1)
            {
                newName += splitName[i];
                newName += "_";
            }
            else newName += splitName[i];
        }
        return newName;
    }

    //get regular name from system type (Double → float)
    static string GetTypeName(Type type)
    {
        var codeDomProvider = CodeDomProvider.CreateProvider("C#");
        var typeReferenceExpression = new CodeTypeReferenceExpression(new CodeTypeReference(type));

        string[] splitTypeName = type.ToString().Split(new char[] { '.' });

        if (splitTypeName[0] == "UnityEngine")
        {
            return splitTypeName[1];
        }
        else
        {
            using (var writer = new StringWriter())
            {
                codeDomProvider.GenerateCodeFromExpression(typeReferenceExpression, writer, new CodeGeneratorOptions());
                return writer.GetStringBuilder().ToString();
            }
        }
    }

    public enum SupportedAttributes { Header, Space, Box, FoldOut, SerializeField, HideInInspector, Range, Min, Max, ReadOnly, Id, Var, Path };

    //Write a supported attribute
    static string WriteAttribute(SupportedAttributes targetAttributes, dynamic arg_1 = null, dynamic arg_2 = null, dynamic arg_3 = null)
    {
        string attributeLine = string.Empty;

        switch (targetAttributes)
        {
            case SupportedAttributes.Header:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[Header(\"" + arg_1 + "\")]";
                break;

            case SupportedAttributes.Space:
                attributeLine = "[Space]";
                break;

            case SupportedAttributes.Box:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[BoxGroup(\"" + arg_1 + "\")]";
                break;

            case SupportedAttributes.FoldOut:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[FoldOut(\"" + arg_1 + "\")]";
                break;

            case SupportedAttributes.SerializeField:
                attributeLine = "[SerializeField]";
                break;

            case SupportedAttributes.HideInInspector:
                attributeLine = "[HideInInspector]";
                break;

            case SupportedAttributes.Range:
                if (arg_1.GetType() == typeof(float) && arg_2.GetType() == typeof(float)) attributeLine = "[Range(" + arg_1.ToString() + "," + arg_2.ToString() + "\")]";
                break;

            case SupportedAttributes.Min:
                if (arg_1.GetType() == typeof(float)) attributeLine = "[Min(" + arg_1.ToString() + "\")]";
                break;

            case SupportedAttributes.Max:
                if (arg_1.GetType() == typeof(float)) attributeLine = "[Max(" + arg_1.ToString() + "\")]";
                break;

            case SupportedAttributes.ReadOnly:
                attributeLine = "[ReadOnly]";
                break;
            case SupportedAttributes.Id:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[Id(\"" + arg_1 + "\")]";
                break;

            case SupportedAttributes.Var:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[Var(\"" + arg_1 + "\")]";
                break;

            case SupportedAttributes.Path:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[Path(\"" + arg_1 + "\")]";
                break;

            default:
                break;
        }

        return attributeLine;
    }

    //Write a var field
    static string WriteVar(Type type, string name, AttributeConstructor[] fieldAttributes = null, string value = null, byte[] guid = null, bool range = true)
    {
        string varField = string.Empty;
        string[] tempSplitGuid = new string[0];

        if (guid != null)
        {
            tempSplitGuid = new Guid(guid).ToString().Split(new char[] { '-' });
        }

        if (fieldAttributes != null)
        {
            foreach (AttributeConstructor attribute in fieldAttributes)
            {
                varField += WriteAttribute(attribute.attribute, attribute.arg_1, attribute.arg_2, attribute.arg_3) + " "; // Refacto from [x][y] to [x,y]
            }
        }

        if (range) varField += "public ";
        else varField += "private ";

        varField += GetTypeName(type) + " ";

        if (guid != null)
        {
            varField += "_";
            for (int i = 0; i < tempSplitGuid.Length; i++)
            {
                varField += tempSplitGuid[i];
                if (i != tempSplitGuid.Length - 1) varField += "µ";
                else varField += "_";
            }
        }

        varField += NameWritter(name);

        if (value != null)
        {
            varField += " = " + value;
        }

        varField += ";";

        return varField;
    }
}
