using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColectableManager : MonoBehaviour
{
    public static ColectableManager instance;
    public bool[] collectablesCollectedLvl1=new bool[2];
    public bool[] collectablesCollectedLvl2=new bool[2];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
