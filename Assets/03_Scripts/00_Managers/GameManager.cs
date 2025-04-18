using System.Collections.Generic;
using TMPro;
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


    [Space(10), Header("-> Chronometer")]
    [Tooltip("Si se esta jugando con el cronómetro.")]public bool isChronometerActive; 
    public TimeChronometer timeInLevel;
    [SerializeField] private TimeChronometer[] _myBestTimesInLevel = new TimeChronometer[3];
    public TimeChronometer[] BestTimesInLevel { get {  return _myBestTimesInLevel; } }
    [HideInInspector,Tooltip("El canvas en donde van a aparecer los mejores tiempos records")] public RecordBestTimesView UIBestTimesInlevel;
    [HideInInspector] public GameObject secondsInGame;


    [Space(10), Header("-> Collecctibles")]
    public bool isTakeCollBongo;
    [HideInInspector] public string nameCollBongo;
    [HideInInspector] public UICollectible UICollBongo;

    public bool isTakeCollFrank;
    [HideInInspector] public string nameCollFrank;
    [HideInInspector] public UICollectible UICollFrank;

    [Space(10), Header("-> Coins")]
    public int totalCoinsInLevel;
    [Tooltip("El nombre de cada moneda.")]public List<string> coinsNameList;

    [HideInInspector] public UICoins uiCoinBongo;
    [HideInInspector, Tooltip("Monedas totales del lado de Bongo")] public int totalCoinsBongoSide;
    [Tooltip("Monedas recolectadas actualmente del lado de Bongo")] public int currentCollectedCoinsBongo;

    [HideInInspector] public UICoins uiCoinFrank;
    [HideInInspector, Tooltip("Monedas totales del lado de Frank")] public int totalCoinsFrankSide;
    [Tooltip("Monedas recolectadas actualmente del lado de Frank")] public int currentCollectedCoinsFrank;


    [Space(10), Header("-> Camera Config.")]
    public CameraTracker bongoNormalCamera;
    public CameraRails bongoRailsCamera;
    [HideInInspector] public ModelBongo modelBongo;

    public CameraTracker frankNormalCamera;
    public CameraRails frankRailsCamera;
    [HideInInspector] public ModelFrank modelFrank;

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

        if (!_scenePractice)
        {
            foreach (var level in CallJson.instance.refJasonSave.GetSaveData.levels)
            {
                //Si su IndexLevel es el mismo que el del GameManager, lo guardo en el currentLevel
                //if (level.indexLevelJSON == _indexLevel)
                //{
                //    currentLevel = level;
                //    break;
                //}

                if (level.sceneReferenceSO.BuildIndex == _indexLevel)
                {
                    currentLevel = level;

                    Debug.Log($"El index del nivel es: {level.sceneReferenceSO.BuildIndex}");
                    break;
                }

            }

            currentCollectedCoinsBongo = currentLevel.coinsObtainedBongoSide;

            currentCollectedCoinsFrank = currentLevel.coinsObtainedFrankSide;

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

    #region Coleccionables
    //public int SetCollectibleManager(Collectible collectible) => CollectibleFacade.SetCollectible(collectible, collectiblesList);

    public void UpdateUICollectible(int buildIndexLevel) => CollectibleFacade.UpdateUICollectible(buildIndexLevel, UICollBongo, UICollFrank);
    #endregion
}

