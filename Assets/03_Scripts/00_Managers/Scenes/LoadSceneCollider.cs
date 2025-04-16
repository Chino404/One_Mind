using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadSceneCollider : MonoBehaviour
{
    [SerializeField] private int _indexScene;

    // Esto es solo para mostrar la escena en el inspector.
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif

    // Esta es la ruta que realmente usarás para cargar la escena.
    [HideInInspector] public int buildIndex = -1;

    [Space(5), Header("Zonas de coleccionables")]
    [SerializeField] private CharacterTarget _player;
    [SerializeField, Tooltip("Activar o descativar escena")] private bool _active = true;

    [Space(5), SerializeField] private bool _isLoadZoneCollectable;

    //[SerializeField] private List<int> loadingQueue;
    //[SerializeField] private GameObject furnituresPrefab;


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

#if UNITY_EDITOR
            if (sceneAsset != null)
            {
                string path = AssetDatabase.GetAssetPath(sceneAsset);
                Debug.Log(path);
                buildIndex = GetBuildIndexByScenePath(path);
            }

            else
            {
                buildIndex = -1;
            }
#endif

            SceneManager.LoadSceneAsync(buildIndex,LoadSceneMode.Additive);
        }
    }

#if UNITY_EDITOR
    int GetBuildIndexByScenePath(string scenePath)
    {
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].path == scenePath)
            {
                return i;
            }
        }
        return -1; // No encontrada
    }
#endif

    //IEnumerator LoadSceneCoroutine()
    //{
    //    var mainLevelAsync = SceneManager.LoadSceneAsync(loadingQueue[0], LoadSceneMode.Additive);

    //    yield return new WaitUntil(() => mainLevelAsync.isDone);

    //    mainLevelAsync = SceneManager.LoadSceneAsync(loadingQueue[1], LoadSceneMode.Additive);
    //    yield return new WaitUntil(() => mainLevelAsync.isDone);

    //    Instantiate(furnituresPrefab);
    //}
}
