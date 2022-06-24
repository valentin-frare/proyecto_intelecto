using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class transition : MonoBehaviour
{
    [SerializeField]
    private RectTransform arriba;
    [SerializeField]
    private RectTransform abajo;

    [SerializeField]
    private RectTransform espada;
    [SerializeField]
    private RectTransform libro;

    [RangeAttribute(0,1)]
    public float timer;

    public bool invert;

    public slide_manager slideManager;

    public AnimationCurve curvita;

    public Animator chispas;

    public bool endTransition = true;

    // Update is called once per frame
    void Update()
    {
        if (timer <= 1 && !invert)
            timer += Time.deltaTime * 3;
        else if (timer >= 0 && invert) {
            timer -= Time.deltaTime * 3;
        }
 
        if (timer > 0.96f && timer < 1.03f && endTransition && !invert) {
            chispas.Play("Base Layer.chispas");
            endTransition = false;
        }

        abajo.anchoredPosition = new Vector2(0f, Mathf.Lerp(-(Screen.height/4 * 3), -(Screen.height/4), curvita.Evaluate(timer)));
        arriba.anchoredPosition = new Vector2(0f, Mathf.Lerp((Screen.height/4 * 3), Screen.height/4, curvita.Evaluate(timer)));
        libro.anchoredPosition = new Vector2(0f, Mathf.Lerp(-500, (Screen.height/2), timer));
        espada.anchoredPosition = new Vector2(0f, Mathf.Lerp(500, -(Screen.height/2), timer));
        espada.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-80, -180, timer));
        abajo.sizeDelta = new Vector2(Screen.width, -Screen.height/2);
        arriba.sizeDelta = new Vector2(Screen.width, -Screen.height/2);
    }

    public IEnumerator Transition(int screen) {
        invert = !invert;
        yield return new WaitForSeconds(1f);
        
        slideManager.setActualScreen(screen);
        yield return new WaitForSeconds(0.05f);
        
        yield return new WaitForSeconds(0.2f);
        invert = !invert;

        //chispas.SetActive(false);
        endTransition = true;
    }

    public void TransitionButton(int screen) {
        StartCoroutine(Transition(screen));
    }
}
