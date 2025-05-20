using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CharacterTarget
{
    Bongo,
    Frank,
    None
}

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Rewind> rewinds;

    [HideInInspector, Tooltip("Nivel actual")] public LevelData currentLevel;
    [SerializeField, Tooltip("Mi referencia de la escena.")] private SceneReferenceSO _mySceneReference;
    private int _indexLevel;
    public int IndexLevel { get { return _indexLevel; } }


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


    [Header("-> Player Confiog.")]
    public ModelBongo modelBongo;
    public ModelFrank modelFrank;
    public bool isNotMoveThePlayer;

    //[Space(10), Header("-> Camera Config.")]
    [HideInInspector] public CameraTracker bongoNormalCamera;
    [HideInInspector] public CameraRails bongoRailsCamera;

    [HideInInspector]public CameraTracker frankNormalCamera;
    [HideInInspector]public CameraRails frankRailsCamera;

    [HideInInspector]public List<PointsForTheCamera> pointsNormalCamera = new ();


    //[Space(15)] public List<Enemy> enemies = new();

    //[Range(0f, 4f)]
    //public float weightSeparation, weightAlignment, weightSeek;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"Destruí este GameManager (<color=red>{gameObject.name}</color>) porque ya existe en escena el <color=yellow>{GameManager.instance.gameObject.name}</color>");
            Destroy(gameObject);
            return;
        }


        if (_mySceneReference)
        {
            _indexLevel = _mySceneReference.BuildIndex;
        }
        else
        {
            Debug.LogError($"Falta la referencia de mi escena (<color=red><b>{gameObject.name}</color></b>)");
            return;
        }

        if (CallJson.instance == null)
        {
            Debug.LogError("Falta la instancia del <color=yellow>CallJson</color>");
            return;
        }




        foreach (var level in CallJson.instance.refJasonSave.GetSaveData.levels)
        {
            //Si su IndexLevel es el mismo que el del GameManager, lo guardo en el currentLevel
            if (level.indexLevelJSON /*level.sceneReferenceSO.BuildIndex*/ == _indexLevel)
            {
                currentLevel = level;
                break;
            }

        }

        currentCollectedCoinsBongo = currentLevel.coinsObtainedBongoSide;

        currentCollectedCoinsFrank = currentLevel.coinsObtainedFrankSide;

        isChronometerActive = CallJson.instance.refJasonSave.GetSaveData.playWithTimer;



        Debug.Log($"<color=yellow>AWAKE GAMEMANAGER</color>");
    }

    private void Start()
    {

        AudioManager.instance.Play(SoundId.Theme);
        //AudioSetting.instance.Play(SoundId.SoundLoop);

        UpdateUICollectible(_indexLevel);

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

