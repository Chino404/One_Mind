using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct SettingScene
{
    public SceneReferenceSO sceneReference;
    [Tooltip("Activar o descativar escena")] public bool isActiveScene;
}

public class LoadSceneCollider : MonoBehaviour
{
    [SerializeField] private SettingScene[] _scenesReference;
    [SerializeField,Tooltip("Si se activan al inicio del juego")] private bool _isAwake;

    private void Awake()
    {
        if (_isAwake) SceneLoaded();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            SceneLoaded();
        }
    }

    private void SceneLoaded()
    {
        if (_scenesReference.Length < 0) return;

        foreach (var scene in _scenesReference)
        {
            if (!scene.isActiveScene && scene.sceneReference.IsSceneLoaded())
            {
                Debug.Log($"<color=red> <b>DESACTIVE LA ESCENA </b></color>");
                scene.sceneReference.UnloadScene();

                continue;
            }

            else if (scene.isActiveScene && !scene.sceneReference.IsSceneLoaded()) scene.sceneReference.LoadSceneAdditive();
        }
    }

}
