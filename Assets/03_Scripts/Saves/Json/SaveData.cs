using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public LevelData[] levels;

    public Dictionary<int, bool> levelDataDictionary = new();

    [Space(15)]public float sfxVolume;
    public float musicVolume;
}
