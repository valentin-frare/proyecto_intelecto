using UnityEditor;
using UnityEngine;

public class tools {
    #if UNITY_EDITOR
    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);
        T[] a = new T[guids.Length];
        for(int i =0;i<guids.Length;i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return a;
    }
    #endif

    public static string PaisCodeToName(string paisCode, bool capitalize = true) {
        if (paisCode == "ARG") return capitalize ? "Argentina" : "argentina";
        if (paisCode == "IND") return capitalize ? "India" : "india";
        if (paisCode == "ITL") return capitalize ? "Italia" : "italia";
        if (paisCode == "HOL") return capitalize ? "Holanda" : "holanda";
        if (paisCode == "SUD") return capitalize ? "Sudáfrica" : "sudáfrica";
        if (paisCode == "EGP") return capitalize ? "Egipto" : "egipto";
        if (paisCode == "USA") return capitalize ? "Estados Unidos" : "estados unidos";
        if (paisCode == "AUS") return capitalize ? "Australia" : "australia";
        if (paisCode == "JPN") return capitalize ? "Japón" : "japón";
        if (paisCode == "FRA") return capitalize ? "Francia" : "francia";
        return "Sos de el espacio exterior";
    }
}