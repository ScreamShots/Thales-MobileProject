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

public class TweekCore : UnityEngine.Object
{
    public enum ScoUpdateMode { Global, Gameplay, Art, Sound }

    private static IntePipeSettingsData Data 
    {
        get { return IntePipeSettingsData.GetOrCreateSettings(); }
    }

    [MenuItem("Tools/Show static info")]
    public static void DebugStatic()
    {
        Debug.Log(Data.gameplayScoPath);
        Debug.Log(Data.graphicScoPath);
        Debug.Log(Data.soundScoPath);
        Debug.Log(string.Empty);
        Debug.Log(Data.gameplayAssetsDirectory);
        Debug.Log(Data.graphicAssetsDirectory);
        Debug.Log(Data.soundAssetsDirectory);
        Debug.Log(string.Empty);
        Debug.Log(Data.soundScoPath);
        Debug.Log(Data.graphicScoPath);
        Debug.Log(Data.gameplayScoPath);
    }

    /// --------------------------------------------------------------------------------------------------------------------///
    ///                                           EDITOR COMMANDE (DEPRECATED)                                              ///
    /// --------------------------------------------------------------------------------------------------------------------///

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

    /// --------------------------------------------------------------------------------------------------------------------///
    ///                                                    PROTOCOLES                                                       ///
    /// --------------------------------------------------------------------------------------------------------------------///

    public static void LaunchScoUpdate(ScoUpdateMode updateMode)
    {
        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Start " + updateMode.ToString() + " update operation", 0f);

        Dictionary<string, List<TweekObj>> sceneRefs;
        Dictionary<string, List<TweekObj>> prefabRefs;

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Referencing values from scene's objects", 0.20f);

        sceneRefs = GetReferencesFromScene();

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Referencing values from prefabs", 0.40f);

        prefabRefs = GetReferencesFromPrefabs();

        if (File.Exists("Assets/Tools/TweekSystem/.tempChecker.txt")) File.Delete("Assets/Tools/TweekSystem/.tempChecker.txt");

        FileStream fs = File.Open("Assets/Tools/Internal/TweekSystem/.tempChecker.txt", FileMode.Create);
        using (StreamWriter outfile = new StreamWriter(fs))
        {
            outfile.WriteLine("temp file");
        }

        if (updateMode == ScoUpdateMode.Global)
        {
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing Gameplay SCO Class", 0.50f);
            WriteScoClass(sceneRefs, prefabRefs, ScoUpdateMode.Gameplay);
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing Art SCO Class", 0.60f);
            WriteScoClass(sceneRefs, prefabRefs, ScoUpdateMode.Art);
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing Sound SCO Class", 0.70f);
            WriteScoClass(sceneRefs, prefabRefs, ScoUpdateMode.Sound);
        }
        else
        {
            EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Writing" + updateMode.ToString() + "SCO Class", 0.60f);
            WriteScoClass(sceneRefs, prefabRefs, updateMode);
        }

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Compiling", 0.80f);
        AssetDatabase.Refresh();
        CompilationPipeline.RequestScriptCompilation();
    }

    [UnityEditor.Callbacks.DidReloadScripts()]
    public static void TestRecentUpdate()
    {
        if (File.Exists("Assets/Tools/Internal/TweekSystem/.tempChecker.txt"))
        {
            File.Delete("Assets/Tools/Internal/TweekSystem/.tempChecker.txt");
            UpdateScoAssets();
        }
    }

