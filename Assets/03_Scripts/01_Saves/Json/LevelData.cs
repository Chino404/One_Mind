
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
    public Dictionary<string, bool> coinsJSON= new ();

    [UnityEngine.Space(7)]
    public bool isBongoTakenTrinket;
    public bool isFrankTakenTrinket;

    public Dictionary<string, bool> collectablesJSON = new();


    public void DefalutValues()
    {
        isLevelCompleteJSON = false;

        isUnlockLevelJSON = false;
        isLevelCompleteWithChronometerJSON = false;

        totalCoin = 0;
        coinsJSON.Clear();

        isBongoTakenTrinket = false;
        isFrankTakenTrinket = false;
        collectablesJSON.Clear();
    }
}
