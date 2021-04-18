using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

///<summary>

/// Rémi Sécher / 21.30.03 / MonoBehaviour extension use for the TweekSystem, generate unique ID for object. 

///</summary>

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public abstract class TweekMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    Guid guid = Guid.Empty;
    [HideInInspector]
    public byte[] serializedGuid;

    [SerializeField]
    string guidDisplay;

#if UNITY_EDITOR
    public enum TweekState { InhertiedValues, ProperValues }
    public enum ObjectState { StandAlone, PrefabInstance, PrefabRoot, PrefabVariant, }

    [Header("Tweeking Parameters")]
    public TweekState valuesTweekState = TweekState.ProperValues;

    [SerializeField, HideInInspector]
    ObjectState thisObjState = ObjectState.StandAlone;
    [SerializeField, HideInInspector]
    ObjectState initState = ObjectState.StandAlone;

    

    public static bool prefabEdition;
    bool lateInit;    
    bool initDone = false;

    bool isPrefabWindow = false;

    [SerializeField, HideInInspector]
    int instanceID = 0;
    PrefabAssetType prefabType;
#endif

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        //detect current environnement of the object
        if (PrefabStageUtility.GetCurrentPrefabStage() != null) isPrefabWindow = true;
        else isPrefabWindow = false;

        //duplication in scene support 
        EditorSceneManager.sceneClosed += ResetInstanceID;  
        CheckInstanceID();

        //Regular unique ID setup
        if(!prefabEdition && !initDone) GuidInit();
#endif
    }

    protected virtual void Start()
    {
#if UNITY_EDITOR
        //set the scene dirty if modification were done
        //can do it in awake because Unity
        //consider looking at Unity Editor Coroutine package to upgrade the system
        if (lateInit)
        {
            lateInit = false;
            EditorUtility.SetDirty(this);
        }
#endif
    }

    public void OnBeforeSerialize()
    {
        if (guid != Guid.Empty)
        {
            serializedGuid = guid.ToByteArray();
        }
    }

    //from ISerializationCallbackReceiver, init GUID with seriliaed value when loading this
    public void OnAfterDeserialize()
    {
        if (serializedGuid != null && serializedGuid.Length == 16)
        {
            guid = new Guid(serializedGuid);
            guidDisplay = guid.ToString();
        }
    }

#if UNITY_EDITOR
    //from ISerializationCallbackReceiver, transform GUID into serializable value before unloading this


    //regular GUID Init on object Awake
    public void GuidInit()
    {
        //test if object's nature has changed
        prefabType = PrefabUtility.GetPrefabAssetType(gameObject);
        StateCheck();

        //if object's nature changed, update and create a new GUID
        if (thisObjState != initState && serializedGuid != null)
        {
            StateUpdate();
            initState = thisObjState;
            //lateInit = true;

            if (!prefabEdition) lateInit = true;
        }
        //if GUID has never been generated or is invalid, create a new one
        else if (serializedGuid == null || serializedGuid.Length != 16)
        {
            NewGuid();
        }
        //else restore GUID from serialized value
        else if (guid == Guid.Empty)
        {
            guid = new Guid(serializedGuid);
            guidDisplay = guid.ToString();
        }
    }

    //force a new value on GUID
    void NewGuid()
    {
        //print("New Guid " + gameObject.name + " " + this.GetType().ToString());
        guid = Guid.NewGuid();
        serializedGuid = guid.ToByteArray();
        guidDisplay = guid.ToString();
    }

    //check if InstanceID is different from the last stored one (in scene object's duplication support)
    void CheckInstanceID()
    {
        if (instanceID != GetInstanceID())
        {
            if (instanceID == 0) instanceID = GetInstanceID();
            else
            {
                instanceID = GetInstanceID();
                if (SceneEventCaller.isSceneOpen && !isPrefabWindow && thisObjState != ObjectState.PrefabRoot && thisObjState != ObjectState.PrefabVariant)
                {
                    NewGuid();
                    initDone = true;
                }
            }
        }
    }

    //force ID to reset when quitting scene (in scene object's duplication support)
    void ResetInstanceID(Scene scene)
    {
        instanceID = 0;
    }

    //check object's nature
    void StateCheck()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage() != null || prefabEdition)
        {
            switch (prefabType)
            {
                case PrefabAssetType.Regular:
                    thisObjState = ObjectState.PrefabVariant;
                    break;
                case PrefabAssetType.Variant:
                    thisObjState = ObjectState.PrefabVariant;
                    break;
                case PrefabAssetType.NotAPrefab:
                    thisObjState = ObjectState.PrefabRoot;
                    break;
                default:
                    print("Unsopported type of prefab (" + prefabType + ") - " + gameObject.name);
                    break;
            }
        }
        else
        {
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                switch (prefabType)
                {
                    case PrefabAssetType.Regular:
                        thisObjState = ObjectState.PrefabRoot;
                        break;
                    case PrefabAssetType.Variant:
                        thisObjState = ObjectState.PrefabVariant;
                        break;
                    default:
                        print("Unsopported type of prefab (" + prefabType + ") - " + gameObject.name);
                        break;
                }
            }
            else
            {
                switch (prefabType)
                {
                    case PrefabAssetType.Regular:
                        thisObjState = ObjectState.PrefabInstance;
                        break;
                    case PrefabAssetType.Variant:
                        thisObjState = ObjectState.PrefabInstance;
                        break;
                    case PrefabAssetType.NotAPrefab:
                        thisObjState = ObjectState.StandAlone;
                        break;
                    default:
                        print("Unsopported type of prefab (" + prefabType + ")  - " + gameObject.name);
                        break;
                }
            }
        }
    }

    //update object's nature and force new value on GUID
    //some transition between two natures may be unsupported
    //if a "not supported error" is throw back in the console inform the referent
    void StateUpdate()
    {
        switch (initState)
        {
            case ObjectState.PrefabInstance:
                if (thisObjState == ObjectState.PrefabVariant || thisObjState == ObjectState.PrefabRoot) NewGuid();
                else if (thisObjState != ObjectState.StandAlone) print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;

            case ObjectState.PrefabRoot:
                if (thisObjState == ObjectState.PrefabInstance || thisObjState == ObjectState.PrefabVariant) NewGuid();
                else print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;

            case ObjectState.PrefabVariant:
                if (thisObjState == ObjectState.PrefabInstance) NewGuid();
                else if (thisObjState != ObjectState.PrefabRoot) print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;

            case ObjectState.StandAlone:
                if (thisObjState == ObjectState.PrefabRoot) NewGuid();
                else if (thisObjState != ObjectState.PrefabInstance) print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;
        }
    }

    //force a new GUID value through inspector
    [ContextMenu("Tweek/Force New Guid")]
    void ForceCreateNewGuid()
    {
        NewGuid();
        EditorUtility.SetDirty(this);
    }
#endif
}

