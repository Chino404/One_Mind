using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UICollectible))]
public class UICollectibleEdito : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Obtiene la referencia del script
        UICollectible script = (UICollectible)target;

        //Que haya un espacio
        GUILayout.Space(10);

        // Muestra el enum
        script.myShowType = (UICollectible.ShowType)EditorGUILayout.EnumPopup("Show type", script.myShowType);

        GUILayout.Space(3);

        switch (script.myShowType)
        {
            case UICollectible.ShowType.InGame:
                // Simular Header
                EditorGUILayout.LabelField("-> Positions", EditorStyles.boldLabel);
                script.showPos = EditorGUILayout.Vector2Field(new GUIContent("Show Pos", "Esta es la posición cuando se muestre"), script.showPos);
                script.hidePos = EditorGUILayout.Vector2Field(new GUIContent("Hide Pos", "Esta es la posición cuando se esconda"), script.hidePos);

                script.timeShow = EditorGUILayout.Slider(new GUIContent("Time Show", "Tiempo que va a estar mostrandose"), script.timeShow, 0f, 3f);
                script.speed = EditorGUILayout.Slider(new GUIContent("Speed", "Tiempo que va a estar mostrandose"), script.speed, 0f, 0.5f);

                GUILayout.Space(5);
                break;
        }

        EditorGUILayout.LabelField("-> Colors", EditorStyles.boldLabel);
        script.activeColor = EditorGUILayout.ColorField(new GUIContent("Active Color", "Cuando se obtenga."), script.activeColor);
        script.deactiveColor = EditorGUILayout.ColorField(new GUIContent("Deactive Color", "Cuando no se obtenga."), script.deactiveColor);

        // Guarda los cambios
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }
}
