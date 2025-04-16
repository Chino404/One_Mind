using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadSceneCollider : MonoBehaviour
{
    [SerializeField] private int _indexScene;

    [SerializeField] private SceneReferenceSO[] _sceneReference;
    [SerializeField] private bool _isAwake;

    [Space(5), Header("Zonas de coleccionables")]
    [SerializeField] private CharacterTarget _player;
    [SerializeField, Tooltip("Activar o descativar escena")] private bool _active = true;

    [Space(5), SerializeField] private bool _isLoadZoneCollectable;

    private void Awake()
    {
        if (_isAwake) SceneLoaded();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            if(CollZoneManager.instance && _isLoadZoneCollectable)
            {
                Debug.Log("cargo zona coleccionable");
                CollZoneManager.instance.SwitchZone(_player, _active, _indexScene);
                return;
            }

            //SceneManager.LoadSceneAsync(_indexScene,LoadSceneMode.Additive);

            SceneLoaded();

        }
    }

    private void SceneLoaded()
    {
        if (_sceneReference.Length < 0) return;

        foreach (var scene in _sceneReference)
        {
            if (!_active && scene.IsSceneLoaded())
            {
                Debug.Log($"<color=red> <b>DESACTIVE LA ESCENA </b></color>");
                scene.UnloadScene();

                return;
            }

            else if (_active && !scene.IsSceneLoaded()) scene.LoadSceneAdditive();
        }
    }

}
