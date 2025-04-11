using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoad : MonoBehaviour
{
    public static int sceneNumber = 0;
    //[SerializeField] private Slider _loader = default;
    [SerializeField] private Image _loaderImage = default;
    private AsyncOperation _asyncOperation = default;

    private void Start()
    {
        StartCoroutine(AsyncCharge());

    }

    public void StartChargeScene()
    {
        StartCoroutine(AsyncCharge());
    }

    //private void Update()
    //{
    //    if(_asyncOperation.allowSceneActivation == false)
    //        _loaderImage.GetComponent<RectTransform>().Rotate(0f, 0f, -200f * Time.deltaTime);

    //}

    IEnumerator AsyncCharge()
    {
        
        _asyncOperation = SceneManager.LoadSceneAsync(sceneNumber);
        _asyncOperation.allowSceneActivation = false;
        Application.backgroundLoadingPriority = ThreadPriority.High;

        while (_asyncOperation.progress < .9f)
        {

            //float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            //yield return null;

            //if (_loader)
            //    _loader.value = progress;

            //if (_loader.value >= 1)
            //    _asyncOperation.allowSceneActivation = true;

            //if (_loaderImage)
            //    _loaderImage.fillAmount = progress;

            //if (_loaderImage.fillAmount >= 1)

            yield return null;
        }
            yield return new WaitForSeconds(2f);
                _asyncOperation.allowSceneActivation = true;
    }
}