    public static void LaunchValuesApplication(object gameplayValues, object artValues, object soundValues)
    {
        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Launching protocole to unpack infos from SCO", 0f);

        Dictionary<string, Dictionary<string, List<TweekField>>> fieldsToUpdate = new Dictionary<string, Dictionary<string, List<TweekField>>>();
        int compOriginCount = 0;
        int compFinalCount = 0;

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Unpack Gameplay Values", 0.10f);

        if (gameplayValues != null) fieldsToUpdate = UnpackScoObject(ScoUpdateMode.Gameplay, gameplayValues, fieldsToUpdate);

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Unpack Art Values", 0.20f);

        if (artValues != null) fieldsToUpdate = UnpackScoObject(ScoUpdateMode.Art, artValues, fieldsToUpdate);

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Unpack Sound Values", 0.30f);

        if (soundValues != null) fieldsToUpdate = UnpackScoObject(ScoUpdateMode.Sound, soundValues, fieldsToUpdate);

        for (int i = 0; i < fieldsToUpdate.Count; i++)
        {
            compOriginCount += fieldsToUpdate.ElementAt(i).Value.Count();
        }

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Start applying new values to Scene's Objects", 0.40f);

        fieldsToUpdate = ApplySceneValues(fieldsToUpdate);

        EditorUtility.DisplayProgressBar("Tweek System operation on going.", "Start applying new values to Prefabs", 0.50f);

        fieldsToUpdate = ApplyPrefabValues(fieldsToUpdate);

        for (int i = 0; i < fieldsToUpdate.Count; i++)
        {
            compFinalCount += fieldsToUpdate.ElementAt(i).Value.Count();
        }

        if (compFinalCount != 0)
        {
            Debug.Log("Tweek System Operation to apply new values to referenced componenet has ended. " + compFinalCount + " components (on " + compOriginCount + ") were not found and so the linked values were not applied");
            Debug.Log("It could be due to a suppression of the fields or an error in the data collector system");
        }

        EditorUtility.ClearProgressBar();
    }

    /// --------------------------------------------------------------------------------------------------------------------///
    ///             SEARCHING FOR FLAGGED FIELD IN SCENE AND PREFAB AND CREATE SCO CLASS REFERENCING THEM                   ///
    /// --------------------------------------------------------------------------------------------------------------------///

    public static Dictionary<string, List<TweekObj>> GetReferencesFromScene()
    {
        Dictionary<string, List<TweekObj>> allScenesReferences = new Dictionary<string, List<TweekObj>>();

        TweekObj tempTObj = new TweekObj();
        TweekComponent tempTComp = new TweekComponent();
        List<TweekField> tempTFields = new List<TweekField>();

        string originScenePath = EditorSceneManager.GetActiveScene().path;
        if (EditorSceneManager.GetActiveScene().isDirty) EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        string[] allScenesPaths = Directory.GetFiles(Data.scenesPath);
        for (int i = 0; i < allScenesPaths.Length; i++) allScenesPaths[i] = PathWritter(allScenesPaths[i]);

        foreach (string scenePath in allScenesPaths)
        {
            if (Path.GetExtension(scenePath) == ".unity" || Path.GetExtension(scenePath) == ".UNITY")
            {
                EditorSceneManager.OpenScene(scenePath);

                object[] allReferencerInScene = FindObjectsOfType(typeof(TweekReferencer));
                if (allReferencerInScene.Length > 0) allScenesReferences.Add(scenePath, new List<TweekObj>());
                else continue;

                foreach (TweekReferencer referencer in allReferencerInScene)
                {
                    if (referencer.referencedComponents.Count > 0) tempTObj = new TweekObj(referencer.gameObject.name, referencer.serializedGuid);
                    else continue;

                    foreach (TweekReference comp in referencer.referencedComponents)
                    {
                        tempTFields = FindFields(comp.component);

                        if (tempTFields.Count > 0)
                        {
                            tempTComp = new TweekComponent(TypeWritter(comp.component.GetType()), comp.serializedGuid);

                            foreach (TweekField field in tempTFields) tempTComp.AddField(field);

                            tempTObj.AddComp(tempTComp);
                        }
                    }

                    if (tempTObj.attachedComponents.Count > 0) allScenesReferences[scenePath].Add(tempTObj);
                }
            }
        }

        EditorSceneManager.OpenScene(originScenePath);

        allScenesReferences = CleanDictionary(allScenesReferences);
        if (allScenesReferences.Count > 0) return allScenesReferences;
        else return null;
    }

