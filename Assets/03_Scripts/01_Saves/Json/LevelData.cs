
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TimeChronometer
{
    [HideInInspector ,Tooltip("Si ya esta usado este espacio")]public bool isBusy;
    public string name;
    public float timeInSeconds;
    [HideInInspector] public string timeInMinutesTxt;
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

        var time = bestTimesJSON[index].timeInSeconds;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        float fractionalSeconds = time % 1;
        int decimals = Mathf.FloorToInt(fractionalSeconds * 100);

        bestTimesJSON[index].timeInMinutesTxt = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, decimals);

        bestTimesJSON[index].isBusy = true;
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
