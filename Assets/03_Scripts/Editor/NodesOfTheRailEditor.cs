using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodesOfTheRail))]
public class NodesOfTheRailEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NodesOfTheRail script = (NodesOfTheRail)target;

        //Que haya un espacio
        GUILayout.Space(10);
        script.isToRotateTheCamera = EditorGUILayout.Toggle("Rotate the Camera.", script.isToRotateTheCamera);

        if(script.isToRotateTheCamera)
        {
            // Convertimos el Quaternion a un Vector3 para mostrarlo en el Inspector
            Vector3 eulerRotation = script.rotationCamera.eulerAngles;
            eulerRotation = EditorGUILayout.Vector3Field(new GUIContent("Euler", "Los angulos hacia donde va a girar la cámara."), eulerRotation);

            // Convertimos de nuevo a Quaternion si hubo cambios
            script.rotationCamera = Quaternion.Euler(eulerRotation);
        }

        // Guardamos los cambios en el objeto
        if (GUI.changed) EditorUtility.SetDirty(script);
    }
}
