using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TorchSequence))]
public class TorchSequenceEditor : Editor
{
    SerializedProperty torchsProp;
    SerializedProperty isLoopProp;

    SerializedProperty isChangeColorProp;
    SerializedProperty changeColorFireProp;

    SerializedProperty sequenceListProp;
    SerializedProperty activeTImeProp;

    private void OnEnable()
    {
        torchsProp = serializedObject.FindProperty("_torchs");
        isLoopProp = serializedObject.FindProperty("_isLoop");

        isChangeColorProp = serializedObject.FindProperty("_isChangeColor");
        changeColorFireProp = serializedObject.FindProperty("_changeColorFire");

        sequenceListProp = serializedObject.FindProperty(nameof(TorchSequence.sequenceList));
        activeTImeProp = serializedObject.FindProperty(nameof(TorchSequence.activeTime));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(torchsProp);
        EditorGUILayout.PropertyField(isLoopProp);

        EditorGUILayout.PropertyField(isChangeColorProp);

        if (isChangeColorProp.boolValue)
        {
            EditorGUILayout.PropertyField(changeColorFireProp, new GUIContent("Color"));
        }

        EditorGUILayout.PropertyField(sequenceListProp);
        EditorGUILayout.PropertyField(activeTImeProp);

        serializedObject.ApplyModifiedProperties();
    }
}
