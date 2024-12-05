using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFacade : MonoBehaviour
{
    [Tooltip("Index que van a tener los coleccioanbles de este nivel")]public int buildIndexLevel;

    [Space(5), Header("UI Coleccionables (Se setea solo)")]
    public Collectible trincketBongo;
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
