using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class ButtonsLevelSelector
{
    public int indexlevel;
    public Button button;

}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("-> Config.")]
    [SerializeField, Tooltip("Escena a cargar")] private int _asyncScene; //Escena a cargar
    [SerializeField] private BookAnim _refBookAnim; //Referencia del Modelo del libro


    [Space(5), Header("-> Canvas")]
    [SerializeField, Tooltip("Canvas del menú principal")] private Canvas _mainMenuCanvas;
    [SerializeField, Tooltip("Canvas de los botones del selector de niveles")] private Canvas _levelSelectorCanvas;
    [SerializeField, Tooltip("Canvas del cronometro")] private Canvas _stopwatchCanvas;

    [Space(5), Header("-> Botones"), Tooltip("En este array se ponen todos los botones que empiezan desactivados. El primero del array no se desactiva")]
    public ButtonsLevelSelector[] buttons;

    private int _indexLevelToPlay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
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
            //PlayerPrefs.DeleteAll();

            CallJson.instance.refJasonSave.DeleteJSON();
        }

        //if(Input.GetKeyDown(KeyCode.U))
        //{
        //    for (int i = 0; i < buttons.Length; i++)
        //    {
        //        CallJson.instance.refJasonSave.GetSaveData.UnlocklevelDataDictionary[buttons[i].indexlevel] = true;
        //    }

        //}

        //var levelData = new LevelData() { isLevelComplete = true, _collectables = new Dictionary<string, bool>() };
        //levelData._collectables.Add("First", true);
        //levelData._collectables.Add("Second", false);

        //var serializedData = JsonUtility.ToJson(levelData);

        //PlayerPrefs.SetString("LevelData", serializedData);

        ////Esto crea en la carpeta de la build o proyecto
        //Directory.CreateDirectory(Application.dataPath);

        ////Con path

        //if (!Directory.Exists("C://Documentos/MonoSaves"))
        //{
        //    Directory.CreateDirectory("C://Documentos/MonoSaves");
        //}

        //if (File.Exists("C://Documentos/MonoSaves/SaveData.txt"))
        //{
        //    File.Delete("C://Documentos/MonoSaves/SaveData.txt");
        //}
        //var reader = File.CreateText("c/documentos");
        //reader.Write(serializedData);
        //reader.Close();
    }

    /// <summary>
    /// Jugar nivel.
    /// </summary>
    /// <param name="sceneNumber"></param>
    /// <param name="levelComplete"></param>
    public void PlayGame(int sceneNumber, bool levelComplete = false)
    {
        _indexLevelToPlay = sceneNumber;

        if(levelComplete)
        {
            _stopwatchCanvas.gameObject.SetActive(true);
        }
        else
        {
            PlayInNormalMode();
        }
    }

    /// <summary>
    /// Jugar en modo normal el nivel.
    /// </summary>
    public void PlayInNormalMode()
    {
        SceneManager.LoadSceneAsync(_asyncScene);
        AsyncLoad.sceneNumber = _indexLevelToPlay;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Jugar con el cronómetro.
    /// </summary>
    /// <param name="scene"></param>
    public void PlayWithChronometer()
    {
        CallJson.instance.refJasonSave.GetSaveData.playWithTimer = true;
        CallJson.instance.refJasonSave.SaveJSON();
        PlayGame(_indexLevelToPlay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelSelector()
    {
        var levelsList = CallJson.instance.refJasonSave.GetSaveData.levels;

        levelsList[0].isUnlockLevelJSON = true;

        //var dictUnlockLevel = CallJson.instance.refJasonSave.GetSaveData.UnlocklevelDataDictionary;
        //dictUnlockLevel[levelsList[0].indexLevelJSON] = true;

        for (int i = 0; i < buttons.Length; i++)
        {
            //Con el DICCIONARIO
            //if (dictUnlockLevel.ContainsKey(levelsList[i].indexLevelJSON)) 
            //{
            //    buttons[i].button.interactable = dictUnlockLevel[levelsList[i].indexLevelJSON];//Accedo al booleano del nivel para saber si está desbloqueado

            //    //ButtonSelector mainButton = buttons[i].button.GetComponent<ButtonSelector>();
            //    //mainButton.playWithChronometer.interactable = dict[level[i].indexLevelJSON + 1];

            //}

            //else Debug.Log($"Esta key {levelsList[i].indexLevelJSON} no existe");

            buttons[i].button.interactable = levelsList[i].isUnlockLevelJSON;
        }

    }

    public void BackToMainMenu()
    {

        _mainMenuCanvas.gameObject.SetActive(true);
        _levelSelectorCanvas.gameObject.SetActive(false);
    }


}

//[Serializable]
//public class CongifData
//{
//    public float sfxVolume;
//    public float musicVolume;

//}

