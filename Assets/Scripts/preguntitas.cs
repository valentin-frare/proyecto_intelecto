using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

enum PeleaPersonaje
{
    Uno,
    Dos
}

public class preguntitas : MonoBehaviour
{
    public bool preguntaPropioPais = true;
    public SetDePreguntas set;
    public SetDePreguntas setEnemigo;
    public Image circulito;
    public bool circulito_visto = false;
    public bool first_time = true;
    public float indicator_timer = 0f;
    private float max_timer = 0f;
    public GameObject panel_pregunta;
    public GameObject panel_entre_comillas;
    public timer_ataque timer;
    public TextMeshProUGUI pregunta;
    public TextMeshProUGUI[] respuestas;
    public Button[] botones;
    public tutorial tutorialObj;

    private Color letra_negra = new Color32 (0, 0, 0, 255);
    [SerializeField]
    public List<List<Pregunta>> set_no_original;

    private Pregunta comparar = new Pregunta();
    private bool comp = false;

    [SerializeField]
    private Color32 correctaColor = new Color32 (63, 236, 76, 255);
    [SerializeField]
    private Color32 incorrectaColor = new Color32 (200, 35, 35, 255);

    public static event Action OnPreguntaStart;
    public static event Action OnPreguntaRespondidaCorrecta;
    public static event Action OnPreguntaRespondidaIncorrecta;

    void Awake()
    {
        set_no_original = new List<List<Pregunta>>();
        set_no_original.Add(new List<Pregunta>());
        set_no_original.Add(new List<Pregunta>());
    }

    private void Shuffle()
    {
        TextMeshProUGUI temp;
        for (int i = 0; i < respuestas.Length - 1; i++) {
            int rnd = Random.Range(i, respuestas.Length);
            temp = respuestas[rnd];
            respuestas[rnd] = respuestas[i];
            respuestas[i] = temp;
        }
    }

    public void CambiarColor(Button b)
    {
        timer.deslizamiento = false;
        bool tf = b.GetComponent<respuesta_correcta_question>().correcto;

        var vida = 0;
        if(!tf){
            OnPreguntaRespondidaIncorrecta?.Invoke();
            b.GetComponent<Image>().color = incorrectaColor;
            vida = pelea.Instance.Atacar((Ataque)timer.GetAtackType(), (int)PeleaPersonaje.Uno, preguntaPropioPais);
            preguntaPropioPais = true;
        }
        else{
            OnPreguntaRespondidaCorrecta?.Invoke();
            b.GetComponent<Image>().color = correctaColor;
            vida = pelea.Instance.Atacar((Ataque)timer.GetAtackType(), (int)PeleaPersonaje.Dos, preguntaPropioPais);
            preguntaPropioPais = false;
        }

        ColorBlock cb = b.colors;
        cb.disabledColor = new Color32 (255, 255, 255, 255);
        b.colors = cb;

        for (int i = 0; i < botones.Length; i++) {
            botones[i].interactable = false;

            if (respuestas[i].transform.parent.name != b.name){
                respuestas[i].color = new Color32 (0, 0, 0, 150);
            }
            else{
                respuestas[i].color = letra_negra;
            }
        }

        if (tutorialObj.enabled) return;
        PasarDePregunta(vida);
    }

    public void PasarDePregunta(int vida) {
        StartCoroutine(Esperar(3.0f, vida));
        if(vida <= 0) return;
        StartCoroutine(EsperarPregunta(5.2f));
    }

    public void PasarDePreguntaSinVida() {
        var vida = 24;
        StartCoroutine(Esperar(3.0f, vida));
        if(vida <= 0) return;
        StartCoroutine(EsperarPregunta(5.2f));
    }

    public void CambiarColorTimeOut(){
        timer.deslizamiento = false;
        var vida = pelea.Instance.Atacar((Ataque)timer.GetAtackType(), (int)PeleaPersonaje.Uno, preguntaPropioPais);
        for (int i = 0; i < botones.Length; i++) {
            botones[i].interactable = false;
            respuestas[i].color = new Color32 (0, 0, 0, 150);
        }
        preguntaPropioPais = true;
        OnPreguntaRespondidaIncorrecta?.Invoke();

        StartCoroutine(Esperar(3.0f, vida));
        if(vida <= 0) return;
        StartCoroutine(EsperarPregunta(5.2f));
    }

