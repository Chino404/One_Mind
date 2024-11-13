using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JsonSaves : MonoBehaviour
{
    [SerializeField] SaveData saveData = new SaveData();
    string _path = ""; //Es la barra que aparece arriba en los archivos, para buscarlos

    private void Awake()
    {                                               //Obtengo la ubicacion de Mis documentos                  //Voy a la carpeta del nombre de la compañia y al nombre del juego  //A la carpeta de SaveData
        string customDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/" + Application.companyName + "/" + Application.productName + "/SaveData";//Mas pro, Customisiado

        if (!Directory.Exists(customDir)) //Si no existe la creo
            Directory.CreateDirectory(customDir);

        _path = customDir + "/Saves.Json";

        Debug.LogWarning($"Ubicacion del saveData: {_path}");
    }

    private void OnApplicationQuit()
    {
        SaveJSON();
    }

    private void Start()
    {
        CallJson.instance.refJasonSave = this;

        LoadJSON();

        var levels = CallJson.instance.refJasonSave.GetSaveData.levels; //Los niveles del saveData
        DictionaryCheck(levels);

        Debug.Log("JSON CARGO CHAVALES");
    }

    /// <summary>
    /// Guardar
    /// </summary>
    public void SaveJSON()
    {
        string json = JsonUtility.ToJson(saveData, true); //Hacemos un string en donde se va a crear el archivo de JSON y en los parametros le ponemos true para que me lo cree ordenado.
        File.WriteAllText(_path, json); //Me crea un archivo JSON con los datos que estan en SaveData, me lo escribe.
        JSONCheck();


        //Debug.Log(json);
    }

    /// <summary>
    /// Cargar
    /// </summary>
    public void LoadJSON()
    {
        JSONCheck();
        string json = File.ReadAllText(_path); //Me lee el archivo de esa ubicacion, es para acceder a archivos.
        JsonUtility.FromJsonOverwrite(json, saveData); //Sobrescribo los datos, le digo en donde esta (json) y le paso los datos (saveData).
    }

    private void JSONCheck()
    {
        if (!File.Exists(_path)) //Si no existe la carpeta buscada
        {
            Debug.LogWarning($"No existe ese camino para guardar/cargar.");

            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(_path, json); //Me crea un archivo JSON con los datos que estan en SaveData, me lo escribe.
        }
    }

    public void DeleteJSON()
    {

        if (File.Exists(_path))
        {
            File.Delete(_path);

            Debug.LogWarningFormat("Se borro el save data");

            CallJson.instance.refJasonSave.GetSaveData.levelDataDictionary.Clear();

            //CallJson.instance.save.GetSaveData.tutorialCompletedJSON = false;
            //CallJson.instance.save.GetSaveData.moneyJSON = 0;

            var levels = CallJson.instance.refJasonSave.GetSaveData.levels;
            //levels[0].isLevelCompleteJSON = true;

            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].isLevelCompleteJSON = false;
            }

            DictionaryCheck(levels);

            SceneManager.LoadScene(1);
        }
        else Debug.LogError("No existe el Path");


        //saveData = new SaveData();//Si quiero resetear los datos
    }

    private void DictionaryCheck(LevelData[] levelsList)
    {
        for (int i = 0; i < levelsList.Length; i++)
        {
            if (!CallJson.instance.refJasonSave.GetSaveData.levelDataDictionary.ContainsKey(levelsList[i].indexLevelJSON))//Si no existe el nivel en el diccionario lo creo con su respectivo booleano
            {
                CallJson.instance.refJasonSave.GetSaveData.levelDataDictionary.Add(levelsList[i].indexLevelJSON, levelsList[i].isLevelCompleteJSON);
                Debug.Log($"IndexBuild: {levelsList[i].indexLevelJSON} | IsLevelComplete: {levelsList[i].isLevelCompleteJSON}");
            }
            else Debug.LogWarning($"YA EXISTE! Key: {levelsList[i].indexLevelJSON} | Value: {levelsList[i].isLevelCompleteJSON}");

        }
        Debug.LogWarning("Cargue el diccionario");
    }

    public SaveData GetSaveData { get { return saveData; } }
}
