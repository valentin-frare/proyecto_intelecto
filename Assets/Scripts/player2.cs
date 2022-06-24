using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2 : MonoBehaviour
{
    public static event Action OnHitPlayer1;
    public Animator an;

    private void Start() {
        player1.OnHitPlayer2 += OnHit;
    }

    void OnHit() {
        if(!gameObject.activeSelf){
            return;
        }
        if (pelea.Instance.pjs[1].life <= 0) {
            Kill();
            StartCoroutine(pelea.Instance.BlinkIndex(1, 5));
        }else {
            an.Play("Base Layer.daño");
            StartCoroutine(pelea.Instance.BlinkIndex(1, 5));
        }
    }

    void HitPlayer1() {
        OnHitPlayer1.Invoke();
    }

    public void Kill() {
        an.Play("Base Layer.derrota");
    }

    public void Respawn() {
        an.Play("Base Layer.postura");
    }
}
