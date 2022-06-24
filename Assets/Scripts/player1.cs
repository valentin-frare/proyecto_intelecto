using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1 : MonoBehaviour
{
    public static event Action OnHitPlayer2;
    public Animator an;

    private void Start() {
        player2.OnHitPlayer1 += OnHit;
    }

    void OnHit() {
        if(!gameObject.activeSelf){
            return;
        }
        if (pelea.Instance.pjs[0].life <= 0) {
            Kill();
            StartCoroutine(pelea.Instance.BlinkIndex(0, 5));
        }else {
            an.Play("Base Layer.daño");
            StartCoroutine(pelea.Instance.BlinkIndex(0, 5));
        }
    }

    void HitPlayer2() {
        OnHitPlayer2.Invoke();
    }
    
    public void Kill() {
        an.Play("Base Layer.derrota");
    }

    public void Respawn() {
        an.Play("Base Layer.postura");
    }
}
