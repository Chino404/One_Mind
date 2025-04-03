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
        GUILayout.Space(5);
        script.isChangeNewCamera = EditorGUILayout.Toggle("Change new Camera", script.isChangeNewCamera);
        if (script.isChangeNewCamera) script.newCamera = (CameraRails)EditorGUILayout.ObjectField("New Camera", script.newCamera, typeof(CameraRails), true);

        GUILayout.Space(15);

        // Muestra el enum
        script.myTransitiontype = (CameraSwitch.TransitionType)EditorGUILayout.EnumPopup("Transition type", script.myTransitiontype);

        GUILayout.Space(3);

        // Dependiendo del valor del enum, muestra variables específicas
        switch (script.myTransitiontype)
        {
            case CameraSwitch.TransitionType.GotoFixedNode:
                script.goToNode = (Transform)EditorGUILayout.ObjectField("Go to Node", script.goToNode, typeof(Transform), true);
                break;
            case CameraSwitch.TransitionType.BackTo:
                script.isBackToNewNode = EditorGUILayout.Toggle("Back to new node", script.isBackToNewNode);
                if (script.isBackToNewNode) script.newToNode = (Transform)EditorGUILayout.ObjectField("New node", script.newToNode, typeof(Transform), true);

                break;
            case CameraSwitch.TransitionType.Both:
                script.goToNode = (Transform)EditorGUILayout.ObjectField("Go to Node", script.goToNode, typeof(Transform), true);

                GUILayout.Space(5);

                script.isBackToNewNode = EditorGUILayout.Toggle("Back to new position", script.isBackToNewNode);
                if (script.isBackToNewNode) script.newToNode = (Transform)EditorGUILayout.ObjectField("New node", script.newToNode, typeof(Transform), true);

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
