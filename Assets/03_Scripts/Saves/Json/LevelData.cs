
using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public int indexLevelJSON;
    public bool isLevelCompleteJSON;

    public Dictionary<string, bool> collectablesJSON = new();
    
}
