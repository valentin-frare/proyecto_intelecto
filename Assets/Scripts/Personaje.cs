
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public enum Paises { Argentina, India, Italia, Holanda, Sudafrica, Egipto, EstadosUnidos, Australia, Japon, Francia };

public class ReadOnlyAttribute : PropertyAttribute {}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

[CreateAssetMenu(fileName = "Personaje", menuName = "proyecto_intelecto/Personajito", order = 0)]
public class Personaje : ScriptableObject
{
    [ReadOnly] public string nombre;
    public Paises pais;
    public Sprite icono;
    public Animator animator;

    #if UNITY_EDITOR
    private void OnValidate() {
        string assetPath =  AssetDatabase.GetAssetPath(this.GetInstanceID());     
        nombre = Path.GetFileNameWithoutExtension(assetPath);
    }
    #endif
}
