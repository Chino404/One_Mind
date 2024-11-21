using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdCollectable
{
    public string level;
    public int indexLevel;

    [Space(5)]
    public bool isBongoTaken;
    public bool isFrankTaken;
}

public class ColectableManager : MonoBehaviour
{
    public static ColectableManager instance;
    public IdCollectable[] collectablesLevels;

    //public bool[] collectablesCollectedLvl1 = new bool[2];
    //public bool[] collectablesCollectedLvl2 = new bool[2];
    //public bool[] collectablesCollectedLvl3 = new bool[2];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    //private void Start()
    //{
    //    var json = CallJson.instance.refJasonSave.GetSaveData.levels;

    //    for (int i = 0; i < json.Length; i++)
    //    {
    //        if (!json[i].collectablesJSON.ContainsKey("BongoTrinket"))
    //        {
    //            json[i].collectablesJSON.Add("BongoTrinket", false);
    //        }
    //        else Debug.Log($"Ya existe este diccionario de BONGO para {json[i].indexLevelJSON}");

    //        if(!json[i].collectablesJSON.ContainsKey("FrankTrinket"))
    //        {
    //            json[i].collectablesJSON.Add("FrankTrinket", false);
    //        }
    //        else Debug.Log($"Ya existe este diccionario de FRANK para {json[i].indexLevelJSON}");

    //    }
    //    Debug.Log("Holie");
    //}
}
