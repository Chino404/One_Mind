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
    [SerializeField] private BookAnim _refBookAnim;

    [Space(10), SerializeField] private int _asyncScene;
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _levelSelectorCanvas;

    [Space(10), SerializeField] private Image[] _colectablesLvl1;
    [SerializeField] private Image[] _colectablesLvl2;

    [Tooltip("En este array se ponen todos los botones que empiezan desactivados. El primero del array no se desactiva")]
    public ButtonsLevelSelector[] buttons;


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

    public void PlayGame(int sceneNumber)
    {
        SceneManager.LoadSceneAsync(_asyncScene);
        AsyncLoad.sceneNumber = sceneNumber;
        Time.timeScale = 1;
    }

    public void PlayWithChronometer(int scene)
    {
        CallJson.instance.refJasonSave.GetSaveData.playWithTimer = true;
        CallJson.instance.refJasonSave.SaveJSON();
        PlayGame(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelSelector()
    {
        //int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        var level = CallJson.instance.refJasonSave.GetSaveData.levels;

        level[0].isLevelCompleteJSON = true; //Esto me va a permitir siempre ingresar al nivel 1
        CallJson.instance.refJasonSave.GetSaveData.levelDataDictionary[level[0].indexLevelJSON] = true;

        var dict = CallJson.instance.refJasonSave.GetSaveData.levelDataDictionary;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (dict.ContainsKey(level[i].indexLevelJSON)) 
            {
                buttons[i].button.interactable = dict[level[i].indexLevelJSON];//Accedo al booleano del nivel para saber si se completo
                ButtonSelector mainButton = buttons[i].button.GetComponent<ButtonSelector>();
                mainButton.playWithChronometer.interactable = dict[level[i].indexLevelJSON + 1];
            }

            else Debug.Log($"Esta key {level[i].indexLevelJSON} no existe");
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

