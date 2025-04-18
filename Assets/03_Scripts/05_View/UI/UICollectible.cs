using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICollectible : MonoBehaviour
{
    [SerializeField] private CharacterTarget _player;
    private string _key;

    public enum ShowType { InGame, InCanvas }
    [HideInInspector] public ShowType myShowType;

    [Space(10), Header("-> Positions")]
    [Tooltip("La posicion que va a estar cuando se MUESTRE"), HideInInspector] public Vector2 showPos;

    [Tooltip("La posicion que va a estar cuando se ESCONDA"), HideInInspector] public Vector2 hidePos;


    [Range(0f, 3f), Tooltip("Tiempo que va a estar mostrandose"),HideInInspector] public float timeShow = 1.75f;

    [Range(0f, 1f), Tooltip("Velocidad de transición"),HideInInspector] public float speed = 0.5f;

    [Tooltip("Si se está mostrando")] private bool _show = false;
    [Tooltip("Es para la posicion en el canvas")] private RectTransform _rectTransform;

    [Space(10), Header("-> Color")]
    [HideInInspector] public Color deactiveColor;

    [HideInInspector] public Color activeColor;

    [SerializeField] private Image _image;
    private TextMeshProUGUI _txt;

    private int _totalAmountCollectible = 1;
    private bool _isTake = false;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _txt = GetComponentInChildren<TextMeshProUGUI>();
        _rectTransform = GetComponent<RectTransform>();


        //Si no hay ninguna refrencia de la UI en el GameManager, me agrego yo
        if (_player == CharacterTarget.Bongo)
        {
            if (myShowType == ShowType.InGame && !GameManager.instance.UICollBongo) GameManager.instance.UICollBongo = this;
            _key = "BongoTrinket";
        }

        else if (_player == CharacterTarget.Frank)
        {
            if(myShowType == ShowType.InGame && !GameManager.instance.UICollFrank) GameManager.instance.UICollFrank = this;

            _key = "FrankTrinket";
        }

        _isTake = CallJson.instance.refJasonSave.GetValueCollectableDict(GameManager.instance.IndexLevel, _key);

        if (myShowType == ShowType.InCanvas)
        {
            //SetUIToLevel(GameManager.instance.currentLevel.indexLevelJSON);
            SetUIToLevel(GameManager.instance.currentLevel.sceneReferenceSO.BuildIndex);

            return;
        }

        _rectTransform.anchoredPosition = hidePos;

    }

    private void OnEnable()
    {
        if (myShowType != ShowType.InCanvas || _image.color == activeColor) return;

        //Si no fue agarrado previamente el colleccionable, pongo el valor que está en el GameManager.
        if(!_isTake) _isTake = _player == CharacterTarget.Bongo ? GameManager.instance.isTakeCollBongo : GameManager.instance.isTakeCollFrank;

        _image.color = _isTake ? activeColor : deactiveColor;
    }

    /// <summary>
    /// Seteo el coleccionable respecto al nivel que pertenezca
    /// </summary>
    /// <param name="buildIndexLevel"></param>
    public void SetUIToLevel(int buildIndexLevel)
    {
        //Si no se agarro el coleccionable en este nivel
        if (!CallJson.instance.refJasonSave.GetValueCollectableDict(buildIndexLevel, _key))
        {
            //Debug.LogWarning($"No se agarro el: {gameObject.name}");

            //Desactivo el color en la UI
            _image.color = deactiveColor;

            //Actualizo el texto a 0
            if(myShowType == ShowType.InGame) UpdateCollectibleTxt(0);
        }

        //Caso contrario
        else
        {
            //Debug.LogWarning($"Se agarro el: {gameObject.name}!!!");

            _image.color = activeColor;

            if (myShowType == ShowType.InGame) UpdateCollectibleTxt(1);
        }
    }

    /// <summary>
    /// Avisa a la UI que tome el coleccionable y lo seteo
    /// </summary>
    public void UICollectibleTaken()
    {
        _image.color = activeColor;

        UpdateCollectibleTxt(1);
    }

    /// <summary>
    /// Actualizo el texto del coleccionable
    /// </summary>
    /// <param name="current"></param>
    private void UpdateCollectibleTxt(int current)
    {
        _txt.text = $"{current} / {_totalAmountCollectible}";
    }

    public void ShowUI()
    {
        if (!_show)
        {
            _show = true;

            StartCoroutine(Show(_show));
        }
    }

    IEnumerator Show(bool active)
    {
        float elapsedTime = 0;

        var actualPos = _rectTransform.anchoredPosition;

        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / speed;

            if(active) _rectTransform.anchoredPosition = Vector2.Lerp(actualPos, showPos, t);

            else _rectTransform.anchoredPosition = Vector2.Lerp(actualPos, hidePos, t);

            yield return null;
        }

        if(active)
        {
            yield return new WaitForSeconds(timeShow);

            StartCoroutine(Show(false));
        }

        else _show = false;
    }

}
