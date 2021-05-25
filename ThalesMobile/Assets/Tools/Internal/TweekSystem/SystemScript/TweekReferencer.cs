using UnityEngine;
using System;
using NaughtyAttributes;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Tweek.FlagAttributes;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif 

#if UNITY_EDITOR
[Serializable]
public struct TweekReference
{
    public MonoBehaviour component;
    [HideInInspector]
    public byte[] serializedGuid;
    public bool update;

    public string guidDisplay
    {
        get
        {
            if (serializedGuid != null && serializedGuid.Length == 16) return new Guid(serializedGuid).ToString();
            else return string.Empty;
        }
    }


    public TweekReference(MonoBehaviour _component)
    {
        component = _component;
        serializedGuid = Guid.NewGuid().ToByteArray();
        update = true;
    }

    public bool TestCurrent(MonoBehaviour holder)
    {
        Attribute[] attr = null;
        bool tagged = false;
        bool findInParents = false;

        if (component != null)
        {
            GameObject tempGO = component.gameObject;

            while (tempGO != component.transform.root.gameObject)
            {
                if(tempGO == holder.gameObject)
                {
                    findInParents = true;
                    break;
                }
                tempGO = tempGO.transform.parent.gameObject;
            }

            if(tempGO == holder.gameObject) findInParents = true;

            if (!findInParents)
            {
                Debug.Log("A component referenced on TweekReferencer is not located on the same GO as the referencer");
                Debug.Log("The reference will be reset. Source: " + holder.gameObject.name + " Component: " + component.GetType().ToString());

                return (true);
            }

            attr = Attribute.GetCustomAttributes(component.GetType());            

            if (attr.Length > 0)
            {
                foreach(Attribute at in attr)
                {
                    if(at is TweekClassAttribute)
                    {
                        tagged = true;
                        break;
                    }
                }
                if (!tagged) return (true);
            }
            else return (true);
        }
        else return (true);

        return (false);
    }
}



[ExecuteInEditMode]
#endif
public class TweekReferencer : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField, ReadOnly]
    string guidDisplay;
    [HideInInspector]
    public byte[] serializedGuid;
    Guid guid;

#if UNITY_EDITOR
    public enum ObjectState { StandAlone, PrefabInstance, PrefabRoot, PrefabVariant, }

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

    public List<TweekReference> referencedComponents = new List<TweekReference>();

    [Button("Update References")]
    public void UpdateReferences()
    {
        MonoBehaviour[] _references = GetComponentsInChildren<MonoBehaviour>();

        foreach(MonoBehaviour reference in _references)
        {
            foreach (Attribute attr in Attribute.GetCustomAttributes(reference.GetType()))
            {
                if(attr is TweekClassAttribute)
                {
                    referencedComponents.Add(new TweekReference(reference));
                    break;
                }
            }
        }

        foreach (TweekReference reference in referencedComponents.ToList())
        {
            if (reference.TestCurrent(this)) referencedComponents.Remove(reference);
        }
    }

#endif

    private void Awake()
    {
#if UNITY_EDITOR
        foreach(TweekReference reference in referencedComponents.ToList())
        {
            if (reference.TestCurrent(this))
            {
                referencedComponents.Remove(reference);
                lateInit = true;
            }                
        }

        if (PrefabStageUtility.GetCurrentPrefabStage() != null) isPrefabWindow = true;
        else isPrefabWindow = false;

        EditorSceneManager.sceneClosed += ResetInstanceID;
        CheckInstanceID();

        if (!prefabEdition && !initDone) GuidInit();
#endif
    }

    private void Start()
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

#if UNITY_EDITOR
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
            if (!SceneEventCaller.isSceneOpen) lateInit = true;
            NewGuid();
        }
        //else restore GUID from serialized value
        else if (guid == Guid.Empty)
        {
            guid = new Guid(serializedGuid);
            guidDisplay = guid.ToString();
        }
    }

    void NewGuid()
    {
        //print("New Guid " + gameObject.name + " " + this.GetType().ToString());

        guid = Guid.NewGuid();
        serializedGuid = guid.ToByteArray();
        guidDisplay = guid.ToString();
    }

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

    void StateUpdate()
    {
        switch (initState)
        {
            case ObjectState.PrefabInstance:
                if (thisObjState == ObjectState.PrefabVariant || thisObjState == ObjectState.PrefabRoot) NewGuid();
                //else if (thisObjState != ObjectState.StandAlone) print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;

            case ObjectState.PrefabRoot:
                if (thisObjState == ObjectState.PrefabInstance || thisObjState == ObjectState.PrefabVariant) NewGuid();
                //else print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;

            case ObjectState.PrefabVariant:
                if (thisObjState == ObjectState.PrefabInstance) NewGuid();
                //else if (thisObjState != ObjectState.PrefabRoot) print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;

            case ObjectState.StandAlone:
                if (thisObjState == ObjectState.PrefabRoot) NewGuid();
                //else if (thisObjState != ObjectState.PrefabInstance) print("Unsupported exeption for object state transition from: " + initState + " to:" + thisObjState + " - " + gameObject.name);
                break;
        }
    }

    void ResetInstanceID(Scene scene)
    {
        instanceID = 0;
    }

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
#endif

    //from ISerializationCallbackReceiver, transform GUID into serializable value before unloading this
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

}
