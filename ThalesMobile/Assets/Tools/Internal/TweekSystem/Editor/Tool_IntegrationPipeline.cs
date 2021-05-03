using UnityEngine;
using UnityEditor;

/// <summary>
/// S'il y a un problème, c'est la faute de Karp
/// PS : j'améliorerais peut être le visuel si je trouve du temps mais actu c'est fonctionnel
/// </summary>
public class Tool_IntegrationPipeline : EditorWindow
{
    public enum Mode { None, Graphic, Balancing , Audio}
    public Mode actualMode = Mode.None;

    private static IntePipeSettingsData Data
    {
        get { return IntePipeSettingsData.GetOrCreateSettings(); }
    }

    public static ArtTweekScriptableObject graphic_SCO = null;
    public static GameplayTweekScriptableObject Balancing_SCO = null;
    public static SoundTweekScriptableObject sound_SCO = null;
    #region SCO Tampon, lastState et son Editor
    private static ArtTweekScriptableObject tempGraphic_SCO = null;
    private static GameplayTweekScriptableObject tempBalancing_SCO = null;
    private static SoundTweekScriptableObject tempSound_SCO = null;

    private static ArtTweekScriptableObject lastGraphic_SCO = null;
    private static GameplayTweekScriptableObject lastBalancing_SCO = null;
    private static SoundTweekScriptableObject lastSound_SCO = null;

    private static Editor graphic_SCOEditor;
    private static Editor Balancing_SCOEditor;
    private static Editor sound_SCOEditor;
    #endregion

    private static bool isCompilling = false;

    private Vector2 scrollPosition;

    [MenuItem("Tools/IntegrationPipeline", false, -999999999)]
    public static void ShowWindow()
    {
        EditorWindow myWindow = GetWindow(typeof(Tool_IntegrationPipeline));
        myWindow.minSize = new Vector2(300,200);
    }

    private void OnEnable()
    {
        isCompilling = false;

        ReinitialisationTempSCO(Mode.Graphic);
        ReinitialisationTempSCO(Mode.Balancing);
        ReinitialisationTempSCO(Mode.Audio);
    }

    [UnityEditor.Callbacks.DidReloadScripts()]
    private static void ReloadEditorDrawer()
    {
        isCompilling = false;

        ReinitialisationTempSCO(Mode.Graphic);
        ReinitialisationTempSCO(Mode.Balancing);
        ReinitialisationTempSCO(Mode.Audio);
    }

    private static void ReinitialisationTempSCO(Mode mode)
    {
        //Temp Creation
#pragma warning disable
        switch (mode)
        {
            case Mode.Graphic:
                tempGraphic_SCO = ScriptableObject.CreateInstance<ArtTweekScriptableObject>();
                break;
            case Mode.Balancing:
                tempBalancing_SCO = ScriptableObject.CreateInstance<GameplayTweekScriptableObject>();
                break;
            case Mode.Audio:
                tempSound_SCO = ScriptableObject.CreateInstance<SoundTweekScriptableObject>();
                break;
            default:
                break;
        }
#pragma warning restore

        //Graphic Editor regenerate
        switch (mode)
        {
            case Mode.Graphic:

                if (graphic_SCO != null)
                {
                    graphic_SCOEditor = Editor.CreateEditor(graphic_SCO);
                }
                else
                {
                    graphic_SCOEditor = Editor.CreateEditor(ScriptableObject.CreateInstance<ArtTweekScriptableObject>());
                }
                break;

            case Mode.Balancing:

                if (Balancing_SCO != null)
                {
                    Balancing_SCOEditor = Editor.CreateEditor(Balancing_SCO);
                }
                else
                {
                    Balancing_SCOEditor = Editor.CreateEditor(ScriptableObject.CreateInstance<GameplayTweekScriptableObject>());
                }

                break;
            case Mode.Audio:

                if (sound_SCO != null)
                {
                    sound_SCOEditor = Editor.CreateEditor(sound_SCO);
                }
                else
                {
                    sound_SCOEditor = Editor.CreateEditor(ScriptableObject.CreateInstance<SoundTweekScriptableObject>());
                }

                break;
            default:
                break;
        }
    }
    private void ReDrawSCOEditors()
    {
        //Je regénère l'Editor du nouveau SCO et refres le lastSCO
        if (graphic_SCO != null)
        {
            graphic_SCOEditor = Editor.CreateEditor(graphic_SCO);
        }
        else
        {
            graphic_SCOEditor = Editor.CreateEditor(tempGraphic_SCO);
        }
        lastGraphic_SCO = graphic_SCO;

        if (Balancing_SCO != null)
        {
            Balancing_SCOEditor = Editor.CreateEditor(Balancing_SCO);
        }
        else
        {
            Balancing_SCOEditor = Editor.CreateEditor(tempBalancing_SCO);
        }
        lastBalancing_SCO = Balancing_SCO;

        if (sound_SCO != null)
        {
            sound_SCOEditor = Editor.CreateEditor(sound_SCO);
        }
        else
        {
            sound_SCOEditor = Editor.CreateEditor(tempSound_SCO);
        }
        lastSound_SCO = sound_SCO;
    }

