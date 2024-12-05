using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CharacterTarget
{
    Bongo,
    Frank
}

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Rewind> rewinds;

    [SerializeField]private int _indexLevel;
    public int IndexLevel
    {
        get { return _indexLevel; }

        set
        {
            _indexLevel = value;

            SetUICollectible(_indexLevel);
        }
    }

    [Space(10), Header("-> Collecctibles")]
    public List<CollectibleFaced> collectibles;

    public UICollectible UIBongoTrincket;
    public UICollectible UIFrankTrincket;

    [Space(10), Header("-> Camera Config.")]
    public CameraTracker bongoCamera;
    [HideInInspector]public ModelBongo bongo;

    public CameraTracker frankCamera;
    [HideInInspector]public ModelFrank frank;

    public List<PointsForTheCamera> points = new ();


    [Space(15)]public List<Enemy> enemies = new();

    [Range(0f, 4f)]
    public float weightSeparation, weightAlignment, weightSeek;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Debug.Log("AWAKE GAMEMANAGER");
    }

    private void Start()
    {
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas
        IndexLevel = currentScene.buildIndex;

        if (!bongo) Debug.LogError("Falta la referencia de BONGO");
        if (!frank) Debug.LogError("Falta la referencia de FRANK");
        

        foreach (var item in rewinds)
        {
            item.Save();
            
        }

        //foreach (var item in points)
        //{
        //    if (item == null) continue;

        //    if (item.player == null)
        //    {
        //        if (item.characterTarget == CharacterTarget.Bongo) item.player = bongo.transform;
        //        else if (item.characterTarget == CharacterTarget.Frank) item.player = frank.transform;
        //    }
        //}
    }

    public void RemoveAll()
    {
        foreach (var item in rewinds)
        {
            rewinds.Remove(item);
        }
    }

    public int SetCollectible(Collectible collectible, CharacterTarget targetPlayer)
    {
        int buildIndex = 0;

        foreach (var faced in collectibles)
        {
            //if (faced.isCollectiblesCompleted) continue;

            if(targetPlayer == CharacterTarget.Bongo)
            {
                if (!faced.trincketBongo)
                {
                    faced.trincketBongo = collectible;
                    buildIndex = faced.buildIndexLevel;
                }
            }
            else
            {
                if (!faced.trincketFrank)
                {
                    faced.trincketFrank = collectible;
                    buildIndex = faced.buildIndexLevel;
                }
            }
        }

        return buildIndex;
    }

    /// <summary>
    /// Seteo el nuevo levelIndex para las UI de los coleccionables
    /// </summary>
    /// <param name="buildIndexLevel"></param>
    public void SetUICollectible(int buildIndexLevel)
    {
        if (!UIBongoTrincket)
        {
            Debug.LogError($"Falta la referencia de {UIBongoTrincket.name}");
            return;
        }

        if (!UIFrankTrincket)
        {
            Debug.LogError($"Falta la referencia de {UIFrankTrincket.name}");
            return;
        }

        UIBongoTrincket.SetUIToLevel(buildIndexLevel);
        UIFrankTrincket.SetUIToLevel(buildIndexLevel);
    }

}

