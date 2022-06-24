using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable]
public class TutorialPaso {
    public string caminoBien;
    public string caminoMal;
    public string caminoBienVieneMal;
    public string caminoMalVieneBien;
}

public class tutorial : MonoBehaviour
{
    [SerializeField] private timer_ataque timer;
    [SerializeField] private preguntitas prg;

    [SerializeField] private int pasoActual = 0;

    public TextMeshProUGUI tutorialText;

    public bool VieneBien;

    [SerializeField] private List<TutorialPaso> pasos;

    public bool tutorialEnd = false;

    public bool firstTime = true;

    [SerializeField] private Sprite fondo_posta;
    [SerializeField] private GameObject fondo_a_cambiar;

    void Start()
    {
        if (PlayerPrefs.GetInt("tutorial", 0) == 1) {
            tutorialEnd = true;
            this.enabled = false;
            fondo_a_cambiar.GetComponent<Image>().sprite = fondo_posta;
            fondo_a_cambiar.GetComponent<Image>().color = new Color32 (255, 255, 255, 255);
            return;
        }

        preguntitas.OnPreguntaStart += OnPreguntaStart;
        preguntitas.OnPreguntaRespondidaCorrecta += OnPreguntaRespondidaCorrecta;
        preguntitas.OnPreguntaRespondidaIncorrecta += OnPreguntaRespondidaIncorrecta;
    }

    void OnPreguntaStart() {
        if (!this.enabled) {
            return;
        }
        if (tutorialEnd) {
            timer.gameObject.GetComponent<Slider>().value = 1f;
            timer.timer = 1f;
            timer.deslizamiento = true;
            this.enabled = false;
            tutorialText.text = "";
            fondo_a_cambiar.GetComponent<Image>().sprite = fondo_posta;
            fondo_a_cambiar.GetComponent<Image>().color = new Color32 (255, 255, 255, 255);
            PlayerPrefs.SetInt("tutorial", 1);
        }else {
            if (firstTime)
                tutorialText.text = "Adelante, responde lo que creas <color=green>correcto</color>";
            timer.gameObject.GetComponent<Slider>().value = 0.78f - (pasoActual * 0.1f);
            timer.timer = 0.78f - (pasoActual * 0.1f);
            timer.deslizamiento = false;
            firstTime = false;
        }
    }

    void OnPreguntaRespondidaCorrecta() {
        if (!this.enabled) return;
        if (tutorialText.gameObject.activeInHierarchy == false) tutorialText.gameObject.SetActive(true);
        tutorialText.text = VieneBien ? pasos[pasoActual].caminoBien : pasos[pasoActual].caminoBienVieneMal;
        prg.PasarDePreguntaSinVida();
        pasoActual++;
        VieneBien = true;
        tutorialEnd = (pasoActual == 4);
    }

    void OnPreguntaRespondidaIncorrecta() {
        if (!this.enabled) return;
        if (tutorialText.gameObject.activeInHierarchy == false) tutorialText.gameObject.SetActive(true);
        tutorialText.text = VieneBien ? pasos[pasoActual].caminoMalVieneBien : pasos[pasoActual].caminoMal;
        prg.PasarDePreguntaSinVida();
        pasoActual++;
        VieneBien = false;
        tutorialEnd = (pasoActual == 4);
    }

    void Update()
    {
        
    }
}
