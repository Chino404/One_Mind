using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [Tooltip("Index que van a tener los coleccioanbles de este nivel")] public int buildIndexLevel;

    [Space(5), Header("UI Coleccionables (Se setea solo)")]
    public Collectible trincketBongo;
    public Collectible trincketFrank;

    private void Awake()
    {
        GameManager.instance.collectiblesList.Add(this);
    }

    private void Start()
    {
        GameManager.instance.UIBongoTrincket.ShowUI();
        GameManager.instance.UIFrankTrincket.ShowUI();
    }

    private void OnDestroy()
    {
        GameManager.instance.collectiblesList.Remove(this);
    }

}
