using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    [Header("-> Canvas")]
    //public Canvas gameOverCanvas;
    public Canvas pauseMenu;
    public Canvas winCanvas;
    public Canvas saveWarningCanvas;
    public Canvas gameOverCanvas;

    private bool _isPaused;
    [Space(10), SerializeField, Tooltip("Escena de carga asincrónica.")] private int _asyncScene = 1;
    [SerializeField] int _mainMenuScene;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused&&Time.timeScale==1)
            {
                PauseGame();
                
            }
            else if (_isPaused)
            {
                ResumeGame();

            }
        }
        //if(Time.timeScale==1)
        //{
        //    //gameOverCanvas.gameObject.SetActive(false);
        //    winCanvas.gameObject.SetActive(false);
        //}

        

    }

    public void GameOver()
    {
        //gameOverCanvas.gameObject.SetActive(true);
        //_animGameOverCanvas.SetTrigger("Enter");
        foreach (var item in GameManager.instance.rewinds)
        {
            item.Load();
        }

        //StartCoroutine(CanvasGameOver());

    }

    IEnumerator CanvasGameOver()
    {
        gameOverCanvas.gameObject.SetActive(true);


        yield return new WaitForSeconds(1.5f);

        foreach (var item in GameManager.instance.rewinds)
        {
            item.Load();
        }

        yield return new WaitForSeconds(1f);
        gameOverCanvas.gameObject.SetActive(false);

    }

    public void NextLvL(int scene)
    {
        //UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas
        //Debug.Log($"Mi Index es: {currentScene.buildIndex}");

        SceneManager.LoadSceneAsync(_asyncScene);
        AsyncLoad.sceneNumber = scene;
        //pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        //OldAudioManager.instance.StopAll();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        _isPaused = true;
    }

    public void SaveWarning() => saveWarningCanvas.gameObject.SetActive(true);
    public void DeactiveSaveWarningCanvas() => saveWarningCanvas.gameObject.SetActive(false);


    public void ReturnMainMenu()
    {
        GameManager.instance.rewinds.Clear();
        //GameManager.instance.enemies.Clear();

        SceneManager.LoadScene(_mainMenuScene);
        //pauseMenu.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; //Me bloque el mouse al centro de la pantalla
        Cursor.visible = false; //Me lo oculta
        Time.timeScale = 1;
        _isPaused = false;
    }

    public void RestartGame()
    {

        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas

        AsyncLoad.sceneNumber = currentScene.buildIndex;
        SceneManager.LoadSceneAsync(_asyncScene);
        //pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;


    }
}
