using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class CollectableMenu
{
    //[Tooltip("Nombre del personaje al que pertenece el coleccionable")]public string playerCollectableName;
    [Tooltip("A quien pertenece este coleccionable")] public CharacterTarget trinketCharacter;
    [Tooltip("Imagen del coleccionble")] public Image imageCollectable;

    [Space(10), Tooltip("Color DESACTIVADO")] public Color deactiveColor;
    [Tooltip("Color ACTIVO")] public Color activeColor;

    [Space(10), Tooltip("Si fue agarrado")] public bool isTaken;
}

public class ButtonLevelSelector : MonoBehaviour
{
    private Button _button;
    private LevelData _currentLevel;

    [Header("-> General Config, Button")]
    [SerializeField] private string _levelName;
    //[Tooltip("Index del nivel")] public int indexLevel;
    [Tooltip("Escena con el video cinemática")] public SceneReferenceSO sceneVideo;
    [Tooltip("Escena a la que voy a ir")] public SceneReferenceSO sceneReference;
    private SceneReferenceSO _scene;
    [SerializeField] private int _totalCoins = 0;

    [Space(10)]
    [SerializeField] private Image _lockedImage;
    [SerializeField] private GameObject _ui;
    [SerializeField, Tooltip("Tiempo de transición de escala"),Range(0.1f, 1f)] private float _timeLerpScale;
    private Vector3 _iniScale = new Vector3(0,0,0);
    private Vector3 _endScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Space(7),Header("-> Config. Collec. Image")]
    public CollectableMenu[] collectablesList = new CollectableMenu[2];

    [Space(7), Header("-> Config. Star Image")]
    public Image[] starImage = new Image[3];
    [SerializeField] private Color _activedStarColor;

    [Space(7), Header("-> Config. Text")]
    [SerializeField] private TextMeshProUGUI _txmpLvelName;
    [SerializeField] private TextMeshProUGUI _txmpTotalCoins;
    [SerializeField] private TextMeshProUGUI _txmpMyBestTime;


    private void Awake()
    {
        _button = GetComponent<Button>();

        //_currentLevel = CallJson.instance.refJasonSave.GetCurrentLevel(indexLevel);
        _currentLevel = CallJson.instance.refJasonSave.GetCurrentLevel(sceneReference.BuildIndex);

        //Si tengo algo escrito en el nombre del nivel
        if (_levelName != string.Empty && _txmpLvelName != null) _txmpLvelName.text = _levelName;

        //El total de las orbes
        if (_txmpTotalCoins != null) _txmpTotalCoins.text = $"{_currentLevel.coinsObtainedBongoSide + _currentLevel.coinsObtainedFrankSide} / {_totalCoins}";

        if (_txmpMyBestTime != null && _currentLevel.myBestTimeRecord.isBusy && _currentLevel.isLevelCompleteJSON)
        {
            _txmpMyBestTime.text = _currentLevel.myBestTimeRecord.txtTimeInMinutes;
            _txmpMyBestTime.gameObject.SetActive(true);
        }
        else _txmpMyBestTime.gameObject.SetActive(false);

        _ui.gameObject.SetActive(false);

        if (!_currentLevel.isUnlockLevelJSON) _button.interactable = false;
        else
        {
            _button.interactable = true;
            _lockedImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Cuando el mouse esta sobre el botón
    /// </summary>
    public void PointEnterFUnc()
    {
        if (!_button.interactable) return;

        foreach (var collectable in collectablesList)
        {
            //if (_currentLevel.isLevelCompleteJSON) 
            collectable.imageCollectable.gameObject.SetActive(true);

            //Si el coleccionable es de Bongo, obtengo su booleano
            //if (collectable.trinketCharacter == CharacterTarget.Bongo) collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "BongoTrinket");
            if (collectable.trinketCharacter == CharacterTarget.Bongo) collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(sceneReference.BuildIndex, "BongoTrinket");

            //Si es de Frank lo mismo
            //else collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "FrankTrinket");
            else collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(sceneReference.BuildIndex, "FrankTrinket");

            if (collectable.isTaken) collectable.imageCollectable.color = collectable.activeColor;
            else collectable.imageCollectable.color = collectable.deactiveColor;
        }

        _ui.gameObject.SetActive(true);

        StartCoroutine(SwitchScale());

        //foreach (var image in starImage)
        //{
        //    image.gameObject.SetActive(true);
        //}

        if (_currentLevel.isLevelCompleteJSON) starImage[0].color = _activedStarColor;

        if (_currentLevel.isTakeAllCoinsThisLevel) starImage[1].color = _activedStarColor;

        if (_currentLevel.isLevelCompleteWithChronometerJSON) starImage[2].color = _activedStarColor;
    }

    IEnumerator SwitchScale()
    {
        float currentTime = 0;

        while (currentTime < _timeLerpScale)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / _timeLerpScale;

            _ui.transform.localScale = Vector3.Lerp(_iniScale, _endScale, progress);

            yield return null;
        }

        _ui.transform.localScale = _endScale;
    }

    /// <summary>
    /// Cuando hago click en el botón
    /// </summary>
    public void PointClickFunc()
    {
        if (!_button.interactable) return;

        //MenuManager.Instance.PlayGame(indexLevel, _currentLevel.isLevelCompleteJSON);
        MenuManager.Instance.PlayGame(sceneReference, sceneVideo != null ? sceneVideo : null , _currentLevel.isLevelCompleteJSON);
    }

    /// <summary>
    /// Cuando el mouse sale del botón
    /// </summary>
    public void PointExitFunc()
    {
        if (!_button.interactable) return;

        _ui.gameObject.SetActive(false);

        _ui.transform.localScale = _iniScale;

    }
}
