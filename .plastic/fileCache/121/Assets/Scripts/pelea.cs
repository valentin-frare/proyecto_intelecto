using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.Animations;

public enum Ataque
{
    Nulo,
    Normal,
    Fuerte,
    Potente
}

[Serializable]
public class PersonajePelea {
    [RangeAttribute(0, 24)]
    public int life = 24;
    public GameObject obj;

    public Animator an;
    public Slider moral;
}

public class pelea : MonoBehaviour {
    public static pelea Instance {get; private set;}

    [SerializeField]
    public PersonajePelea[] pjs;

    [SerializeField]
    private seleccion_plus_mk spmk;

    [SerializeField]
    private transition tr;

    [SerializeField]
    private GameObject panel_entre_comillas;

    [SerializeField]
    private timer_ataque timer;
    [SerializeField]
    private preguntitas preguntitas;
    [SerializeField]
    private GameObject panel_pelea;
    [SerializeField]
    private GameObject[] vida_resta;

    public List<SetDePreguntas> sets;
    [SerializeField] private player1 p1;
    [SerializeField] private player2 p2;
    
    public String player1_animator = "Irwin";
    public String player2_animator = "Irwin";

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(Instance);
        }else {
            Instance = this;
        }
    }

    private void LateUpdate() {
        pjs[0].moral.value = (float)pjs[0].life/24;
        pjs[1].moral.value = (float)pjs[1].life/24;
    }

    public int Atacar(Ataque ataque, int pj, bool preguntaDePaisPropio = false) {
        int ataque_total = (int)ataque * (preguntaDePaisPropio ? (pj == 0 ? 2 : 1) : (pj == 0 ? 1 : 2));
        pjs[pj].life -= ataque_total;

        pjs[pj == 1 ? 0 : 1].an.Play(
            ataque == Ataque.Normal ?  $"{(pj == 0 ? player1_animator : player2_animator)}.ataque_debil" + (pj == 1 ? "" : "_invertido") : 
            ataque == Ataque.Fuerte ?  $"{(pj == 0 ? player1_animator : player2_animator)}.ataque_normal" + (pj == 1 ? "" : "_invertido") : 
                                       $"{(pj == 0 ? player1_animator : player2_animator)}.ataque_fuerte" + (pj == 1 ? "" : "_invertido")) ;

        Debug.Log(ataque);

        int index_pj0 = 0;
        int index_pj1 = 0;
        int cant = panel_pelea.transform.childCount;

        for (int i = 0; i < cant; i++)
        {
            if(panel_pelea.transform.GetChild(i).name == "Pj1"){
                index_pj0 = panel_pelea.transform.GetChild(i).GetSiblingIndex();
            }
            if(panel_pelea.transform.GetChild(i).name == "Pj2"){
                index_pj1 = panel_pelea.transform.GetChild(i).GetSiblingIndex();
            }
        }

        if(index_pj0 > index_pj1){
            if(pj == 0){
                panel_pelea.transform.GetChild(index_pj1).SetSiblingIndex(cant);
            }
        }
        else{
            if(pj == 1){
                panel_pelea.transform.GetChild(index_pj0).SetSiblingIndex(cant);
            }
        }

        if(pj == 0){
            RestarVidaTXT(0, ataque_total);
        }
        else{
            RestarVidaTXT(1, ataque_total);
        }

        //StartCoroutine(Blink(pjs[pj], ataque_total));
        return pjs[pj].life;
    }

    private void RestarVidaTXT(int index, int ataque){
        vida_resta[index].SetActive(true);
        vida_resta[index].GetComponent<TextMeshProUGUI>().text = "- " + ataque;
        StartCoroutine(DesaparecerTXT(2.5f, index));
    }

    private IEnumerator DesaparecerTXT(float n, int i){
        yield return new WaitForSeconds(n);
        vida_resta[i].SetActive(false);
    }

    IEnumerator Blink(PersonajePelea pj, int potencia) {
        for (int i = 0; i < potencia; i++)
        {
            pj.obj.GetComponent<RawImage>().color = new Color(1,0,0,1);
            yield return new WaitForSeconds(0.08f);
            pj.obj.GetComponent<RawImage>().color = new Color(1,1,1,1);
            yield return new WaitForSeconds(0.08f);
        }
    }

    public IEnumerator BlinkIndex(int index, int potencia) {
        var pj = pjs[index];
        for (int i = 0; i < potencia; i++)
        {
            pj.obj.GetComponent<RawImage>().color = new Color(1,0,0,1);
            yield return new WaitForSeconds(0.08f);
            pj.obj.GetComponent<RawImage>().color = new Color(1,1,1,1);
            yield return new WaitForSeconds(0.08f);
        }
    }

    public void AparecerVictoriaDerrota(Button[] b, GameObject p, GameObject t, GameObject c){
        panel_entre_comillas.SetActive(true);
        panel_entre_comillas.transform.GetChild(1).transform.gameObject.SetActive(true);
        if(spmk.personaje_a_enfrentar < 8){
            panel_entre_comillas.transform.GetChild(2).transform.gameObject.SetActive(true);
            panel_entre_comillas.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "Abandonar";
            if (pjs[0].life <= 0) {
                panel_entre_comillas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PERDISTE";
                panel_entre_comillas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Reintentar";
            }
            if (pjs[1].life <= 0) {
                panel_entre_comillas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "GANASTE";
                panel_entre_comillas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Continuar";
            }
        }
        else{
            panel_entre_comillas.transform.GetChild(2).transform.gameObject.SetActive(false);
            if (pjs[0].life <= 0) {
                panel_entre_comillas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PERDISTE";
                panel_entre_comillas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Reintentar";
            }
            if (pjs[1].life <= 0) {
                panel_entre_comillas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "GANASTE EL JUEGO";
                panel_entre_comillas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Volver al Menú";
            }
        }
        for (int i = 0; i < b.Length; i++)
        {
            b[i].transform.gameObject.SetActive(false);
        }
        p.SetActive(false);
        t.SetActive(false);
        c.SetActive(false);
    }

    public void HacerCosas(Button b){
        switch (b.name)
        {
            case "ButtonContinuarReintentar":
                if(pjs[0].life <= 0){
                    preguntitas.OcultarMostrarCosas(false);
                    preguntitas.first_time = true;
                    preguntitas.indicator_timer = 0f;
                    preguntitas.circulito.fillAmount = 0f;
                    timer.timer = 1;
                    preguntitas.circulito_visto = true;
                    pjs[0].life = 24;
                    pjs[1].life = 24;
                    p1.Respawn();
                }
                if(pjs[1].life <= 0){
                    if(spmk.personaje_a_enfrentar < 8){
                        spmk.personaje_a_enfrentar++;
                        spmk.PonerBoxCollider(spmk.personaje_a_enfrentar);
                        spmk.QuitarBoxCollider(spmk.personaje_a_enfrentar-1);
                        tr.TransitionButton(1);
                        StartCoroutine(Esperar(2.0f));
                    }
                    else{
                        spmk.Reconstruir();
                        pjs[0].life = 24;
                        pjs[1].life = 24;
                        tr.TransitionButton(0);
                    }
                    //p2.Respawn();
                }
                break;
            case "ButtonVolverMenu":
                spmk.Reconstruir();
                pjs[0].life = 24;
                pjs[1].life = 24;
                tr.TransitionButton(0);
                break;
            default:
                break;
        }
    }

    public void ElegirPersonaje(string codigoPais) {
        SetDePreguntas[] preguntitasSet = sets.ToArray();
        foreach (var set in preguntitasSet)
        {
            if (set.name.Split('_')[1] == codigoPais) {
                preguntitas.set = set;
                return;
            }
        }
        preguntitas.set = preguntitasSet[Random.Range(0, preguntitasSet.Length)];
        //SacarAnimators(codigoPais, 1);
    }

    public void ElegirContricante(string codigoPais) {
        SetDePreguntas[] preguntitasSet = sets.ToArray();
        foreach (var set in preguntitasSet)
        {
            if (set.name.Split('_')[1] == codigoPais) {
                preguntitas.setEnemigo = set;
                preguntitas.set_no_original[1] = new List<Pregunta>();
                return;
            }
        }
        preguntitas.setEnemigo = preguntitasSet[Random.Range(0, preguntitasSet.Length)];
        preguntitas.set_no_original[1] = new List<Pregunta>();
        //SacarAnimators(codigoPais, 2);
    }

    private IEnumerator Esperar(float sec)
    {
        Debug.Log("Me la mueve esperando");
        yield return new WaitForSeconds(sec);
        spmk.mover_pa_bajo = true;
        pjs[0].life = 24;
        pjs[1].life = 24;
    }

    public void PaBajo(GameObject g){
        if(g.transform.childCount > 0){
            spmk.mover_pa_bajo = true;
        }
    }

    public void SacarAnimators(string pais, int jugador){

        String pj;

        switch (pais)
        {
            case ("ARG"):
                pj = "Maradona";
                break;
            case ("AUS"):
                pj = "Irwin";
                break;
            case ("EGP"):
                pj = "Cleopatra";
                break;
            case ("FRA"):
                pj = "De_arco";
                break;
            case ("HOL"):
                pj = "Van_gogh";
                break;
            case ("IND"):
                pj = "Gandhi";
                break;
            case ("ITL"):
                pj = "Da_vinci";
                break;
            case ("JPN"):
                pj = "Kurosawa";
                break;
            case ("SUD"):
                pj = "Mandela";
                break;
            case ("USA"):
                pj = "Jackson";
                break;
            default:
                pj = "Irwin";
                break;
        }

        if(jugador == 1){
            player1_animator = pj;
            pjs[0].an.Play(player1_animator+".postura");
        }
        else{
            player2_animator = pj;
            pjs[1].an.Play(player2_animator+".postura");
        }

    }
}
