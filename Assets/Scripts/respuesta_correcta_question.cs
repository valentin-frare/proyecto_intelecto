using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respuesta_correcta_question : MonoBehaviour
{
    public bool correcto;

    public void CorrectoIncorrecto(bool c) => correcto = c;
}
