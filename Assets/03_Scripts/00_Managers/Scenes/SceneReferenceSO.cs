using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SceneReference", menuName = "Game/Scene Reference")]
public class SceneReferenceSO :ScriptableObject
{

#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif

    [SerializeField, HideInInspector]
    private string sceneName;

    [SerializeField, HideInInspector]
    private int buildIndex;

    public string SceneName => sceneName;
    public int BuildIndex => buildIndex;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(sceneAsset);
            sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            buildIndex = GetBuildIndexByScenePath(path);
        }
        else
        {
            sceneName = string.Empty;
            buildIndex = -1;
        }
    }

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
        if (buildIndex >= 0)
            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        else
            Debug.LogError($"La escena '{sceneName}' no está en los Build Settings.");
    }

    public void UnloadScene()
    {
        if(buildIndex >= 0)
            SceneManager.UnloadSceneAsync(buildIndex);
        else
            Debug.LogError($"La escena '{sceneName}' no está en los Build Settings.");
    }

    public bool IsSceneLoaded()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == buildIndex && scene.isLoaded)
                return true;
        }
        return false;
    }
}
