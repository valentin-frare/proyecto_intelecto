using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colision : MonoBehaviour
{
    public GameObject seleccion;
    public transition tr;
    public timer_ataque timer;
    public preguntitas preguntitas;

    void OnTriggerEnter2D() {
        seleccion.GetComponent<seleccion_plus_mk>().mover_pa_bajo = false;
        StartCoroutine(Esperar(1.0f));
    }

    private IEnumerator Esperar(float sec)
    {
        yield return new WaitForSeconds(sec);
        StartCoroutine(tr.Transition(2));
        preguntitas.OcultarMostrarCosas(false);
        preguntitas.first_time = true;
        preguntitas.indicator_timer = 0f;
        preguntitas.circulito.fillAmount = 0f;
        timer.timer = 1;
        yield return new WaitForSeconds(1.5f);
        preguntitas.circulito_visto = true;
        
        pelea.Instance.ElegirContricante(
            seleccion.GetComponent<seleccion_plus_mk>()
            .panel_mk_derecha.transform.GetChild(seleccion.GetComponent<seleccion_plus_mk>().personaje_a_enfrentar)
            .GetComponent<posible_enemigo>()
            .codigoPais
            );
        preguntitas.preguntaPropioPais = false;
    }
}
