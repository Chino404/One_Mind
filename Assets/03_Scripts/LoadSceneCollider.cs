using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneCollider : MonoBehaviour
{
    [SerializeField] private int _nextScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            SceneManager.LoadSceneAsync(_nextScene, LoadSceneMode.Additive);
            gameObject.SetActive(false);
        }
    }
}
