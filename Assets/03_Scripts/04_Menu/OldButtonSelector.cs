
using UnityEngine;
using UnityEngine.UI;



public class OldButtonSelector : MonoBehaviour
{
    private Animator _myAnimator;
    private Button _button;

    [Tooltip("Index del nivel")] public int indexLevel;
    private LevelData _currentLevel;

    [Space(7)] public CollectableMenu[] _collectables = new CollectableMenu[2];

    [Space(7)] public Image[] starImage = new Image[3];

    [SerializeField] private Color _deactiveColor;
    [SerializeField] private Color _activeColor;

    //public Button playWithChronometer;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        _button = GetComponent<Button>();

        ////Recorro cada nivel del Json
        //foreach (var level in CallJson.instance.refJasonSave.GetSaveData.levels)
        //{
        //    //Si su IndexLevel es el mismo que el del boton, lo guardo en el _currentLevel
        //    if (/*level.indexLevelJSON*/  level.sceneReferenceSO.BuildIndex == indexLevel)
        //    {
        //        _currentLevel = level;

        //        break;
        //    }
        //}
    }

    /// <summary>
    /// Cuando el mouse esta sobre el botón
    /// </summary>
    public void PointEnterFunc()
    {
        if (!_button.interactable) return;

        //MenuManager.Instance.IndexLevelToPlay = indexLevel;
        //playWithChronometer.gameObject.SetActive(true);

        //foreach (var collectable in _collectables)
        //{
        //    //if (_currentLevel.isLevelCompleteJSON) 
        //    collectable.imageCollectable.gameObject.SetActive(true);

        //    //Si el coleccionable es de Bongo, obtengo su booleano
        //    if (collectable.trinketCharacter == CharacterTarget.Bongo) collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "BongoTrinket");

        //    //Si es de Frank lo mismo
        //    else collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "FrankTrinket");

        //    if (collectable.isTaken) collectable.imageCollectable.color = collectable.activeColor;
        //    else collectable.imageCollectable.color = collectable.deactiveColor;
        //}

        foreach (var image in starImage)
        {
            image.gameObject.SetActive(true);
        }

        if (_currentLevel.isLevelCompleteJSON) starImage[0].color = _activeColor;

        if(_currentLevel.isTakeAllCoinsThisLevel) starImage[1].color = _activeColor;

        if(_currentLevel.isLevelCompleteWithChronometerJSON) starImage[2].color = _activeColor;
    }

    /// <summary>
    /// Cuando hago click en el botón
    /// </summary>
    public void PointClickFunc()
    {
        if (!_button.interactable) return;

        //MenuManager.Instance.PlayGame(indexLevel, _currentLevel.isLevelCompleteJSON);
    }

    /// <summary>
    /// Cuando el mouse sale del botón
    /// </summary>
    public void PointExitFunc()
    {
        if (!_button.interactable) return;

        //playWithChronometer.gameObject.SetActive(false);

        _myAnimator.SetTrigger("PointExit");

        foreach (var collectable in _collectables)
        {
            collectable.imageCollectable.gameObject.SetActive(false);
        }

        foreach (var image in starImage)
        {
            image.gameObject.SetActive(false);
        }

    }
}
