
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public int indexLevelJSON;
    public bool isUnlockLevelJSON;

    public bool isLevelCompleteJSON;
    public bool isLevelCompleteWithChronometerJSON;

    [UnityEngine.Space(7)]public int totalCoin;
    public Dictionary<string, bool> dictCoinsJSON= new ();
    public string txtCoinsJSON;

    [UnityEngine.Space(7)]
    public bool isBongoTakenTrinket;
    public bool isFrankTakenTrinket;

    public Dictionary<string, bool> collectablesJSON = new();

    public void MondeAgarrada(string name)
    {
        dictCoinsJSON[name] = true;

        txtCoinsJSON = JsonConvert.SerializeObject(dictCoinsJSON, Formatting.Indented);
    }

    public void DefalutValues()
    {
        isLevelCompleteJSON = false;

        isUnlockLevelJSON = false;
        isLevelCompleteWithChronometerJSON = false;

        txtCoinsJSON = string.Empty;

        totalCoin = 0;
        dictCoinsJSON.Clear();

        isBongoTakenTrinket = false;
        isFrankTakenTrinket = false;
        collectablesJSON.Clear();
    }
}