    private void OnGUI()
    {
        //Si je perd la référence (SCO supprimé)
        if(graphic_SCO == null || Balancing_SCO == null ||sound_SCO == null)
        {
            graphic_SCO = tempGraphic_SCO;
            Balancing_SCO = tempBalancing_SCO;
            sound_SCO = tempSound_SCO;
        }
        //Si la valeur Change
        if (lastGraphic_SCO != graphic_SCO || lastBalancing_SCO != Balancing_SCO || lastSound_SCO != sound_SCO)
        {
            ReDrawSCOEditors();
        }

        //ReCalcSCO on Projet
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            float _minWidth = 90;
            float _height = 30;
            //All SCO Rebuild
            if (GUILayout.Button("Rebuild All SCO for Project", GUILayout.MinWidth(_minWidth * 3), GUILayout.Height(_height)))
            {
                isCompilling = true;
                TweekCore.LaunchScoUpdate(TweekCore.ScoUpdateMode.Global);
            }
            using (new GUILayout.HorizontalScope())
            {
                //Graphic SCO Rebuild
                if (GUILayout.Button("Rebuild only Graphic", GUILayout.MinWidth(_minWidth), GUILayout.Height(_height)))
                {
                    isCompilling = true;
                    TweekCore.LaunchScoUpdate(TweekCore.ScoUpdateMode.Art);
                }
                //Balancing SCO Rebuild
                if (GUILayout.Button("Rebuild only Balancing", GUILayout.MinWidth(_minWidth), GUILayout.Height(_height)))
                {
                    isCompilling = true;
                    TweekCore.LaunchScoUpdate(TweekCore.ScoUpdateMode.Gameplay);
                }
                //Sound SCO Rebuild
                if (GUILayout.Button("Rebuild only Sound", GUILayout.MinWidth(_minWidth), GUILayout.Height(_height)))
                {
                    isCompilling = true;
                    TweekCore.LaunchScoUpdate(TweekCore.ScoUpdateMode.Sound);
                }
            }
        }

