using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SceneReference", menuName = "Game/Scene Reference")]
public class SceneReferenceSO :ScriptableObject
{

#if UNITY_EDITOR
    public SceneAsset sceneAsset;

    public SceneAsset[] moreScene;
#endif

    [SerializeField, HideInInspector]
    private List<string> _scenesNames;
    public List<string> ScenesNames => _scenesNames;

    [SerializeField, HideInInspector]
    private List<int> _buildIndexScenes;
    public List<int> BuildIndexScenes => _buildIndexScenes;


    [SerializeField, HideInInspector]
    private string _sceneName;
    public string SceneName => _sceneName;

    [SerializeField, HideInInspector]
    private int _buildIndex;
    public int BuildIndex => _buildIndex;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(sceneAsset);
            _sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            _buildIndex = GetBuildIndexByScenePath(path);
        }
        else
        {
            _sceneName = string.Empty;
            _buildIndex = -1;
        }

        if (moreScene != null && moreScene.Length > 0)
        {
            Debug.Log("Hay más escenas!!");

            _scenesNames.Clear();
            _buildIndexScenes.Clear();

            for (int i = 0; i < moreScene.Length; i++)
            {
                string path = AssetDatabase.GetAssetPath(moreScene[i]);
                _scenesNames.Add(System.IO.Path.GetFileNameWithoutExtension(path));
                _buildIndexScenes.Add(GetBuildIndexByScenePath(path));
            }
        }
        else
        {
            Debug.Log("NO hay más escenas!!");

            _scenesNames.Clear();
            _buildIndexScenes.Clear();
        }
    }

    /// <summary>
    /// Obtengo el indice en base a la ubicación.
    /// </summary>
    /// <param name="scenePath"></param>
    /// <returns></returns>
    private int GetBuildIndexByScenePath(string scenePath)
    {
        var scenes = EditorBuildSettings.scenes;

        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].path == scenePath)
                return i;
        }
        return -1;
    }
#endif

    public void LoadSceneAdditive()
    {
        if (_buildIndex >= 0)
            SceneManager.LoadSceneAsync(_buildIndex, LoadSceneMode.Additive);
        else
            Debug.LogError($"La escena <color=yellow>'{_sceneName}'</color> no está en los Build Settings.");
    }

#if UNITY_EDITOR
    public void LoadMoreSceneAdditive()
    {
        if (moreScene.Length <= 0 && moreScene != null)
        {
            Debug.LogError($"No hay escenas de más cargadas!");
            return;
        }

        for (int i = 0; i < moreScene.Length; i++)
        {
            SceneManager.LoadSceneAsync(_buildIndexScenes[i],LoadSceneMode.Additive);
        }
    }
#endif

    public void UnloadScene()
    {
        if(_buildIndex >= 0)
            SceneManager.UnloadSceneAsync(_buildIndex);
        else
            Debug.LogError($"La escena '{_sceneName}' no está en los Build Settings.");
    }

    public bool IsSceneLoaded()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == _buildIndex && scene.isLoaded)
                return true;
        }
        return false;
    }
}
