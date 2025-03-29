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
        script.isToRotateTheCamera = EditorGUILayout.Toggle("Rotate the Camera", script.isToRotateTheCamera);

        if(script.isToRotateTheCamera)
        {
            GUILayout.Space(3);

            // Convertimos el Quaternion a un Vector3 para mostrarlo en el Inspector
            Vector3 eulerRotation = script.rotationCamera;
            eulerRotation = EditorGUILayout.Vector3Field(new GUIContent("Euler", "Los angulos hacia donde va a girar la cámara."), eulerRotation);

            // Convertimos de nuevo a Quaternion si hubo cambios
            script.rotationCamera = eulerRotation;

            GUILayout.Space(3);

        }

        GUILayout.Space(5);
        script.isChangeTheCameraOffset = EditorGUILayout.Toggle("Change the camera offset", script.isChangeTheCameraOffset);

        if(script.isChangeTheCameraOffset)
        {
            GUILayout.Space(3);

            script.newOffset = EditorGUILayout.Vector3Field(new GUIContent("New Offset", "Nuevo valor del Offset."), script.newOffset);
        }

        // Guardamos los cambios en el objeto
        if (GUI.changed) EditorUtility.SetDirty(script);
    }
}
