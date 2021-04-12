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

    public ArtTweekScriptableObject graphic_SCO = null;
    public GameplayTweekScriptableObject Balancing_SCO = null;
    public SoundTweekScriptableObject sound_SCO = null;
    #region SCO Tampon, lastState et son Editor
    private ArtTweekScriptableObject tempGraphic_SCO = null;
    private GameplayTweekScriptableObject tempBalancing_SCO = null;
    private SoundTweekScriptableObject tempSound_SCO = null;

    private ArtTweekScriptableObject lastGraphic_SCO = null;
    private GameplayTweekScriptableObject lastBalancing_SCO = null;
    private SoundTweekScriptableObject lastSound_SCO = null;

    private Editor graphic_SCOEditor;
    private Editor Balancing_SCOEditor;
    private Editor sound_SCOEditor;
    #endregion

    private Vector2 scrollPosition;

    [MenuItem("Tools/IntegrationPipeline", false, -999999999)]
    public static void ShowWindow()
    {
        EditorWindow myWindow = GetWindow(typeof(Tool_IntegrationPipeline));
        myWindow.minSize = new Vector2(300,200);
    }

    private void OnEnable()
    {
        #pragma warning disable
        tempSound_SCO = new SoundTweekScriptableObject();
        tempGraphic_SCO = new ArtTweekScriptableObject();
        tempBalancing_SCO = new GameplayTweekScriptableObject();
        #pragma warning restore

        //Initialisation des Editors
        if (graphic_SCO != null)
        {
            graphic_SCOEditor = Editor.CreateEditor(graphic_SCO);
        }
        else
        {
            graphic_SCOEditor = Editor.CreateEditor(tempGraphic_SCO);
        }
        
        if (Balancing_SCO != null)
        {
            Balancing_SCOEditor = Editor.CreateEditor(Balancing_SCO);
        }
        else
        {
            Balancing_SCOEditor = Editor.CreateEditor(new GameplayTweekScriptableObject());
        }

        if (sound_SCO != null)
        {
            sound_SCOEditor = Editor.CreateEditor(sound_SCO);
        }
        else
        {
            sound_SCOEditor = Editor.CreateEditor(new SoundTweekScriptableObject());
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
                TweekCore.LaunchScoUpdate(TweekCore.ScoUpdateMode.Global);
            }
            using (new GUILayout.HorizontalScope())
            {
                //Graphic SCO Rebuild
                if (GUILayout.Button("Rebuild only Graphic", GUILayout.MinWidth(_minWidth), GUILayout.Height(_height)))
                {
                    TweekCore.LaunchScoUpdate(TweekCore.ScoUpdateMode.Art);
                }
                //Balancing SCO Rebuild
                if (GUILayout.Button("Rebuild only Balancing", GUILayout.MinWidth(_minWidth), GUILayout.Height(_height)))
                {
                    TweekCore.LaunchScoUpdate(TweekCore.ScoUpdateMode.Gameplay);
                }
                //Sound SCO Rebuild
                if (GUILayout.Button("Rebuild only Sound", GUILayout.MinWidth(_minWidth), GUILayout.Height(_height)))
                {
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

    private void GenerateNewSco<T>(T tempSCO, Mode mode) where T : UnityEngine.ScriptableObject
    {
        //Génère l'instance
        T asset = CreateInstance<T>();
        asset = tempSCO;
        switch (mode)
        {
            case Mode.Graphic:
                tempGraphic_SCO = new ArtTweekScriptableObject();
                break;

            case Mode.Balancing:
                tempBalancing_SCO = new GameplayTweekScriptableObject();
                break;

            case Mode.Audio:
                tempSound_SCO = new SoundTweekScriptableObject();
                break;

            default:
                tempGraphic_SCO = new ArtTweekScriptableObject();
                tempBalancing_SCO = new GameplayTweekScriptableObject();
                tempSound_SCO = new SoundTweekScriptableObject();
                break;
        }

        //Trouve le Path
        string path;
        switch (mode)
        {
            case Mode.Graphic:
                path = TweekCore.graphicAssetsDirectory + "/";
                break;
            case Mode.Balancing:
                path = TweekCore.gameplayAssetsDirectory + "/";
                break;
            case Mode.Audio:
                path = TweekCore.soundAssetsDirectory + "/";
                break;
            default:
                path = "Assets" + "/";
                break;
        }
        path += "NewGraphSCO.asset";

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
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