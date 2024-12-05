using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFaced : MonoBehaviour
{
    public int buildIndexLevel;

    [Space(5), Header("UI Coleccionables")]
    public UICollectible bongoUI;
    public Collectible trincketBongo;

    [Space(10)] public UICollectible frankUI;
    public Collectible trincketFrank;

    private void Awake()
    {
        GameManager.instance.collectibles.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.instance.collectibles.Remove(this);
    }

}
