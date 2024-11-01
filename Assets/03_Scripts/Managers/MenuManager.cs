using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private BookAnim _refBookAnim;

    [Space(10), SerializeField]private int _asyncScene;
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _levelSelectorCanvas;

    [Space(10), SerializeField]private Image[] _colectablesLvl1; 
    [SerializeField] private Image[] _colectablesLvl2;

    [Tooltip("En este array se ponen todos los botones que empiezan desactivados. El primero del array no se desactiva")]
    public Button[] buttons;

    

    private void Awake()
    {
        
    }

    private void Start()
    {
        Time.timeScale = 1;
        foreach (var item in ColectableManager.instance.collectablesCollectedLvl1)
        {
            if (item == true)
            {
                foreach (var elem in _colectablesLvl1)
                {
                    //if (elem.color != Color.green)
                    //{
                    //    elem.color = Color.green;
                    //    break;
                    //}

                    if (elem.color.a != 1f)
                    {
                        Color newColor = elem.color;
                        newColor.a = 1f;
                        elem.color = newColor;
                        break;
                    }

                }

            }

            //else foreach (var elem in _colectablesLvl1)
            //        if (elem.color != Color.green)
            //            elem.color = Color.red;
        }

        foreach (var item in ColectableManager.instance.collectablesCollectedLvl2)
        {
            if (item == true)
            {
                foreach (var elem in _colectablesLvl2)
                {
                    //if (elem.color != Color.green)
                    //{
                    //    elem.color = Color.green;
                    //    break;
                    //}

                    if (elem.color.a != 1f)
                    {
                        Color newColor = elem.color;
                        newColor.a = 1f; 
                        elem.color = newColor;
                        break;
                    }

                }

            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadSceneAsync(_asyncScene);
            AsyncLoad.sceneNumber = 2;
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadSceneAsync(_asyncScene);
            AsyncLoad.sceneNumber = 3;
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadSceneAsync(_asyncScene);
            AsyncLoad.sceneNumber = 4;
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadSceneAsync(_asyncScene);
            AsyncLoad.sceneNumber = 5;
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Se borro la data");
        }

    }

    public void PlayGame(int sceneNumber)
    {
        SceneManager.LoadSceneAsync(_asyncScene);
        AsyncLoad.sceneNumber = sceneNumber;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelSelector()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }


        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
        //_levelSelectorCanvas.gameObject.SetActive(true);
        //_mainMenuCanvas.gameObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        
        _mainMenuCanvas.gameObject.SetActive(true);
        _levelSelectorCanvas.gameObject.SetActive(false);
    }
    

}

    
