using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CanEditMultipleObjects]
[CustomEditor(typeof(AudioSetting))]
public class AudioSettingEditor : Editor
{
    SerializedProperty soundsProp;
    SerializedProperty mixerProp;

    [Tooltip("Si se debería resaltar.")]private bool _isShouldHighlight = false;
    [Tooltip("Si es todo válido.")]private bool _isAllValid = true;

    private void OnEnable()
    {
        soundsProp = serializedObject.FindProperty("sounds");
        mixerProp = serializedObject.FindProperty("mixer");

        // -> 'serializedObject' es una representación de Unity del objeto (en este caso AudioSetting) que permite editar sus propiedades de forma segura. Es una versión editable del objeto.
        // -> 'soundsProp' obtiene una referencia al array 'sounds' usando FindProperty.
    }

    /// <summary>
    /// Dibujar en el inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Audio Setting", EditorStyles.boldLabel); //Muestra el título “Audio Setting” en negrita.

        EditorGUILayout.PropertyField(mixerProp); // Mostrar el AudioMixer en el inspector
        EditorGUILayout.Space();

        for (int i = 0; i < soundsProp.arraySize; i++)
        {
            SerializedProperty soundProp = soundsProp.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = soundProp.FindPropertyRelative(nameof(Sound.name)); //Accedo al 'name' de ese sound.

            soundProp.isExpanded = EditorGUILayout.Foldout(soundProp.isExpanded, $"{nameProp.stringValue}", true);
                                             //-> 'Foldout': crea una sección desplegable con el nombre del sonido (name).

            if (soundProp.isExpanded)
            {
                EditorGUILayout.BeginVertical("box");
                       //-> 'BeginVertical("box")' dibuja un recuadro visual.


                //Llamo al método DrawSoundFields para mostrar los campos del Sound.
                DrawSoundFields(soundProp);

                GUILayout.Space(15);
                if (GUILayout.Button("Eliminar este sonido")) //Si se hace clic, elimina ese Sound del array.
                {
                    soundsProp.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

        CheckSounds();

        //Guarda y aplica los cambios al objeto AudioSetting.
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Validar si todos los sonidos están bien configurados
    /// </summary>
    private void CheckSounds()
    {
        _isAllValid = true;

        for (int i = 0; i < soundsProp.arraySize; i++)
        {
            SerializedProperty soundProp = soundsProp.GetArrayElementAtIndex(i);

            SerializedProperty outputProp = soundProp.FindPropertyRelative(nameof(Sound.output));
            SerializedProperty clipProp = soundProp.FindPropertyRelative(nameof(Sound.clip));
            SerializedProperty nameProp = soundProp.FindPropertyRelative(nameof(Sound.name));
            SerializedProperty idProp = soundProp.FindPropertyRelative(nameof(Sound.id));

            if(outputProp.objectReferenceValue == null)
            {
                _isAllValid = false;
                break;
            }

            if (clipProp.objectReferenceValue == null || string.IsNullOrEmpty(nameProp.stringValue))
            {
                _isAllValid = false;
                break;
            }

            SoundId idValue = (SoundId)idProp.enumValueIndex;
            if (idValue == SoundId.None || idValue == SoundId.____Generic_____ || idValue == SoundId.____Players_____)
            {
                _isAllValid = false;
                break;
            }
        }

        EditorGUI.BeginDisabledGroup(!_isAllValid);

        if (GUILayout.Button("Agregar sonido"))
        {
            soundsProp.InsertArrayElementAtIndex(soundsProp.arraySize);
        }

        EditorGUI.EndDisabledGroup();

        if (!_isAllValid)
        {
            EditorGUILayout.HelpBox("No podés agregar un nuevo sonido hasta que todos los campos requeridos estén bien configurados.", MessageType.Error);
        }
    }

    /// <summary>
    /// Dibuja manualmente cada campo del objeto Sound.
    /// </summary>
    /// <param name="sound"></param>
    private void DrawSoundFields(SerializedProperty sound)
    {
        //'FindPropertyRelative(...)' busca propiedades internas del objeto Sound.
        //'PropertyField(...)' muestra el campo en el inspector.
        //'SerializedProperty' Es una representación "serializada" y flexible de una propiedad de un objeto.


        GUILayout.Label("-> Seteo General", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.target)));

        SerializedProperty outputProp = sound.FindPropertyRelative(nameof(Sound.output));
        ChangeColorVariable(outputProp);

        GUILayout.Space(10);

        SerializedProperty nameProp = sound.FindPropertyRelative(nameof(Sound.name));
        ChangeColorVariable(nameProp);

        SerializedProperty clipProp = sound.FindPropertyRelative(nameof(Sound.clip));
        ChangeColorVariable(clipProp);

        SerializedProperty idProp = sound.FindPropertyRelative(nameof(Sound.id));
        ChangeColorVariable(idProp);



        GUILayout.Space(15);

        GUILayout.Label("-> Volumen y Pitch", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.maxVolume)));
        EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.volumeCurve)));
        EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.maxPitch)));
        EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.minPitch)));

        GUILayout.Space(15);

        GUILayout.Label("-> Distancia", EditorStyles.boldLabel);

        SerializedProperty isNotWithDistanceProp = sound.FindPropertyRelative(nameof(Sound.isNotWithDistance)); //Me guardo su valor en una variable.
        EditorGUILayout.PropertyField(isNotWithDistanceProp);

        if (!isNotWithDistanceProp.boolValue)
        {
            EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.maxDistance)));
            EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.minDistance)));
        }
        else
        {
            EditorGUILayout.HelpBox("Este sonido no usa distancia.\nLos campos 'MinDistance' y 'MaxDistance' están deshabilitados para evitar confusión.", MessageType.Warning);
        }

        //GUILayout.Space(5);
        //EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.posSound)));

        GUILayout.Space(15);

        GUILayout.Label("-> Opciones", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.isLoop)));
        EditorGUILayout.PropertyField(sound.FindPropertyRelative(nameof(Sound.isPlayOnAwake)));
    }

    /// <summary>
    /// Cambiar el color de la variable si es que esta mal seteada.
    /// </summary>
    /// <param name="prop"></param>
    private void ChangeColorVariable(SerializedProperty prop)
    {
        // Guardar el color original
        Color originalColor = GUI.color;

        _isShouldHighlight = false;
        GUIContent warningIcon = null;

        switch (prop.propertyType)
        {
            case SerializedPropertyType.String:

                _isShouldHighlight = string.IsNullOrEmpty(prop.stringValue); //Si es null o está vació.
                if (_isShouldHighlight) warningIcon = EditorGUIUtility.IconContent("console.warnicon", "Este campo es obligatorio.");

                break;

            case SerializedPropertyType.Enum:

                if (prop.name == nameof(Sound.id)) // <- este es el nombre de la propiedad en tu clase Sound
                {
                    SoundId current = (SoundId)prop.enumValueIndex;

                    if (current == SoundId.None || current == SoundId.____Generic_____ || current == SoundId.____Players_____)
                    {
                        _isShouldHighlight = true;
                        warningIcon = EditorGUIUtility.IconContent("console.warnicon", "Seleccioná un ID válido.");
                    }
                }

                break;

            case SerializedPropertyType.ObjectReference:

                _isShouldHighlight = prop.objectReferenceValue == null;
                if (_isShouldHighlight) warningIcon = EditorGUIUtility.IconContent("console.warnicon", "Este campo requiere una referencia.");

                break;
        }

        EditorGUILayout.BeginHorizontal(); //Para poder mostrar el nombre de la variable y la variable de forma horizontal (en una sola línea).

        if (_isShouldHighlight) GUI.color = new Color(1f, 0.5f, 0.5f); // rojo suave

        EditorGUILayout.PropertyField(prop);

        GUI.color = originalColor; // Restaurar color

        if (_isShouldHighlight && warningIcon != null)
        {
            GUILayout.Space(5);
            GUILayout.Label(warningIcon, GUILayout.Width(20));
        }

        EditorGUILayout.EndHorizontal();
    }
}
