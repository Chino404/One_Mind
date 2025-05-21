using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TemporalyChangeTheCamera))]
public class TemporalyChangeTheCameraEditor : Editor
{
    SerializedProperty targetPlayerProp;
    SerializedProperty typeToChangeWithProp;

    SerializedProperty fixedCameraProp;

    SerializedProperty objToInteractProp;
    SerializedProperty objActionProp;

    SerializedProperty timeActivaProp;

    private void OnEnable()
    {
        targetPlayerProp = serializedObject.FindProperty(nameof(TemporalyChangeTheCamera.targetPlayer));
        typeToChangeWithProp = serializedObject.FindProperty(nameof(TemporalyChangeTheCamera.typeToChangeWith));

        fixedCameraProp = serializedObject.FindProperty(nameof(TemporalyChangeTheCamera.fiexdCamera));

        objToInteractProp = serializedObject.FindProperty(nameof(TemporalyChangeTheCamera.layerToInteract));
        objActionProp = serializedObject.FindProperty(nameof(TemporalyChangeTheCamera.objAction));

        timeActivaProp = serializedObject.FindProperty(nameof(TemporalyChangeTheCamera.timeActive));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(targetPlayerProp);
        EditorGUILayout.PropertyField(fixedCameraProp);

        EditorGUILayout.PropertyField(typeToChangeWithProp);


        //Obtengo el valor del enum
        TypeToChange typeToChangeWith = (TypeToChange)typeToChangeWithProp.enumValueIndex;

        EditorGUILayout.Space(5);
        if (typeToChangeWith == TypeToChange.Layers)
        {
            EditorGUILayout.PropertyField(objToInteractProp);
        }
        else if(typeToChangeWith == TypeToChange.Action)
        {
            EditorGUILayout.PropertyField(objActionProp);
        }

        EditorGUILayout.PropertyField(timeActivaProp);

        serializedObject.ApplyModifiedProperties();
    }
}