    private IEnumerator Esperar(float sec, int vida)
    {
        yield return new WaitForSeconds(sec);
        if(vida <= 0){
            pelea.Instance.AparecerVictoriaDerrota(botones, panel_pregunta, timer.transform.gameObject, circulito.transform.gameObject);
            yield break;
        }
        OcultarMostrarCosas(false);
        indicator_timer = 0f;
        circulito.fillAmount = max_timer;
        circulito_visto = true;
    }

    private IEnumerator EsperarPregunta(float sec)
    {
        yield return new WaitForSeconds(sec);
        OcultarMostrarCosas(true);
        ComenzarPregunta();
        if(first_time){
            first_time = false;
        }
        else{
            timer.timer = 1;
        }
    }

    public void ComenzarPregunta()
    {    
        OnPreguntaStart?.Invoke();

        if(set_no_original[preguntaPropioPais ? 0 : 1].Count == 0){
            LlenarArray(!preguntaPropioPais);
        }

        int random = Random.Range(0, (set_no_original[preguntaPropioPais ? 0 : 1].Count));

        if(set_no_original[preguntaPropioPais ? 0 : 1].Count == (preguntaPropioPais ? set.Preguntas.Length : setEnemigo.Preguntas.Length) && comp){
            
            if(set_no_original[preguntaPropioPais ? 0 : 1][random].id == comparar.id){
                
                if((random + 1) > (set_no_original[preguntaPropioPais ? 0 : 1].Count - 1)){
                    random -= 1;
                }
                else if((random - 1) < 0){
                    random += 1;
                }
                else{
                    if((preguntaPropioPais ? set.Preguntas.Length : setEnemigo.Preguntas.Length) == 1){
                        random = 0;
                    }
                    else{
                        random += 1;
                    }
                }
            }
        }

        pregunta.text = set_no_original[preguntaPropioPais ? 0 : 1][random].pregunta;

        Shuffle();

        Color color_button = new Color32 (255, 255, 255, 255);
        Color disable_color_button = new Color32 (255, 255, 255, 175);
        for (int i = 0; i < respuestas.Length; i++) {
            botones[i].interactable = true;
            botones[i].GetComponent<Image>().color = color_button;
            ColorBlock cb = botones[i].colors;
            cb.disabledColor = disable_color_button;
            botones[i].colors = cb;
            respuestas[i].color = letra_negra;
            respuestas[i].text =  set_no_original[preguntaPropioPais ? 0 : 1][random].respuestas[i].respuesta;
            respuestas[i].GetComponentInParent<respuesta_correcta_question>().CorrectoIncorrecto(set_no_original[preguntaPropioPais ? 0 : 1][random].respuestas[i].correcta);
        }

        if(set_no_original[preguntaPropioPais ? 0 : 1].Count == 1){
            comparar = set_no_original[preguntaPropioPais ? 0 : 1][0];
            comp = true;
        }

        set_no_original[preguntaPropioPais ? 0 : 1].RemoveAt(random);
    }

    public void LlenarArray(bool enemigo = false){
        for (int i = 0; i < (enemigo ? setEnemigo.Preguntas.Length : set.Preguntas.Length); i++)
        {
            set_no_original[enemigo ? 1 : 0].Add(enemigo ? setEnemigo.Preguntas[i] : set.Preguntas[i]);
        }
    }

    public void OcultarMostrarCosas(bool mostrar_casi_todo)
    {
        if(mostrar_casi_todo){
            BoolMostrar(mostrar_casi_todo);
            circulito_visto = false;
            timer.deslizamiento = true;
        }
        else{
            BoolMostrar(mostrar_casi_todo);
        }
    }

    private void BoolMostrar(bool mostrar){
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].transform.gameObject.SetActive(mostrar);
        }
        panel_pregunta.SetActive(mostrar);
        timer.transform.gameObject.SetActive(mostrar);
        circulito.transform.gameObject.SetActive(!mostrar);
        panel_entre_comillas.SetActive(false);
    }

    private void FixedUpdate(){

        if(!circulito_visto){
            return;
        }
        indicator_timer = Mathf.Clamp(indicator_timer + Time.deltaTime, 0, 2);
        circulito.fillAmount = indicator_timer/2;
        if (indicator_timer == 2){
            circulito_visto = false;
            if(first_time){
                indicator_timer = 0f;
                circulito.fillAmount = max_timer;
                StartCoroutine(EsperarPregunta(0.2f));
            }
        }

    }

}