    public static Dictionary<string, List<TweekObj>> GetReferencesFromPrefabs()
    {

        Dictionary<string, List<TweekObj>> allPrefabsReferences = new Dictionary<string, List<TweekObj>>();

        TweekObj tempTObj = new TweekObj();
        TweekComponent tempTComp = new TweekComponent();
        List<TweekField> tempTFields = new List<TweekField>();
        GameObject tempPrefab;
        TweekReferencer tempReferencer;

        string[][] allPrefabsPaths = new string[0][];
        string[] subDirectoriesPaths = new string[0];
        string indexPath;

        if (Directory.GetDirectories(Data.prefabsPath).Length > 0)
        {
            subDirectoriesPaths = Directory.GetDirectories(Data.prefabsPath);
            for (int i = 0; i < subDirectoriesPaths.Length; i++) subDirectoriesPaths[i] = PathWritter(subDirectoriesPaths[i]);

            allPrefabsPaths = new string[subDirectoriesPaths.Length + 1][];
            allPrefabsPaths[0] = Directory.GetFiles(Data.prefabsPath);
            for (int i = 1; i < allPrefabsPaths.Length; i++) allPrefabsPaths[i] = Directory.GetFiles(subDirectoriesPaths[i - 1]);
            for (int i = 0; i < allPrefabsPaths.Length; i++) for (int z = 0; z < allPrefabsPaths[i].Length; z++) allPrefabsPaths[i][z] = PathWritter(allPrefabsPaths[i][z]);
        }
        else
        {
            allPrefabsPaths = new string[1][];
            allPrefabsPaths[0] = Directory.GetFiles(Data.prefabsPath);
            for (int i = 0; i < allPrefabsPaths[0].Length; i++) allPrefabsPaths[0][i] = PathWritter(allPrefabsPaths[0][i]);
        }

        TweekReferencer.prefabEdition = true;

        for (int x = 0; x < allPrefabsPaths.Length; x++)
        {
            if (x == 0) indexPath = PathWritter(Data.prefabsPath);
            else indexPath = PathWritter(Data.prefabsPath + "/" + subDirectoriesPaths[x - 1]);
            allPrefabsReferences.Add(indexPath, new List<TweekObj>());

            for (int y = 0; y < allPrefabsPaths[x].Length; y++)
            {
                if (Path.GetExtension(allPrefabsPaths[x][y]) == ".prefab" || Path.GetExtension(allPrefabsPaths[x][y]) == ".PREFAB")
                {
                    tempPrefab = PrefabUtility.LoadPrefabContents(allPrefabsPaths[x][y]);
                    tempReferencer = tempPrefab.GetComponent<TweekReferencer>();

                    if (tempReferencer != null)
                    {
                        if (tempReferencer.referencedComponents.Count > 0) tempTObj = new TweekObj(tempPrefab.name, tempReferencer.serializedGuid);
                        else
                        {
                            PrefabUtility.UnloadPrefabContents(tempPrefab);
                            continue;
                        }                           
                    }
                    else continue;

                    foreach (TweekReference comp in tempReferencer.referencedComponents)
                    {
                        tempTFields = FindFields(comp.component);

                        if (tempTFields.Count > 0)
                        {
                            tempTComp = new TweekComponent(TypeWritter(comp.component.GetType()), comp.serializedGuid);

                            foreach (TweekField field in tempTFields) tempTComp.AddField(field);

                            tempTObj.AddComp(tempTComp);
                        }
                    }

                    if (tempTObj.attachedComponents.Count > 0) allPrefabsReferences[indexPath].Add(tempTObj);
                    PrefabUtility.UnloadPrefabContents(tempPrefab);
                }
            }
        }

        TweekReferencer.prefabEdition = false;
        allPrefabsReferences = CleanDictionary(allPrefabsReferences);
        if (allPrefabsReferences.Count > 0) return allPrefabsReferences;
        else return null;
    }

    static List<TweekField> FindFields(MonoBehaviour comp)
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