        //Zone Input du SCO à edit
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            float _width = 140;
            //Graphic Emplacement
            using (new GUILayout.HorizontalScope())
            {
                //Nom
                GUILayout.Label("graphic_SCO", actualMode == Mode.Graphic ? EditorStyles.boldLabel : EditorStyles.label, GUILayout.Width(_width));
                //Scriptable Object
                graphic_SCO = (ArtTweekScriptableObject)EditorGUILayout.ObjectField(graphic_SCO, typeof(ArtTweekScriptableObject), false);
                //Button New SCO
                if (GUILayout.Button("New" , GUILayout.Width(60)))
                {
                    GenerateNewSco<ArtTweekScriptableObject>(tempGraphic_SCO, Mode.Graphic);
                }
            }
            //Balancing Emplacement
            using (new GUILayout.HorizontalScope())
            {
                //Nom
                GUILayout.Label("Balancing_SCO", actualMode == Mode.Balancing ? EditorStyles.boldLabel : EditorStyles.label, GUILayout.Width(_width));
                //Scriptable Object
                Balancing_SCO = (GameplayTweekScriptableObject)EditorGUILayout.ObjectField(Balancing_SCO, typeof(GameplayTweekScriptableObject), false);
                //Button New SCO
                if (GUILayout.Button("New", GUILayout.Width(60)))
                {
                    GenerateNewSco<GameplayTweekScriptableObject>(tempBalancing_SCO, Mode.Balancing);
                }
            }
            //Sound Emplacement
            using (new GUILayout.HorizontalScope())
            {
                //Nom
                GUILayout.Label("sound_SCO", actualMode == Mode.Audio ? EditorStyles.boldLabel : EditorStyles.label, GUILayout.Width(_width));
                //Scriptable Object
                sound_SCO = (SoundTweekScriptableObject)EditorGUILayout.ObjectField(sound_SCO, typeof(SoundTweekScriptableObject), false);
                //Button New SCO
                if (GUILayout.Button("New", GUILayout.Width(60)))
                {
                    GenerateNewSco<SoundTweekScriptableObject>(tempSound_SCO, Mode.Audio);
                }
            }
        }
        GUILayout.Space(10);

        GUILayout.Label("Mode");
        using(new GUILayout.HorizontalScope())
        {
            ModeButton("Graphic", Mode.Graphic);
            ModeButton("Equilibrage", Mode.Balancing);
            ModeButton("Sound", Mode.Audio);
        }

        //Debug show in what mode i am
        GUILayout.Label(actualMode.ToString(), EditorStyles.helpBox);

        //Draw SCO Information
        if (!isCompilling)
        {
            //Draw SCO Information
            using (var scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition, EditorStyles.helpBox))
            {
                scrollPosition = scrollViewScope.scrollPosition;

                switch (actualMode)
                {
                    case Mode.None:
                        GUILayout.Label("Select a mode");
                        break;

                    case Mode.Graphic:
                        graphic_SCOEditor.OnInspectorGUI();
                        break;

                    case Mode.Balancing:
                        Balancing_SCOEditor.OnInspectorGUI();
                        break;

                    case Mode.Audio:
                        sound_SCOEditor.OnInspectorGUI();
                        break;

                    default:
                        GUILayout.TextField("!!!Error!!!");
                        break;
                }
            }
        }
        else
        {
            actualMode = Mode.None;
            GUILayout.Label("The Scriptables Objects are compilling, please wait");
            GUILayout.Space(20);
            GUILayout.Label("Fun Fact : " +
                "\n Arthur essaie de vous divertir en attendant de pouvoir travailler" +
                "\n " +
                "\n J'espère que ça ne va pas être le giga rush en fin de projet, " +
                "\n si c'est le cas... bah RIP" +
                "\n TRAVAIL PLUS VITE !!!" +
                "\n " +
                "\n Nan je déconne, faut attendre que ça recompille" +
                "\n " +
                "\n Imagine une petite musique d'ascenseur ");
        }

        GUILayout.FlexibleSpace();
        GUILayout.Space(20);
        if (GUILayout.Button("ApplyValue", GUILayout.Height(30)))
        {
            ApplyValues();
        }
    }

    void ModeButton(string name, Mode mode)
    {
        GUIContent content = new GUIContent(name);

        var oldBackGroundColor = GUI.backgroundColor;
        var oldContentColor = GUI.contentColor;

        if (actualMode == mode)
        {
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
            GUI.contentColor = Color.white;
        }

        if (GUILayout.Button(content, GetButtonStyle(), GUILayout.Height(40), GUILayout.MinWidth(90)))
        {
            actualMode = actualMode == mode ? Mode.None : mode;

            switch (actualMode)
            {
                case Mode.None:

                    break;

                case Mode.Graphic:

                    break;

                case Mode.Balancing:

                    break;

                case Mode.Audio:

                    break;

                default:
                    Debug.LogError("Problème : Je ne suis pas dans un mode connue", this);
                    break;
            }

        }

        GUI.backgroundColor = oldBackGroundColor;
        GUI.contentColor = oldContentColor;
    }
    GUIStyle GetButtonStyle()
    {
        var s = new GUIStyle(GUI.skin.button);
        s.margin.left = 0;
        s.margin.top = 0;
        s.margin.right = 0;
        s.margin.bottom = 0;
        s.border.left = 0;
        s.border.top = 0;
        s.border.right = 0;
        s.border.bottom = 0;
        return s;
    }

    private void GenerateNewSco<T>(T tempSCO, Mode mode) where T : ScriptableObject
    {
        //Génère l'instance
        T asset = CreateInstance<T>();
        asset = tempSCO;

        //Trouve le Path
        string path;
        switch (mode)
        {
            case Mode.Graphic:
                path = Data.graphicAssetsDirectory + "/";
                break;
            case Mode.Balancing:
                path = Data.gameplayAssetsDirectory + "/";
                break;
            case Mode.Audio:
                path = Data.soundAssetsDirectory + "/";
                break;
            default:
                path = "Assets" + "/";
                break;
        }
        path += "NewGraphSCO.asset";

        //Create The new Asset
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        //Remplace the Hole
        ReinitialisationTempSCO(Mode.Graphic);
        ReinitialisationTempSCO(Mode.Balancing);
        ReinitialisationTempSCO(Mode.Audio);
    }
    public void ApplyValues()
    {
        string actuGraphicSCOPath = AssetDatabase.GetAssetPath(Balancing_SCO);
        object graphicSCOAsset = graphic_SCO != null || graphic_SCO != tempGraphic_SCO ? AssetDatabase.LoadAssetAtPath(actuGraphicSCOPath, typeof(ArtTweekScriptableObject)) as object : null;

        string actuBalancingSCOPath = AssetDatabase.GetAssetPath(Balancing_SCO);
        object BalancingSCOAsset = Balancing_SCO != null || Balancing_SCO != tempBalancing_SCO ? AssetDatabase.LoadAssetAtPath(actuBalancingSCOPath, typeof(GameplayTweekScriptableObject)) as object : null;

        string actuSoundSCOPath = AssetDatabase.GetAssetPath(Balancing_SCO);
        object soundSCOAsset = sound_SCO != null || sound_SCO != tempSound_SCO ? AssetDatabase.LoadAssetAtPath(actuSoundSCOPath, typeof(SoundTweekScriptableObject)) as object : null;

        TweekCore.LaunchValuesApplication(BalancingSCOAsset, graphicSCOAsset, soundSCOAsset);
    }
}