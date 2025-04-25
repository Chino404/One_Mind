using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoad : MonoBehaviour
{
    private int _myIndexScene;
    [Tooltip("Escena a cargar")]public static int sceneNumber = 0;
    [Tooltip("Escena a cargar")]public static SceneReferenceSO sceneReference;
    [SerializeField] private Image _loaderImage = default;

    private AsyncOperation _asyncOperation = default;

    private void Start()
    {
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas
        _myIndexScene = currentScene.buildIndex;

        StartCoroutine(AsyncCharge());

    }

    public void StartChargeScene()
    {
        StartCoroutine(AsyncCharge());
    }

    IEnumerator AsyncCharge()
    {

        // 1. Escena de carga (pantalla de loading actual)
        Scene loadingScene = SceneManager.GetActiveScene();

        // 2. Cargar escena principal (aditiva) SIN activarla aún
        _asyncOperation = sceneReference != null ? SceneManager.LoadSceneAsync(sceneReference.BuildIndex, LoadSceneMode.Additive) : SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);

        _asyncOperation.allowSceneActivation = false;

        Application.backgroundLoadingPriority = ThreadPriority.High;

        // 3. Esperar a que la escena principal llegue al 90%
        while (_asyncOperation.progress < 0.9f)
        {
            yield return null;
        }


        // 4. Cargar escenas adicionales desde el SO
        List<AsyncOperation> additionalLoads = new();

        if (sceneReference != null && sceneReference.BuildIndexScenes.Count > 0)
        {
            foreach (int buildIndex in sceneReference.BuildIndexScenes)
            {
                if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
                {
                    Debug.LogWarning($"Escena adicional con índice inválido: {buildIndex}");
                    continue;
                }

                var op = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

                additionalLoads.Add(op);
            }
        }


        // 5. Esperar a que TODAS las escenas adicionales estén cargadas
        foreach (var op in additionalLoads)
        {
            while (op.progress < 0.9f)
            {
                yield return null;
            }
        }

        // 6. (Opcional) delay por estética
        yield return new WaitForSecondsRealtime(1f);

        // 7. Activar la escena principal
        _asyncOperation.allowSceneActivation = true;

        // 8. Esperar que se active
        yield return new WaitUntil(() => _asyncOperation.isDone);

        // 9. Establecer escena activa
        Scene mainScene = sceneReference != null ? SceneManager.GetSceneByBuildIndex(sceneReference.BuildIndex) : SceneManager.GetSceneByBuildIndex(sceneNumber);
        if (mainScene.IsValid())
            SceneManager.SetActiveScene(mainScene);

        // 10. Descargar la escena de carga
        if (loadingScene.IsValid() && loadingScene.name != mainScene.name)
            SceneManager.UnloadSceneAsync(loadingScene);

        sceneReference = default;

        Debug.Log("<color=green>Escena cargada correctamente.</color>");

    }
}
