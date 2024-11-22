
using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public int indexLevelJSON;
    public bool isLevelCompleteJSON;

    public Dictionary<string, bool> collectablesJSON = new();

    public bool isBongoTakenTrinket;
    public bool isFrankTakenTrinket;
    
}
