using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Canvas gameOverCanvas;
    public Canvas pauseMenu;
    public static PauseManager instance;
    bool _isPaused;
    [SerializeField] int _asyncScene;
    [SerializeField] int _mainMenuScene;
    

    private void Awake()
    {
        

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

    

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
                PauseGame();
            else if (_isPaused)
                ResumeGame();
        }
        if(Time.timeScale==1)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }

    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverCanvas.gameObject.SetActive(true);
    }

    public void NextLvL(int scene)
    {
        AsyncLoad.sceneNumber = scene;
        SceneManager.LoadSceneAsync(_asyncScene);
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true;
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(_mainMenuScene);
        pauseMenu.gameObject.SetActive(false);

    }

    public void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pauseMenu.gameObject.SetActive(false);

        Time.timeScale = 1;
    }
}
