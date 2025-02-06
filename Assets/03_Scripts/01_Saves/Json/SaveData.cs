using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public LevelData[] levels;
    public bool playWithTimer;

    [Tooltip("Diccionario de los niveles. El booleano representa si el nivel está desbloqueado.")]public Dictionary<int, bool> UnlocklevelDataDictionary = new();

    [Space(15)]public float sfxVolume;
    public float musicVolume;

}
