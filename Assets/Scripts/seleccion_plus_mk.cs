using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class seleccion_plus_mk : MonoBehaviour
{
    public GameObject panel_mk;
    public GameObject panel_seleccion;
    public GameObject panel_mk_izquierda;
    public GameObject panel_mk_derecha;
    public GameObject texto_elegir_pj;
    public bool mover_pa_bajo = false;

    private Vector2 panel_mk_derecha_origin;

    public int personaje_a_enfrentar = 0;

    private void Awake() {
        panel_mk_derecha_origin = panel_mk_derecha.GetComponent<RectTransform>().anchoredPosition;
    }

    public void PasarHijitos(GameObject b)
    {
        panel_mk.SetActive(true);
        for (int i=panel_seleccion.transform.childCount-1; i >= 0; --i) {
            Transform child = panel_seleccion.transform.GetChild(i);
            if(child.name != b.name){
                child.SetParent(panel_mk_derecha.transform, false);
            }
            else{
                child.SetParent(panel_mk_izquierda.transform, false);
            }
            child.transform.GetChild(0).gameObject.SetActive(false);
            child.GetComponent<Button>().interactable = false;
        }

        List<int> indexes = new List<int>();
        List<Transform> items = new List<Transform>();
        for (int i = 0; i < panel_mk_derecha.transform.childCount;++i)
        {
            indexes.Add(i);
            items.Add(panel_mk_derecha.transform.GetChild(i));
        }
        foreach (var item in items)
        {
            item.SetSiblingIndex(indexes[Random.Range(personaje_a_enfrentar, indexes.Count)]);
        }

        PonerBoxCollider(personaje_a_enfrentar);

        panel_seleccion.SetActive(false);
        texto_elegir_pj.SetActive(false);

        mover_pa_bajo = true;
    }

    public void PonerBoxCollider(int pj){
        GameObject chold = panel_mk_derecha.transform.GetChild(pj).transform.gameObject;
        BoxCollider2D bc = chold.AddComponent<BoxCollider2D>();
        bc.size = new Vector2(200, 200);
    }

    public void QuitarBoxCollider(int pj){
        Destroy(panel_mk_derecha.transform.GetChild(pj).GetComponent<BoxCollider2D>());
    }

    public void Reconstruir() {
        panel_mk.SetActive(false);
        GameObject[] objs = new GameObject[10];
        for (int i = 0; i < panel_mk_derecha.transform.childCount; i++)
        {
            if (!panel_mk_derecha.transform.GetChild(i).name.Contains("Clone"))
                objs[int.Parse(panel_mk_derecha.transform.GetChild(i).gameObject.name.Replace("PJ", "")) - 1] = panel_mk_derecha.transform.GetChild(i).gameObject;
            else {
                Destroy(panel_mk_derecha.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < panel_mk_izquierda.transform.childCount; i++)
        {
            objs[int.Parse(panel_mk_izquierda.transform.GetChild(i).gameObject.name.Replace("PJ", "")) - 1] = panel_mk_izquierda.transform.GetChild(i).gameObject;
        }
        foreach (GameObject item in objs)
        {
            item.transform.SetParent(panel_seleccion.transform);
        }
        panel_seleccion.SetActive(true);
        texto_elegir_pj.SetActive(true);
        panel_mk_derecha.GetComponent<RectTransform>().anchoredPosition = panel_mk_derecha_origin;
        for (int i = 0; i < panel_seleccion.transform.childCount; i++)
        {
            var collider_malito = panel_seleccion.transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>();
            if (collider_malito) Destroy(collider_malito);
            panel_seleccion
                .transform.GetChild(i)
                .gameObject.GetComponent<Button>()
                .interactable = true;
        }
        personaje_a_enfrentar = 0;
    }

    private void FixedUpdate()
    {
        if(!mover_pa_bajo){
            return;
        }

        float step = 150f * Time.deltaTime;
        panel_mk_derecha.transform.position = Vector2.MoveTowards(panel_mk_derecha.transform.position,
            new Vector2(panel_mk_derecha.transform.position.x, panel_mk_izquierda.transform.GetChild(0).transform.position.y) - new Vector2(0, panel_mk_derecha.transform.GetChild(personaje_a_enfrentar).transform.localPosition.y),
            step);
    }

}
