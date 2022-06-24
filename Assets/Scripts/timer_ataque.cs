using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer_ataque : MonoBehaviour
{
    private Slider slider;
    public bool deslizamiento = false;
    public float timer = 1f;
    public float time;
    public GameObject limit1to2;
    public GameObject limit2to3;
    public preguntitas preguntitas;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if(deslizamiento){
            timer = Mathf.Clamp(timer - (Time.deltaTime / time), 0, 1);
            slider.value = timer;
        }

        if (slider.value < 0.66f) {
            limit2to3.SetActive(false);
        }
        else{
            limit2to3.SetActive(true);
        }

        if (slider.value < 0.33f) {
            limit1to2.SetActive(false);
        }
        else{
            limit1to2.SetActive(true);
        }

        if(deslizamiento){
            if (slider.value == 0f){
                deslizamiento = false;
                preguntitas.CambiarColorTimeOut();
            }
        }
    }

    public int GetAtackType() => (1 + (limit1to2.activeInHierarchy ? 1 : 0) + (limit2to3.activeInHierarchy ? 1 : 0));
}
