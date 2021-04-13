#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>

/// Rémi Sécher / 21.30.03 / Editor tools that detected whenever a scene is closing or openning

/// </summary>

[ExecuteInEditMode]
public class SceneEventCaller
{
    //if this bool is true mean that no scene is open in editor (so a scene is loading) 
    public static bool isSceneOpen = false;

    //add custom method to Unity Editor's delegates that handle scene closing and openning
    [InitializeOnLoadMethod]
    static void AddListenersSceneEvent()
    {
        EditorSceneManager.sceneOpened += SceneOpenedCallback;
        EditorSceneManager.sceneClosed += SceneClosedCallback;
    }

    //what to do when a scene is closing
    static void SceneOpenedCallback(Scene _scene, OpenSceneMode _mode)
    {
        //Debug.Log("SceneOpen");
        isSceneOpen = true;
    }

    //what to do when a scene is openning
    static void SceneClosedCallback(Scene scene)
    {
        //Debug.Log("SceneClose");
        isSceneOpen = false;
    }
}
#endif
