using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneCollider : MonoBehaviour
{
    [SerializeField] private int _nextScene;

    [SerializeField] private List<int> loadingQueue;
    [SerializeField] private GameObject furnituresPrefab;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            SceneManager.LoadSceneAsync(_nextScene, LoadSceneMode.Additive);
            
            gameObject.SetActive(false);
        }
    }

    IEnumerator LoadSceneCoroutine()
    {
        var mainLevelAsync = SceneManager.LoadSceneAsync(loadingQueue[0], LoadSceneMode.Additive);

        yield return new WaitUntil(() => mainLevelAsync.isDone);

        mainLevelAsync = SceneManager.LoadSceneAsync(loadingQueue[1], LoadSceneMode.Additive);
        yield return new WaitUntil(() => mainLevelAsync.isDone);

        Instantiate(furnituresPrefab);
    }
}
