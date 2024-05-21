using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]int _asyncScene;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void PlayGame(int sceneNumber)
    {
        AsyncLoad.sceneNumber = sceneNumber;
        SceneManager.LoadSceneAsync(_asyncScene);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

    