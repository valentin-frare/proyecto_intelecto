using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[Serializable]
public class Respuesta
{
    public string respuesta = "Aca va tu respuesta";
    public bool correcta = false;
}

[Serializable]
public class Pregunta
{
    public string pregunta = "Aca va tu pregunta";
    public int id = 0;
    public Respuesta[] respuestas;
    [HideInInspector]
    public bool[] previusChanges = new bool[4];
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Respuesta))]
public class RespuestaDrawerUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorStyles.label.richText = true;
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var respuestaRect = new Rect(position.x, position.y, position.width - 20, position.height);
        var correctaRect = new Rect(position.x + respuestaRect.width + 5, position.y, 20, position.height);

        GUI.color = property.FindPropertyRelative("correcta").boolValue ? Color.green : Color.red;
        EditorGUI.PropertyField(respuestaRect, property.FindPropertyRelative("respuesta"), GUIContent.none);
        EditorGUI.PropertyField(correctaRect, property.FindPropertyRelative("correcta"), GUIContent.none);
        GUI.color = Color.white;

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif

[CreateAssetMenu(fileName = "SetDePreguntas", menuName = "proyecto_intelecto/Pregunta3", order = 0)]
public class SetDePreguntas : ScriptableObject {
    public Pregunta[] Preguntas;

    private void OnValidate() {
        for (int i = 0; i < Preguntas.Length; i++)
        {
            Preguntas[i].id = i;
        }

        foreach (Pregunta pregunta in Preguntas)
        {
            if (pregunta.respuestas.Length < 4) {
                Array.Resize(ref pregunta.respuestas, 4);
            }
            if (pregunta.respuestas.Length > 4) {
                Array.Resize(ref pregunta.respuestas, 4);
            }

            int correctCount = 0;
            foreach (Respuesta respuesta in pregunta.respuestas)
            {
                if (respuesta.correcta) correctCount++;
            }
            
            if (correctCount > 1) {
                for (int i = 0; i < pregunta.respuestas.Length; i++)
                {
                    if (pregunta.respuestas[i].correcta != pregunta.previusChanges[i]) {
                        if (pregunta.respuestas[i].correcta == true && pregunta.previusChanges[i] == false) {
                            for (int x = 0; x < pregunta.respuestas.Length; x++)
                            {
                                if (pregunta.respuestas[x] != pregunta.respuestas[i])
                                    pregunta.respuestas[x].correcta = false;
                            }
                        }
                        break;
                    }
                }
            }

            int count = 0;
            foreach (Respuesta respuesta in pregunta.respuestas)
            {
                if(pregunta.previusChanges.Length <= 3) pregunta.previusChanges = new bool[4];
                
                pregunta.previusChanges[count] = respuesta.correcta;
                count++;
            }
        }
    }
}

