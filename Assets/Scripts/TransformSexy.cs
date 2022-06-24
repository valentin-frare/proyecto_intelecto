using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class TransformSexy : MonoBehaviour
{
    [SerializeField] private bool Pj1;
    [SerializeField] private RectTransform tr;
    [SerializeField] private Vector2 pos;

    private void OnValidate() {
        if (tr == null)
            tr = GetComponent<RectTransform>();
    }
    
    private void Update() {
        tr.anchoredPosition = pos * new Vector2(Pj1 ? 1 : -1,1);
    }
}
#endif