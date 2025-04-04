
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TimeChronometer
{
    public string name;

    [HideInInspector, Tooltip("Si ya esta usado este espacio")] public bool isBusy;

    [SerializeField] private float _timeInSeconds;
    public float TimeInSeconds
    {
        get { return _timeInSeconds; }

        set
        {
            _timeInSeconds = value;

            int minutes = Mathf.FloorToInt(_timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(_timeInSeconds % 60);
            float fractionalSeconds = _timeInSeconds % 1;
            int decimals = Mathf.FloorToInt(fractionalSeconds * 100);

            txtTimeInMinutes = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, decimals);
        }
    }

    [HideInInspector]public string txtTimeInMinutes;
}

[Serializable]
public class LevelData
{
    [Header("-> NORMAL VALUES")]
    public int indexLevelJSON;
    public bool isUnlockLevelJSON;
    public bool isLevelCompleteJSON;

    //Monedas
    [Space(10), Header("-> COINS")]
    public bool isTakeAllCoinsThisLevel;
    [Tooltip("Monedas ya obtenidas y guardadas del lado de Bongo")] public int coinsObtainedBongoSide = 0;
    [Tooltip("Monedas ya obtenidas y guardadas del lado de Frank")] public int coinsObtainedFrankSide = 0;
    [Tooltip("Diccionario de las monedas")] public Dictionary<string, bool> dictCoinsJSON= new ();
    public string txtCoinsJSON;

    //Coleccionables
    [Space(10), Header("-> COLLECTABLES")]
    public bool isBongoTakenTrinket;
    public bool isFrankTakenTrinket;
    public Dictionary<string, bool> collectablesJSON = new();

    //Cronómetro
    [Space(10), Header("-> CHRONOMETER")]
    public bool isLevelCompleteWithChronometerJSON;
    public TimeChronometer myBestTimeRecord;
    [Tooltip("Mejores Tiempos")]public TimeChronometer[] bestTimesJSON = new TimeChronometer[3];

    /// <summary>
    /// Obtengo la moneda
    /// </summary>
    /// <param name="name"></param>
    public void TakeMoneyLevelData(string name)
    {
        dictCoinsJSON[name] = true;

        txtCoinsJSON = JsonConvert.SerializeObject(dictCoinsJSON, Formatting.Indented);
    }

    public void SaveTimeInOrder(TimeChronometer newTime, bool isMyTime = false)
    {
        TimeChronometer currentTimeChronometer = newTime;

        if (!myBestTimeRecord.isBusy && isMyTime)
        {
            myBestTimeRecord = currentTimeChronometer;
            myBestTimeRecord.isBusy = true;
            Debug.Log("No habia anda y guarde");
        }
        else if (currentTimeChronometer.TimeInSeconds < myBestTimeRecord.TimeInSeconds && isMyTime)
        {
            Debug.Log($"{currentTimeChronometer.name} el tiempo es mejor que {myBestTimeRecord.name}");

            myBestTimeRecord = currentTimeChronometer;
            myBestTimeRecord.isBusy = true;

        }

        for (int i = 0; i < bestTimesJSON.Length; i++)
        {
            //Si son lo mismo, entonces corto con el for para que no se guarde en los siguientes indices
            if (newTime.name == bestTimesJSON[i].name && newTime.TimeInSeconds == bestTimesJSON[i].TimeInSeconds) return;

            //Si no hay nada en este indice, lo guardo acá
            if (!bestTimesJSON[i].isBusy)
            {
                SaveBestTime(i, currentTimeChronometer);

                return;
            }

            //Sino pregunto si su tiempo es menor que el que esta guardado, si es así lo sobrescribo y me guardo la otra referencia
            else if (currentTimeChronometer.TimeInSeconds < bestTimesJSON[i].TimeInSeconds)
            {
                //Me guardo el actual
                TimeChronometer aux = bestTimesJSON[i];

                SaveBestTime(i, currentTimeChronometer);

                currentTimeChronometer = aux;
            }

        }
    }

    /// <summary>
    /// Guardo el record del tiempo
    /// </summary>
    /// <param name="index"></param>
    /// <param name="timeRecord"></param>
    private void SaveBestTime(int index, TimeChronometer timeRecord)
    {
        bestTimesJSON[index] = timeRecord;

        var time = bestTimesJSON[index].TimeInSeconds;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        float fractionalSeconds = time % 1;
        int decimals = Mathf.FloorToInt(fractionalSeconds * 100);

        bestTimesJSON[index].txtTimeInMinutes = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, decimals);

        bestTimesJSON[index].isBusy = true;
    }

    /// <summary>
    /// Chequeo el nuevo tiempo con los que hay guardados.
    /// </summary>
    /// <param name="newTimeChronometer"></param>
    public (bool,int) CheckNewTimeWithTheBestTimes(TimeChronometer newTimeChronometer) //Devuelve una Tupla
    {
        TimeChronometer currentTimeChronometer = newTimeChronometer;

        bool checkNewRecord = false;
        int index = 0;

        //Recorro el próximo index que me mandaron por parámetro
        for (int i = 0; i < bestTimesJSON.Length; i++)
        {
            //Si la referencia vieja es menor tiempo que el próximo index
            if (currentTimeChronometer.TimeInSeconds < bestTimesJSON[i].TimeInSeconds)
            {
                checkNewRecord = true;

                index = i;
                
                break;
            }
        }

        return (checkNewRecord, index);
    }

    public void DefalutValues()
    {
        isLevelCompleteJSON = false;

        isUnlockLevelJSON = false;

        //Monedas
        isTakeAllCoinsThisLevel = false;
        txtCoinsJSON = string.Empty;
        coinsObtainedBongoSide = 0;
        coinsObtainedFrankSide = 0;
        dictCoinsJSON.Clear();

        //Coleccionables
        isBongoTakenTrinket = false;
        isFrankTakenTrinket = false;
        collectablesJSON.Clear();

        //Cronómetro
        myBestTimeRecord = default;
        isLevelCompleteWithChronometerJSON = false;
        bestTimesJSON = new TimeChronometer[3];

    }
}
