using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoad : MonoBehaviour
{
    public static int sceneNumber = 0;
    [SerializeField] private Slider _loader = default;
    private AsyncOperation _asyncOperation = default;

    private void Start()
    {
        StartCoroutine(AsyncCharge());

    }

    public void StartChargeScene()
    {
        StartCoroutine(AsyncCharge());
    }

    IEnumerator AsyncCharge()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(sceneNumber);
        _asyncOperation.allowSceneActivation = false;
        Application.backgroundLoadingPriority = ThreadPriority.High;

        while (!_asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            yield return null;

            if (_loader)
                _loader.value = progress;

            if (_loader.value >= 1)
                _asyncOperation.allowSceneActivation = true;
        }
    }
}
