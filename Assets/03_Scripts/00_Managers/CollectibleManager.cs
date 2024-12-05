using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;

    public int buildIndexLevel;

    [Space(10), Header("UI Coleccionables")]
    public UICollectible bongoUI;
    public UICollectible frankUI;

    private void Awake()
    {
        instance = this;
    }
}
