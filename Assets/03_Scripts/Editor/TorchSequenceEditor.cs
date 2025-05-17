using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TorchSequence))]
public class TorchSequenceEditor : Editor
{
    SerializedProperty isoneActiveProp;
    SerializedProperty torchsProp;
    SerializedProperty isSequenceWithTwoTorchesProp;

    SerializedProperty isChangeColorProp;
    SerializedProperty changeColorFireProp;
    SerializedProperty saveColorFireProp;

    SerializedProperty sequenceListProp;
    SerializedProperty activeTImeProp;

    private void OnEnable()
    {
        isoneActiveProp = serializedObject.FindProperty("_isOneActive");
        torchsProp = serializedObject.FindProperty("_torchs");
        isSequenceWithTwoTorchesProp = serializedObject.FindProperty("_isSequenceWithTwoTorches");

        isChangeColorProp = serializedObject.FindProperty("_isChangeColor");
        changeColorFireProp = serializedObject.FindProperty("_changeColorFire");
        saveColorFireProp = serializedObject.FindProperty("_saveColorFire");

        sequenceListProp = serializedObject.FindProperty(nameof(TorchSequence.sequenceList));
        activeTImeProp = serializedObject.FindProperty(nameof(TorchSequence.delayActive));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(isoneActiveProp);
        EditorGUILayout.PropertyField(torchsProp);

        EditorGUILayout.PropertyField(isSequenceWithTwoTorchesProp);

        if(isSequenceWithTwoTorchesProp.boolValue)
        {
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("-> Sequence Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(isChangeColorProp);

            if (isChangeColorProp.boolValue)
            {
                EditorGUILayout.PropertyField(changeColorFireProp);
                EditorGUILayout.PropertyField(saveColorFireProp);
            }

            EditorGUILayout.PropertyField(sequenceListProp);

        }

        EditorGUILayout.PropertyField(activeTImeProp);

        serializedObject.ApplyModifiedProperties();
    }
}
