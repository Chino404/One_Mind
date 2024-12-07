using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICollectible : MonoBehaviour
{
    [SerializeField] private CharacterTarget _player;
    private string _key;

    [Space(10), Header("-> Positions")]
    [Tooltip("La posicion que va a estar cuando se MUESTRE"), SerializeField] private Vector2 _showPos;
    [Tooltip("La posicion que va a estar cuando se ESCONDA"), SerializeField] private Vector2 _hidePos;
    [Range(0f, 3f), Tooltip("Tiempo que va a estar mostrandose"),SerializeField] private float _timeShow = 1.75f;
    [Range(0f, 1f), Tooltip("Velocidad de transición"),SerializeField] private float _speed = 0.5f;

    [Tooltip("Si se está mostrando")] private bool _show = false;
    [Tooltip("Es para la posicion en el canvas")]private RectTransform _rectTransform;

    [Space(10), Header("-> Color")]
    [SerializeField] private Color _deactiveColor;
    [SerializeField] private Color _activeColor;

    private Image _image;
    private TextMeshProUGUI _txt;

    private int _totalAmountCollectible = 1;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _txt = GetComponentInChildren<TextMeshProUGUI>();
        _rectTransform = GetComponent<RectTransform>();

        //Si no hay ninguna refrencia de la UI en el GameManager, me agrego yo
        if (_player == CharacterTarget.Bongo && !GameManager.instance.UIBongoTrincket)
        {
            GameManager.instance.UIBongoTrincket = this;
            _key = "BongoTrinket";
        }

        else if (_player == CharacterTarget.Frank && !GameManager.instance.UIFrankTrincket)
        {
            GameManager.instance.UIFrankTrincket = this;
            _key = "FrankTrinket";
        }

        //Si ya hay, me destruyo
        else
        {
            Destroy(gameObject);
            return;
        }

        _rectTransform.anchoredPosition = _hidePos;
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
            //Desactivo el color en la UI
            _image.color = _deactiveColor;

            //Actualizo el texto a 0
            UpdateCollectibleTxt(0);
        }

        //Caso contrario
        else
        {
            _image.color = _activeColor;
            UpdateCollectibleTxt(1);
        }
    }

    /// <summary>
    /// Avisa a la UI que tome el coleccionable y lo seteo
    /// </summary>
    public void UICollectibleTaken()
    {
        _image.color = _activeColor;

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
        float elpasedTime = 0;

        var actualPos = _rectTransform.anchoredPosition;

        while (elpasedTime < _speed)
        {
            elpasedTime += Time.deltaTime;

            float t = elpasedTime / _speed;

            if(active) _rectTransform.anchoredPosition = Vector2.Lerp(actualPos, _showPos, t);

            else _rectTransform.anchoredPosition = Vector2.Lerp(actualPos, _hidePos, t);

            yield return null;
        }

        if(active)
        {
            yield return new WaitForSeconds(_timeShow);

            StartCoroutine(Show(false));
        }

        else _show = false;
    }

}
