
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TimeChronometer
{
    [HideInInspector ,Tooltip("Si ya esta usado este espacio")] public bool isBusy;
    public string name;

    [SerializeField]private float _timeInSeconds;

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

    public string txtTimeInMinutes;
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
    public int currentCoins;
    public Dictionary<string, bool> dictCoinsJSON= new ();
    public string txtCoinsJSON;

    //Coleccionables
    [Space(10), Header("-> COLLECTABLES")]
    public bool isBongoTakenTrinket;
    public bool isFrankTakenTrinket;
    public Dictionary<string, bool> collectablesJSON = new();

    //Cronómetro
    [Space(10), Header("-> CHRONOMETER")]
    public bool isLevelCompleteWithChronometerJSON;
    [Tooltip("Mejores Tiempos")]public TimeChronometer[] bestTimesJSON = new TimeChronometer[3];

    /// <summary>
    /// Obtengo la moneda
    /// </summary>
    /// <param name="name"></param>
    public void TakeMoney(string name)
    {
        dictCoinsJSON[name] = true;

        txtCoinsJSON = JsonConvert.SerializeObject(dictCoinsJSON, Formatting.Indented);
    }

    /// <summary>
    /// Guardo el record del tiempo
    /// </summary>
    /// <param name="index"></param>
    /// <param name="timeRecord"></param>
    public void SaveBestTime(int index, TimeChronometer timeRecord)
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
    public bool CheckNewTimeWithTheBestTimes(TimeChronometer newTimeChronometer, bool saveNow = true)
    {
        TimeChronometer currentTimeChronometer = newTimeChronometer;

        bool checkNewRecord = false;

        //Recorro el próximo index que me mandaron por parámetro
        for (int i = 0; i < bestTimesJSON.Length; i++)
        {
            TimeChronometer aux = default;
            //Si la referencia vieja es menor tiempo que el próximo index
            if (currentTimeChronometer.TimeInSeconds < bestTimesJSON[i].TimeInSeconds)
            {
                if(!checkNewRecord) checkNewRecord = true;

                aux = bestTimesJSON[i];

                if(saveNow) SaveBestTime(i, currentTimeChronometer); //Lo guardo en su lugar

                currentTimeChronometer = aux;

            }

        }

        return checkNewRecord;
    }

    public void DefalutValues()
    {
        isLevelCompleteJSON = false;

        isUnlockLevelJSON = false;

        //Monedas
        isTakeAllCoinsThisLevel = false;
        txtCoinsJSON = string.Empty;
        currentCoins = 0;
        dictCoinsJSON.Clear();

        //Coleccionables
        isBongoTakenTrinket = false;
        isFrankTakenTrinket = false;
        collectablesJSON.Clear();

        //Cronómetro
        isLevelCompleteWithChronometerJSON = false;
        bestTimesJSON = new TimeChronometer[3];

    }
}
