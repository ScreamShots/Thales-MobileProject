using UnityEngine;
using UnityEditor;
using Tweek.ScoAttributes;

///<summary>

/// Rémi Sécher / 21.30.03 / Custom Editor drawers handling dispay of attributes for Tweek SCO 

///</summary>

[CustomPropertyDrawer(typeof(IdAttribute))]
public class IdDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var previousGUIState = GUI.enabled;
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, new GUIContent((attribute as IdAttribute).displayName));
        GUI.enabled = previousGUIState;
    }
}

[CustomPropertyDrawer(typeof(VarAttribute))]
public class VarDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, new GUIContent((attribute as VarAttribute).displayName));
    }
}

[CustomPropertyDrawer(typeof(PathAttribute))]
public class PathDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var previousGUIState = GUI.enabled;
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, new GUIContent((attribute as PathAttribute).displayName));
        GUI.enabled = previousGUIState;
    }
}
