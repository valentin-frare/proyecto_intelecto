using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slide_manager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] slides;

    [SerializeField]
    private float speed = 40f;

    public int actualScreen = 0;

    private void Awake(){
        actualScreen = 0;
    }

    private void FixedUpdate() {
        int index = 0;

        foreach (GameObject slide in slides) {
            Vector2 target = new Vector2((Screen.width * (index - actualScreen)) + Screen.width/2, Screen.height / 2); //   Se calcula la posición en pantalla segun indice y pantalla actual.
            float step =  (speed * Time.deltaTime) * Vector2.Distance(slide.transform.position, target); //     Se calcula la velocidad de movimiento usando la distancia, velocidad fija y delta time.
            slide.transform.position = Vector2.MoveTowards(slide.transform.position, target, step); //     Se mueve el objeto hasta la posicion elegida.
            if (slide == slides[actualScreen]) {
                slides[index].SetActive(true);
            }
            index++;
        }
    }

    public void setActualScreen(int i) {

        if(actualScreen == i){
            return;
        }

        actualScreen = i;
        StartCoroutine(DesactivarSlides());
    }

    IEnumerator DesactivarSlides() {
        yield return new WaitForSeconds(2);
        int index = 0;
        foreach (GameObject slide in slides) {
            if (slide != slides[actualScreen]) {
                slides[index].SetActive(false);
            }else {
                slides[index].SetActive(true);
            }
            index++;
        }
    }
}
