using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadSceneCollider : MonoBehaviour
{
    [SerializeField] private int _indexScene;

    public SceneReferenceSO sceneReference;

    [Space(5), Header("Zonas de coleccionables")]
    [SerializeField] private CharacterTarget _player;
    [SerializeField, Tooltip("Activar o descativar escena")] private bool _active = true;

    [Space(5), SerializeField] private bool _isLoadZoneCollectable;

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

            if (!_active && sceneReference.IsSceneLoaded())
            {
                Debug.Log("DESACTIVE LA ESCENA");
                sceneReference.UnloadScene();

                return;
            }

            else if(_active && !sceneReference.IsSceneLoaded()) sceneReference.LoadSceneAdditive();
        }
    }

}
