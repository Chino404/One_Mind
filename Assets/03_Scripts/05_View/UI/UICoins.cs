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

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        
        if(_targetCharacter == CharacterTarget.Bongo && GameManager.instance.uiCoinBongo == null) GameManager.instance.uiCoinBongo = this;

        else if(GameManager.instance.uiCoinFrank == null) GameManager.instance.uiCoinFrank = this;
    }

    private void OnEnable()
    {
        if (!isInPause) return;

        if (_targetCharacter == CharacterTarget.Bongo)
        {
            if(totalPointInThisSide == default) totalPointInThisSide = GameManager.instance.totalCoinsBongoSide;

            _textMesh.text = $"{GameManager.instance.currentCollectedCoinsBongo} / {totalPointInThisSide}";
        }

        else
        {
            if(totalPointInThisSide == default ) totalPointInThisSide = GameManager.instance.totalCoinsFrankSide;

            _textMesh.text = $"{GameManager.instance.currentCollectedCoinsFrank} / {totalPointInThisSide}";
        }
    }

    private void Start()
    {
        if (isInPause) return;

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
}