    static void WriteScoClass(Dictionary<string, List<TweekObj>> scenesReferences, Dictionary<string, List<TweekObj>> prefabsReferences, ScoUpdateMode updateMode)
    {
        string targetScoPath = string.Empty;
        string currentKey = string.Empty;

        AttributeBuilder[] tempAttributes;
        int separatorCount = 0;

        switch (updateMode)
        {
            case ScoUpdateMode.Art:
                targetScoPath = Data.graphicScoPath;
                break;
            case ScoUpdateMode.Gameplay:
                targetScoPath = Data.gameplayScoPath;
                break;
            case ScoUpdateMode.Sound:
                targetScoPath = Data.soundScoPath;
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
            outfile.WriteLine("using PlayerEquipement;");
            outfile.WriteLine("");
            outfile.WriteLine("[CreateAssetMenu(menuName =\"Tweek/" + updateMode.ToString() + " Asset\")]");
            outfile.WriteLine("public class " + updateMode.ToString() + "TweekScriptableObject : ScriptableObject");
            outfile.WriteLine("{");
            #endregion

            #region Write For Object From Scenes
            if (DictionaryUsagePresence(scenesReferences, updateMode))
            {
                outfile.WriteLine("[Header(\"Objects From Scenes\")]");
                outfile.WriteLine("");

                for (int a = 0; a < scenesReferences.Count; a++)
                {
                    currentKey = scenesReferences.ElementAt(a).Key;

                    if (ListUsagePresence(scenesReferences[currentKey], updateMode))
                    {
                        outfile.WriteLine("[Space]");
                        tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Path, "From") };
                        outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "_From", tempAttributes, "\"" + currentKey + "\""));
                        separatorCount += 1;
                        outfile.WriteLine("");
                    }
                    else continue;

