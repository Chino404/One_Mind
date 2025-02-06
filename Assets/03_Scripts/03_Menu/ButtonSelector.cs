
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CollectableMenu
{
    //[Tooltip("Nombre del personaje al que pertenece el coleccionable")]public string playerCollectableName;
    [Tooltip("A quien pertenece este coleccionable")]public CharacterTarget trinketCharacter;
    [Tooltip("Imagen del coleccionble")] public Image imageCollectable;

    [Space(10),Tooltip("Color DESACTIVADO")] public Color deactiveColor;
    [Tooltip("Color ACTIVO")] public Color activeColor;

    [Space(10), Tooltip("Si fue agarrado")] public bool isTaken;
}

public class ButtonSelector : MonoBehaviour
{
    private Animator _myAnimator;
    private Button _button;

    [Tooltip("Index del nivel")] public int indexLevel;
    private LevelData _currentLevel;
    public CollectableMenu[] _collectables = new CollectableMenu[2];

    //public Button playWithChronometer;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        _button = GetComponent<Button>();

        //Recorro cada nivel del Json
        foreach (var level in CallJson.instance.refJasonSave.GetSaveData.levels)
        {
            //Si su IndexLevel es el mismo que el del boton, lo guardo en el _currentLevel
            if (level.indexLevelJSON == indexLevel)
            {
                _currentLevel = level;

                break;
            }
        }
    }

    /// <summary>
    /// Cuando el mouse esta sobre el botón
    /// </summary>
    public void PointEnterFunc()
    {
        if (!_button.interactable) return;

        //MenuManager.Instance.IndexLevelToPlay = indexLevel;
        //playWithChronometer.gameObject.SetActive(true);

        foreach (var collectable in _collectables)
        {
            //if (_currentLevel.isLevelCompleteJSON) 
            collectable.imageCollectable.gameObject.SetActive(true);

            //Si el coleccionable es de Bongo, obtengo su booleano
            if (collectable.trinketCharacter == CharacterTarget.Bongo) collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "BongoTrinket");

            //Si es de Frank lo mismo
            else collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "FrankTrinket");

            if (collectable.isTaken) collectable.imageCollectable.color = collectable.activeColor;
            else collectable.imageCollectable.color = collectable.deactiveColor;
        }
    }

    /// <summary>
    /// Cuando hago click en el botón
    /// </summary>
    public void PointClickFunc()
    {
        if (!_button.interactable) return;

        MenuManager.Instance.PlayGame(indexLevel, _currentLevel.isLevelCompleteJSON);
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

    }
}
