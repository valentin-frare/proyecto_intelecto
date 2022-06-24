using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

[Serializable]
public class AtaqueDebil {
    public AnimationClip animation;
}

[Serializable]
public class AtaqueNormal {
    public AnimationClip animation;
}

[Serializable]
public class AtaqueFuerte {
    public AnimationClip animation;
}

[CreateAssetMenu(fileName = "AtaquesDeUnPj", menuName = "proyecto_intelecto/AtaquePj", order = 0)]
public class AtaquePj : ScriptableObject {
    public AtaqueDebil ataqueDebil;
    public AtaqueNormal ataqueNormal;
    public AtaqueFuerte ataqueFuerte;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AtaqueDebil))]
public class AtaqueDebilUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorStyles.label.richText = true;
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = -9;

        var debilRect = new Rect(position.x, position.y, position.width, position.height);

        GUI.color = Color.green;
        EditorGUI.PropertyField(debilRect, property.FindPropertyRelative("animation"), GUIContent.none);
        GUI.color = Color.white;

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}


[CustomPropertyDrawer(typeof(AtaqueNormal))]
public class AtaqueNormalUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorStyles.label.richText = true;
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = -9;

        var debilRect = new Rect(position.x, position.y, position.width, position.height);

        GUI.color = Color.yellow;
        EditorGUI.PropertyField(debilRect, property.FindPropertyRelative("animation"), GUIContent.none);
        GUI.color = Color.white;

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(AtaqueFuerte))]
public class AtaqueFuerteUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorStyles.label.richText = true;
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = -9;

        var debilRect = new Rect(position.x, position.y, position.width, position.height);

        GUI.color = Color.red;
        EditorGUI.PropertyField(debilRect, property.FindPropertyRelative("animation"), GUIContent.none);
        GUI.color = Color.white;

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif