using System.Collections.Generic;
using Unity.VisualScripting;
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

    [HideInInspector, Tooltip("Nivel actual")] public LevelData currentLevel;
    [SerializeField] private int _indexLevel;
    [SerializeField] private bool _scenePractice;
    public int IndexLevel
    {
        get { return _indexLevel; }

        set
        {
            _indexLevel = value;

            UpdateUICollectible(_indexLevel);
            //UpdateUICollectibleManager(_indexLevel);
        }
    }


    [Space(10), Header("-> Chronometer")] public bool isChronometerActive; 
    public TimeChronometer timeInLevel;
    [SerializeField] private TimeChronometer[] _myBestTimesInLevel = new TimeChronometer[2];
    [HideInInspector,Tooltip("El canvas en donde van a aparecer los mejores tiempos records")]public RecordBestTimesView UIBestTimesInlevel;




    [Space(10), Header("-> Collecctibles")]
    public List<CollectibleManager> collectiblesList;

    [HideInInspector]public UICollectible UIBongoTrincket;
    [HideInInspector] public UICollectible UIFrankTrincket;

    [Space(10), Header("-> Coins")]
    public int totalCoinsInLevel;
    [HideInInspector]public UICoins uiCoinBongo;
    public int totalCoinsBongoSide;
    [HideInInspector]public UICoins uiCoinFrank;
    public int totalCoinsFrankSide;

    [Space(10), Header("-> Camera Config.")]
    public CameraTracker bongoNormalCamera;
    public CameraRails bongoRailsCamera;
    [HideInInspector]public ModelBongo modelBongo;

    public CameraTracker frankNormalCamera;
    public CameraRails frankRailsCamera;
    [HideInInspector]public ModelFrank modelFrank;
    public List<PointsForTheCamera> points = new ();


    //[Space(15)] public List<Enemy> enemies = new();

    //[Range(0f, 4f)]
    //public float weightSeparation, weightAlignment, weightSeek;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        if(!_scenePractice)
        {
            foreach (var level in CallJson.instance.refJasonSave.GetSaveData.levels)
            {
                //Si su IndexLevel es el mismo que el del GameManager, lo guardo en el currentLevel
                if (level.indexLevelJSON == _indexLevel)
                {
                    currentLevel = level;
                    break;
                }
            }

            isChronometerActive = CallJson.instance.refJasonSave.GetSaveData.playWithTimer;
        }

        Debug.Log("AWAKE GAMEMANAGER");
    }

    private void Start()
    {
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas
        IndexLevel = currentScene.buildIndex;

        if (!modelBongo) Debug.LogError("Falta la referencia de BONGO");
        if (!modelFrank) Debug.LogError("Falta la referencia de FRANK");
        
        //Guardar en el memento
        foreach (var item in rewinds) item.Save();

        for (int i = 0; i < currentLevel.bestTimesJSON.Length; i++)
        {
            currentLevel.SaveTimeInOrder(_myBestTimesInLevel[i]);
        }
    }

    public void RemoveAll()
    {
        foreach (var item in rewinds)
        {
            rewinds.Remove(item);
        }
    }

    public int SetCollectibleManager(Collectible collectible) => CollectibleFacade.SetCollectible(collectible, collectiblesList);

    private void UpdateUICollectible(int buildIndexLevel) => CollectibleFacade.UpdateUICollectible(buildIndexLevel, UIBongoTrincket, UIFrankTrincket);

    ///// <summary>
    ///// Seteo el coleccionable a su respectivo lugar en base a la lista de CollectibleFade
    ///// </summary>
    ///// <param name="collectible"></param>
    ///// <param name="targetPlayer"></param>
    ///// <returns></returns>
    //public int SetCollectible(Collectible collectible, CharacterTarget targetPlayer)
    //{
    //    int buildIndex = 0;

    //    //Recorro cada CollectibleFaced 
    //    foreach (var faced in collectiblesList)
    //    {
    //        if(targetPlayer == CharacterTarget.Bongo)
    //        {
    //            //Si la referencia del colecicoanble de Bongo esta vac�a
    //            if (!faced.trincketBongo)
    //            {
    //                //Agrego la que me pasaron por parametro
    //                faced.trincketBongo = collectible;

    //                //Y le seteo el index del CollectibleFaced
    //                buildIndex = faced.buildIndexLevel;
    //            }
    //        }
    //        else
    //        {
    //            if (!faced.trincketFrank)
    //            {
    //                faced.trincketFrank = collectible;
    //                buildIndex = faced.buildIndexLevel;
    //            }
    //        }
    //    }

    //    return buildIndex;
    //}



    ///// <summary>
    ///// En base al nuevo levelIndex, actualizo la UI
    ///// </summary>
    ///// <param name="buildIndexLevel"></param>
    //public void UpdateUICollectibleManager(int buildIndexLevel)
    //{
    //    if (!UIBongoTrincket)
    //    {
    //        Debug.LogError($"Falta la referencia de {UIBongoTrincket.name}");
    //        return;
    //    }

    //    if (!UIFrankTrincket)
    //    {
    //        Debug.LogError($"Falta la referencia de {UIFrankTrincket.name}");
    //        return;
    //    }

    //    UIBongoTrincket.SetUIToLevel(buildIndexLevel);
    //    UIFrankTrincket.SetUIToLevel(buildIndexLevel);
    //}

}

