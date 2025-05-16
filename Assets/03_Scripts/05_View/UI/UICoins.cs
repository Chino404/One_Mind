using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICoins : MonoBehaviour
{
    [SerializeField] private CharacterTarget _targetCharacter;
    public int totalPointInThisSide;
    private int _points;
    private TextMeshProUGUI _textMesh;

    [Space(7)]
    [Tooltip("Solo para el menú de pausa")] public bool isInPause;
    [Tooltip("Solo para el Canvas Win")] public bool isInCanvasWin;

    private enum TypeView { InGame, InPuase, InWin}
    [SerializeField] private TypeView _type;

    private RectTransform _rectTransform;
    public Vector2 hidePos;
    public Vector2 showPos;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeShow;
    private bool _show;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _textMesh = GetComponent<TextMeshProUGUI>();
        
        if(_targetCharacter == CharacterTarget.Bongo && GameManager.instance.uiCoinBongo == null) GameManager.instance.uiCoinBongo = this;

        else if(GameManager.instance.uiCoinFrank == null) GameManager.instance.uiCoinFrank = this;

        //ShowUI();
        _rectTransform.anchoredPosition = hidePos;
    }

    private void OnEnable()
    {
        if (_type == TypeView.InGame) return;

        if (_targetCharacter == CharacterTarget.Bongo)
        {
            if(totalPointInThisSide == default) totalPointInThisSide = GameManager.instance.totalCoinsBongoSide;

            if (_type == TypeView.InWin)
            {
                _textMesh.text = $"0 / {totalPointInThisSide}";
                return;
            }

            _textMesh.text = $"{GameManager.instance.currentCollectedCoinsBongo} / {totalPointInThisSide}";
        }

        else
        {
            if(totalPointInThisSide == default ) totalPointInThisSide = GameManager.instance.totalCoinsFrankSide;

            if (_type == TypeView.InWin)
            {
                _textMesh.text = $"0 / {totalPointInThisSide}";
                return;
            }

            _textMesh.text = $"{GameManager.instance.currentCollectedCoinsFrank} / {totalPointInThisSide}";
        }
    }

    private void Start()
    {
        if (_type != TypeView.InGame) return;

        if (_targetCharacter == CharacterTarget.Bongo)
        {           
            totalPointInThisSide = GameManager.instance.totalCoinsBongoSide;

            AddPoints(GameManager.instance.currentLevel.coinsObtainedBongoSide);
        }

        else
        {         
            totalPointInThisSide = GameManager.instance.totalCoinsFrankSide;

            AddPoints(GameManager.instance.currentLevel.coinsObtainedFrankSide);
        }       
    }

    public void AddPoints(int newPoints)
    {       
        _points += newPoints;

        _textMesh.text = _points.ToString();

        _textMesh.text = $"{_points} / {totalPointInThisSide}";
    }

    public void ShowUI()
    {
        StopAllCoroutines();

        
        
            
            _show = true;

            StartCoroutine(Show(_show));
        
    }

    IEnumerator Show(bool active)
    {
        float elapsedTime = 0;

        var actualPos = _rectTransform.anchoredPosition;

        while (elapsedTime < _speed)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / _speed;

            if (active) _rectTransform.anchoredPosition = Vector2.Lerp(actualPos, showPos, t);

            else _rectTransform.anchoredPosition = Vector2.Lerp(actualPos, hidePos, t);

            yield return null;
        }

        if (active)
        {
            yield return new WaitForSeconds(_timeShow);

            StartCoroutine(Show(false));
        }

        else _show = false;
    }
}