                    foreach (TweekObj obj in scenesReferences[currentKey])
                    {
                        if (ObjectUsagePresence(obj, updateMode))
                        {
                            outfile.WriteLine("[Header(\"GameObject: " + obj.objName + "\")]");
                            tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Id, obj.objName + " Component - ID:") };
                            outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "Id", tempAttributes, "\"" + new Guid(obj.serializedGuid).ToString() + "\""));
                            separatorCount += 1;
                            outfile.WriteLine("");
                        }
                        else continue;

                        foreach (TweekComponent comp in obj.attachedComponents)
                        {
                            if (CompUsagePresence(comp, updateMode))
                            {
                                tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Comp, comp.componentName + " Component - ID:") };
                                Debug.Log(obj.serializedGuid.Length + " " + comp.serializedGuid.Length);
                                outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "Comp", tempAttributes, "\"" + new Guid(obj.serializedGuid).ToString() + "_" + new Guid(comp.serializedGuid).ToString() + "\""));
                                separatorCount += 1;
                                outfile.WriteLine("");
                            }
                            else continue;

                            foreach (TweekField field in comp.fields)
                            {
                                if (FieldUsagePresence(field, updateMode))
                                {
                                    tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Var, field.fieldName), new AttributeBuilder(SupportedAttributes.ToolTip, field.fieldName)};
                                    outfile.WriteLine(WriteVar(field.fieldType, field.fieldName, tempAttributes, null, new byte[][] { obj.serializedGuid, comp.serializedGuid }));
                                }
                            }
                            outfile.WriteLine("");
                        }
                    }
                }
            }
            #endregion

            #region Write For Object From Prefabs
            if (DictionaryUsagePresence(prefabsReferences, updateMode))
            {
                outfile.WriteLine("[Header(\"Objects From Prefabs\")]");
                outfile.WriteLine("");

                for (int a = 0; a < prefabsReferences.Count; a++)
                {
                    currentKey = prefabsReferences.ElementAt(a).Key;

                    if (ListUsagePresence(prefabsReferences[currentKey], updateMode))
                    {
                        outfile.WriteLine("[Space]");
                        tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Path, "From") };
                        outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "_From", tempAttributes, "\"" + currentKey + "\""));
                        separatorCount += 1;
                        outfile.WriteLine("");
                    }
                    else continue;

                    foreach (TweekObj obj in prefabsReferences[currentKey])
                    {
                        if (ObjectUsagePresence(obj, updateMode))
                        {
                            outfile.WriteLine("[Header(\"GameObject: " + obj.objName + "\")]");
                            tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Id, obj.objName + " Component - ID:") };
                            outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "Id", tempAttributes, "\"" + new Guid(obj.serializedGuid).ToString() + "\""));
                            separatorCount += 1;
                            outfile.WriteLine("");
                        }
                        else continue;

                        foreach (TweekComponent comp in obj.attachedComponents)
                        {
                            if (CompUsagePresence(comp, updateMode))
                            {
                                tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Comp, comp.componentName + " Component - ID:") };
                                outfile.WriteLine(WriteVar(typeof(string), "s" + separatorCount.ToString() + "Comp", tempAttributes, "\"" + new Guid(obj.serializedGuid).ToString() + "_" + new Guid(comp.serializedGuid).ToString() + "\""));
                                separatorCount += 1;
                                outfile.WriteLine("");
                            }
                            else continue;

                            foreach (TweekField field in comp.fields)
                            {
                                if (FieldUsagePresence(field, updateMode))
                                {
                                    tempAttributes = new AttributeBuilder[] { new AttributeBuilder(SupportedAttributes.Var, field.fieldName), new AttributeBuilder(SupportedAttributes.ToolTip, field.fieldName) };
                                    outfile.WriteLine(WriteVar(field.fieldType, field.fieldName, tempAttributes, null, new byte[][] { obj.serializedGuid, comp.serializedGuid }));
                                }
                            }
                            outfile.WriteLine("");
                        }
                    }
                }
            }
            #endregion

            #region Class Footer
            outfile.WriteLine("}");
            #endregion

        }
    }

    public enum SupportedAttributes { Header, Space, Box, FoldOut, SerializeField, HideInInspector, Range, Min, Max, ReadOnly, Id, Var, Path, Comp, ToolTip };

    static string WriteAttribute(SupportedAttributes targetAttributes, dynamic arg_1 = null, dynamic arg_2 = null)
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

            case SupportedAttributes.Comp:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[Comp(\"" + arg_1 + "\")]";
                break;

            case SupportedAttributes.ToolTip:
                if (arg_1.GetType() == typeof(string)) attributeLine = "[Tooltip(\"" + arg_1 + "\")]";
                break;

            default:
                break;
        }

        return attributeLine;
    }

    static string WriteVar(Type type, string name, AttributeBuilder[] fieldAttributes = null, string value = null, byte[][] guids = null, bool range = true)
    {
        string varField = string.Empty;
        string[][] tempSplitGuid = new string[0][];

        if (guids != null)
        {
            tempSplitGuid = new string[guids.Length][];

            for (int i = 0; i < tempSplitGuid.Length; i++)
            {
                tempSplitGuid[i] = new Guid(guids[i]).ToString().Split(new char[] { '-' });
            }
        }

        if (fieldAttributes != null)
        {
            foreach (AttributeBuilder attribute in fieldAttributes)
            {
                varField += WriteAttribute(attribute.attribute, attribute.arg_1, attribute.arg_2) + " "; 
            }
        }

        if (range) varField += "public ";
        else varField += "private ";

        varField += TypeWritter(type) + " ";

        varField += NameWritter(name);

        if (guids != null)
        {
            for (int x = 0; x < guids.Length; x++)
            {
                varField += "_";
                for (int y = 0; y < tempSplitGuid[x].Length; y++)
                {
                    varField += tempSplitGuid[x][y];
                    if (y != tempSplitGuid[x].Length - 1) varField += "µ";
                }
            }
        }

        if (value != null)
        {
            varField += " = " + value;
        }

        varField += ";";

        return varField;
    }

    static bool DictionaryUsagePresence(Dictionary<string, List<TweekObj>> dic, ScoUpdateMode updateMode)
    {
        if (dic == null) return false;
        else if (updateMode == ScoUpdateMode.Global) return true;
        else
        {
            for (int i = 0; i < dic.Count; i++)
            {
                if (ListUsagePresence(dic.ElementAt(i).Value, updateMode)) return true;
            }
        }
        return false;
    }

    static bool ListUsagePresence(List<TweekObj> list, ScoUpdateMode updateMode)
    {
        if (updateMode == ScoUpdateMode.Global) return true;
        else
        {
            foreach (TweekObj obj in list)
            {
                if (ObjectUsagePresence(obj, updateMode)) return true;
            }
        }
        return false;
    }

    static bool ObjectUsagePresence(TweekObj obj, ScoUpdateMode updateMode)
    {
        switch (updateMode)
        {
            case ScoUpdateMode.Art:
                if (obj.artFields) return true;
                break;
            case ScoUpdateMode.Gameplay:
                if (obj.gameplayFields) return true;
                break;
            case ScoUpdateMode.Sound:
                if (obj.soundFields) return true;
                break;
            default:
                break;
        }
        return false;
    }

    static bool CompUsagePresence(TweekComponent comp, ScoUpdateMode updateMode)
    {
        switch (updateMode)
        {
            case ScoUpdateMode.Art:
                if (comp.artFields) return true;
                break;
            case ScoUpdateMode.Gameplay:
                if (comp.gameplayFields) return true;
                break;
            case ScoUpdateMode.Sound:
                if (comp.soundFields) return true;
                break;
            default:
                break;
        }
        return false;
    }

    static bool FieldUsagePresence(TweekField field, ScoUpdateMode updateMode)
    {
        switch (updateMode)
        {
            case ScoUpdateMode.Art:
                if (field.fieldUsage == FieldUsage.Art) return true;
                break;
            case ScoUpdateMode.Gameplay:
                if (field.fieldUsage == FieldUsage.Gameplay) return true;
                break;
            case ScoUpdateMode.Sound:
                if (field.fieldUsage == FieldUsage.Sound) return true;
                break;
            default:
                break;
        }
        return false;

    }

    public static void UpdateScoAssets()
    {
        string[] tempPaths = new string[0];

        List<object> gameplayScoAssets = new List<object>();
        List<object> artScoAssets = new List<object>();
        List<object> soundScoAssets = new List<object>();

        GameplayTweekScriptableObject gameplayScoRoot = ScriptableObject.CreateInstance<GameplayTweekScriptableObject>();
        ArtTweekScriptableObject artScoRoot = ScriptableObject.CreateInstance<ArtTweekScriptableObject>();
        SoundTweekScriptableObject soundScoRoot = ScriptableObject.CreateInstance<SoundTweekScriptableObject>();

        tempPaths = Directory.GetFiles(Data.gameplayAssetsDirectory);

        if (tempPaths.Length != 0)
        {
            foreach (string path in tempPaths)
            {
                if (Path.GetExtension(path) == ".asset" || Path.GetExtension(path) == ".ASSET")
                {
                    gameplayScoAssets.Add(AssetDatabase.LoadAssetAtPath(path, typeof(GameplayTweekScriptableObject)) as object);
                }
            }
        }

        tempPaths = Directory.GetFiles(Data.graphicAssetsDirectory);

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

        tempPaths = Directory.GetFiles(Data.soundAssetsDirectory);

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

            foreach (object asset in scoAssets)
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

                        CompAttribute compAttribute = field.GetCustomAttribute<CompAttribute>();

                        if(compAttribute != null)
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

    /// --------------------------------------------------------------------------------------------------------------------///
    ///               EXTRACTION FROM SCO ASSETS & APPLICATION TO OBJECT IN SCENE AND PROJECT'S PREFABS                     ///
    /// --------------------------------------------------------------------------------------------------------------------///

    static Dictionary<string, Dictionary<string, List<TweekField>>> UnpackScoObject(ScoUpdateMode scoUpdateMode, object scoAsset, Dictionary<string, Dictionary<string, List<TweekField>>> lastDictionary)
    {
        Dictionary<string, Dictionary<string, List<TweekField>>> compsToUpdate;
        if (lastDictionary != null) compsToUpdate = lastDictionary;
        else compsToUpdate = new Dictionary<string, Dictionary<string, List<TweekField>>>();

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

        string[] splitValue;
        string referencerId = string.Empty;
        string compId = string.Empty;
        string tempName = string.Empty;

        foreach (FieldInfo field in compFields)
        {
            if (field.GetCustomAttributes().ToArray().Length > 0)
            {
                IdAttribute idAttribute = field.GetCustomAttribute<IdAttribute>();

                if (idAttribute != null)
                {
                    if (!compsToUpdate.ContainsKey(field.GetValue(scoAsset).ToString())) compsToUpdate.Add(field.GetValue(scoAsset).ToString(), new Dictionary<string, List<TweekField>>());
                }                
            }
        }

        foreach (FieldInfo field in compFields)
        {
            if (field.GetCustomAttributes().ToArray().Length > 0)
            {
                CompAttribute compAttribute = field.GetCustomAttribute<CompAttribute>();

                if (compAttribute != null)
                {
                    splitValue = field.GetValue(scoAsset).ToString().Split(new char[] { '_' });
                    referencerId = splitValue[0];
                    compId = splitValue[1];

                    if (compsToUpdate.ContainsKey(referencerId)) compsToUpdate[referencerId].Add(compId, new List<TweekField>());
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
                    splitValue = field.Name.Split(new char[] { '_' });

                    tempName = splitValue[0];
                    referencerId = splitValue[1];
                    compId = splitValue[2];

                    splitValue = referencerId.Split(new char[] { 'µ' });
                    referencerId = string.Empty;
                    for (int i = 0; i < splitValue.Length; i++)
                    {                        
                        referencerId += splitValue[i];
                        if (i != splitValue.Length - 1) referencerId += "-";
                    }

                    splitValue = compId.Split(new char[] { 'µ' });
                    compId = string.Empty;
                    for (int i = 0; i < splitValue.Length; i++)
                    {                        
                        compId += splitValue[i];
                        if (i != splitValue.Length - 1) compId += "-";
                    }
                    //Debug.Log(referencerId + " " + compId); //


                    compsToUpdate[referencerId][compId].Add(new TweekField(field.FieldType, tempName, field.GetValue(scoAsset)));
                }
            }           
        }
        return compsToUpdate;
    }

    static Dictionary<string, Dictionary<string, List<TweekField>>> ApplySceneValues(Dictionary<string, Dictionary<string, List<TweekField>>> compsToUpdate)
    {
        int tempFieldCount = 0;
        int tempLeftField = 0;

        string originScene = EditorSceneManager.GetActiveScene().path;
        if (EditorSceneManager.GetActiveScene().isDirty) EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        string[] allScenesPaths = Directory.GetFiles(Data.scenesPath);

        foreach (string path in allScenesPaths)
        {
            if (Path.GetExtension(path) == ".unity" || Path.GetExtension(path) == ".UNITY")
            {
                EditorSceneManager.OpenScene(path);

                object[] allReferencerInScene = FindObjectsOfType(typeof(TweekReferencer));                
                if (allReferencerInScene.Length == 0) continue;

                string firstIndex = string.Empty;
                string secondIndex = string.Empty;

                foreach (TweekReferencer referencer in allReferencerInScene)
                {
                    firstIndex = new Guid(referencer.serializedGuid).ToString();
                    if (!compsToUpdate.ContainsKey(firstIndex)) continue;

                    foreach(TweekReference comp in referencer.referencedComponents)
                    {
                        secondIndex = new Guid(comp.serializedGuid).ToString();
                        if (!compsToUpdate[firstIndex].ContainsKey(secondIndex) || !comp.update) continue;

                        tempFieldCount = compsToUpdate[firstIndex][secondIndex].Count;
                        tempLeftField = UpdateFields(comp.component, compsToUpdate[firstIndex][secondIndex]);
                        compsToUpdate[firstIndex].Remove(secondIndex);

                        if (tempLeftField != 0)
                        {
                            Debug.Log(tempLeftField + " stored field's values (on a total of: " + tempFieldCount + ") were impossible to apply to Component: " + comp.component.GetType() + " in Scenes: " + path);
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

    static Dictionary<string, Dictionary<string, List<TweekField>>> ApplyPrefabValues(Dictionary<string, Dictionary<string, List<TweekField>>> compsToUpdate)
    {
        int tempFieldCount = 0;
        int tempLeftField = 0;

        string[][] allPrefabsPaths = new string[0][];
        string[] subDirectoriesPaths = new string[0];
        string indexPath;

        GameObject tempPrefab;
        object[] allReferencerOnPrefab;


        if (Directory.GetDirectories(Data.prefabsPath).Length > 0)
        {
            subDirectoriesPaths = Directory.GetDirectories(Data.prefabsPath);
            for (int i = 0; i < subDirectoriesPaths.Length; i++) subDirectoriesPaths[i] = PathWritter(subDirectoriesPaths[i]);

            allPrefabsPaths = new string[subDirectoriesPaths.Length + 1][];
            allPrefabsPaths[0] = Directory.GetFiles(Data.prefabsPath);
            for (int i = 1; i < allPrefabsPaths.Length; i++) allPrefabsPaths[i] = Directory.GetFiles(subDirectoriesPaths[i - 1]);
            for (int i = 0; i < allPrefabsPaths.Length; i++) for (int z = 0; z < allPrefabsPaths[i].Length; z++) allPrefabsPaths[i][z] = PathWritter(allPrefabsPaths[i][z]);
        }
        else
        {
            allPrefabsPaths = new string[1][];
            allPrefabsPaths[0] = Directory.GetFiles(Data.prefabsPath);
            for (int i = 0; i < allPrefabsPaths[0].Length; i++) allPrefabsPaths[0][i] = PathWritter(allPrefabsPaths[0][i]);
        }

        TweekReferencer.prefabEdition = true;

        for (int x = 0; x < allPrefabsPaths.Length; x++)
        {
            if (x == 0) indexPath = PathWritter(Data.prefabsPath);
            else indexPath = PathWritter(subDirectoriesPaths[x - 1]);

            for (int y = 0; y < allPrefabsPaths[x].Length; y++)
            {
                if (Path.GetExtension(allPrefabsPaths[x][y]) == ".prefab" || Path.GetExtension(allPrefabsPaths[x][y]) == ".PREFAB")
                {
                    string test = allPrefabsPaths[x][y];
                    tempPrefab = PrefabUtility.LoadPrefabContents(test);
                    allReferencerOnPrefab = tempPrefab.GetComponentsInChildren<TweekReferencer>();
                    if (allReferencerOnPrefab.Length == 0)
                    {
                        PrefabUtility.UnloadPrefabContents(tempPrefab);
                        continue;
                    }                       

                    string firstIndex = string.Empty;
                    string secondIndex = string.Empty;

                    foreach (TweekReferencer referencer in allReferencerOnPrefab)
                    {
                        firstIndex = new Guid(referencer.serializedGuid).ToString();
                        if (!compsToUpdate.ContainsKey(firstIndex)) continue;

                        foreach (TweekReference comp in referencer.referencedComponents)
                        {
                            secondIndex = new Guid(comp.serializedGuid).ToString();
                            if (!compsToUpdate[firstIndex].ContainsKey(secondIndex) || !comp.update) continue;

                            tempFieldCount = compsToUpdate[firstIndex][secondIndex].Count;
                            tempLeftField = UpdateFields(comp.component, compsToUpdate[firstIndex][secondIndex]);
                            compsToUpdate[firstIndex].Remove(secondIndex);

                            if (tempLeftField != 0)
                            {
                                Debug.Log(tempLeftField + " stored field's values (on a total of: " + tempFieldCount + ") were impossible to apply to Component: " + comp.component.GetType() + " on Prefab: " + allPrefabsPaths[x][y]);
                                Debug.Log("It could be due to a suppression of the fields or an error in the data collector system");
                            }
                        }
                    }

                    PrefabUtility.SaveAsPrefabAsset(tempPrefab, test);
                    PrefabUtility.UnloadPrefabContents(tempPrefab);
                }
            }
        }
        TweekReferencer.prefabEdition = false;
        return compsToUpdate;
    }

    static int UpdateFields(MonoBehaviour compToUpdate, List<TweekField> fieldInfosToApply)
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

    /// --------------------------------------------------------------------------------------------------------------------///
    ///                                                  UTILITY METHODS                                                    ///
    /// --------------------------------------------------------------------------------------------------------------------///

    static Dictionary<string, List<TweekObj>> CleanDictionary(Dictionary<string, List<TweekObj>> _dic)
    {
        Dictionary<string, List<TweekObj>> dic = _dic;

        for (int i = 0; i < dic.Count; i++)
        {
            if (dic.ElementAt(i).Value.Count == 0) dic.Remove(dic.ElementAt(i).Key);
        }

        return dic;
    }

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

    static string TypeWritter(Type type)
    {
        var codeDomProvider = CodeDomProvider.CreateProvider("C#");
        var typeReferenceExpression = new CodeTypeReferenceExpression(new CodeTypeReference(type));

        string[] splitTypeName = type.ToString().Split(new char[] { '.' });

        if (splitTypeName[0] == "System")
        {
            using (var writer = new StringWriter())
            {
                codeDomProvider.GenerateCodeFromExpression(typeReferenceExpression, writer, new CodeGeneratorOptions());
                return writer.GetStringBuilder().ToString();
            }
        }
        else
        {
            return splitTypeName[splitTypeName.Length - 1];
        }
    }

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
   
}
