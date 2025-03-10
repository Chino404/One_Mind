using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraSwitch))]
public class CameraSwitchEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Obtiene la referencia del script
        CameraSwitch script = (CameraSwitch)target;

        //Que haya un espacio
        GUILayout.Space(15);

        // Muestra el enum
        script.myTransitiontype = (CameraSwitch.TransitionType)EditorGUILayout.EnumPopup("Transition type", script.myTransitiontype);

        GUILayout.Space(5);

        // Dependiendo del valor del enum, muestra variables específicas
        switch (script.myTransitiontype)
        {
            case CameraSwitch.TransitionType.Goto:
                script.goTo = (Transform)EditorGUILayout.ObjectField("Go to Node", script.goTo, typeof(Transform), true);
                break;
            case CameraSwitch.TransitionType.BackTo:
                script.backToPosition = EditorGUILayout.Toggle("back to position", script.backToPosition);
                break;
            case CameraSwitch.TransitionType.Both:
                script.goTo = (Transform)EditorGUILayout.ObjectField("Go to Node", script.goTo, typeof(Transform), true);
                script.backToPosition = EditorGUILayout.Toggle("back to position", script.backToPosition);
                break;
            default:
                break;
        }

        // Guarda los cambios
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }


}
