using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]int _asyncScene;
    [SerializeField] Canvas _levelSelectorCanvas;
    [SerializeField] Canvas _mainMenuCanvas;
    public Image[] _colectablesLvl1; 
    public Image[] _colectablesLvl2;

    

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
        _levelSelectorCanvas.gameObject.SetActive(true);
        _mainMenuCanvas.gameObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        
        _mainMenuCanvas.gameObject.SetActive(true);
        _levelSelectorCanvas.gameObject.SetActive(false);
    }
}

    
