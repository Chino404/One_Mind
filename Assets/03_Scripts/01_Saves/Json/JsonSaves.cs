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

        LoadJSON();

    }

    private void OnApplicationQuit()
    {
        SaveJSON();
    }

    private void Start()
    {
        CallJson.instance.refJasonSave = this;

        //LoadJSON();

        DictionaryCheck(CallJson.instance.refJasonSave.GetSaveData.levels);

        //Debug.Log("JSON CARGO CHAVALES");
    }

    /// <summary>
    /// Guardar
    /// </summary>
    public void SaveJSON()
    {
        DictionaryCheck(CallJson.instance.refJasonSave.GetSaveData.levels);
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

            var levelList = CallJson.instance.refJasonSave.GetSaveData.levels;

            foreach (var currentLevel in levelList) currentLevel.DefalutValues();

            DictionaryCheck(levelList);

            SceneManager.LoadSceneAsync(0);
        }
        else Debug.LogError("No existe el Path");

        //saveData = new SaveData();//Si quiero resetear los datos
    }

    private void DictionaryCheck(LevelData[] levelList)
    {
        for (int i = 0; i < levelList.Length; i++)
        {
            //if (!CallJson.instance.refJasonSave.GetSaveData.UnlocklevelDataDictionary.ContainsKey(levelList[i].indexLevelJSON))//Si no existe el nivel en el diccionario lo creo con su respectivo booleano
            //{
            //    CallJson.instance.refJasonSave.GetSaveData.UnlocklevelDataDictionary.Add(levelList[i].indexLevelJSON, levelList[i].isUnlockLevelJSON);
            //    //Debug.Log($"IndexBuild: {levelList[i].indexLevelJSON} | IsLevelComplete: {levelList[i].isLevelCompleteJSON}");
            //}
            //else Debug.LogWarning($"YA EXISTE! Key: {levelList[i].indexLevelJSON} | Value: {levelList[i].isLevelCompleteJSON}");

            if (!levelList[i].collectablesJSON.ContainsKey("BongoTrinket")) levelList[i].collectablesJSON.Add("BongoTrinket", levelList[i].isBongoTakenTrinket);
            else
            {
                //Le asigno el valor del diccionario
                levelList[i].isBongoTakenTrinket = levelList[i].collectablesJSON["BongoTrinket"];

                //Debug.Log($"Ya existe este diccionario de BONGO para {levelList[i].indexLevelJSON}");
            }


            if (!levelList[i].collectablesJSON.ContainsKey("FrankTrinket")) levelList[i].collectablesJSON.Add("FrankTrinket", levelList[i].isFrankTakenTrinket);
            else
            {
                //Le asigno el valor del diccionario
                levelList[i].isFrankTakenTrinket = levelList[i].collectablesJSON["FrankTrinket"]; 

                //Debug.LogWarning($"Ya existe este diccionario de FRANK para {levelList[i].indexLevelJSON}");
            }


        }
        Debug.LogWarning("Cargue el diccionario");
    }

    /// <summary>
    /// Para modificar el booleano del coleccionable en el diccionario
    /// </summary>
    /// <param name="indexLevel"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void ModyfyValueCollectableDict(int indexLevel , string key, bool value)
    {
        var levelList = CallJson.instance.refJasonSave.GetSaveData.levels; //Los niveles de LevelData

        for (int i = 0; i < levelList.Length; i++)
        {
            if (levelList[i].indexLevelJSON == indexLevel)
            {
                if (levelList[i].collectablesJSON.ContainsKey(key)) levelList[i].collectablesJSON[key] = value; //Modifico el valor del Diccionario del Coleccionable

                else
                {
                    //Si no existe creo el diccionario con el valor
                    levelList[i].collectablesJSON.Add(key, value);
                    Debug.Log("Cree la Key del coleccionable");
                }
            }
        }

        DictionaryCheck(CallJson.instance.refJasonSave.GetSaveData.levels);
    }

    /// <summary>
    /// Obtengo el nivel actual
    /// </summary>
    /// <param name="indexManager"></param>
    /// <returns></returns>
    public LevelData GetCurrentLevel(int indexManager)
    {
        var levelList = CallJson.instance.refJasonSave.GetSaveData.levels; //Los niveles de LevelData
        LevelData indexValue = default;

        foreach (var level in levelList)
        {
            if (indexManager == level.indexLevelJSON)
            {
                indexValue = level;
                break;
            }
        }

        if(indexValue == default) Debug.LogError($"El index '{indexManager}' no existe.");

        return indexValue;
    }

    /// <summary>
    /// Para obtener el booleano del coleccionable en el diccioanrio
    /// </summary>
    /// <param name="indexLevel"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetValueCollectableDict(int indexLevel, string key)
    {
        var levelList = CallJson.instance.refJasonSave.GetSaveData.levels; //Los niveles de LevelData
        var valueKey = false;

        for (int i = 0; i < levelList.Length; i++)
        {
            if (levelList[i].indexLevelJSON == indexLevel && levelList[i].collectablesJSON.ContainsKey(key)) valueKey = levelList[i].collectablesJSON[key];
        }

        return valueKey;
    }

    public SaveData GetSaveData { get { return saveData; } }
}
