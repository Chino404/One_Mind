using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICoins : MonoBehaviour
{
    [SerializeField] private CharacterTarget _targetCharacter;
    private int _points;
    public int totalPointInThisSide;
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        
        if(_targetCharacter == CharacterTarget.Bongo) GameManager.instance.uiCoinBongo = this;

        else GameManager.instance.uiCoinFrank = this;
    }

    private void Start()
    {
        if (_targetCharacter == CharacterTarget.Bongo)
        {           
            totalPointInThisSide = GameManager.instance.totalCoinsBongoSide;

            AddPoints(GameManager.instance.currentLevel.currentCoinsBongoSide);

        }
        else
        {         
            totalPointInThisSide = GameManager.instance.totalCoinsFrankSide;


            AddPoints(GameManager.instance.currentLevel.currentCoinsFrankSide);

        }
    }

    public void AddPoints(int newPoints)
    {       
        _points += newPoints;

        _textMesh.text = _points.ToString();

        _textMesh.text = $"{_points} / {totalPointInThisSide}";
    }
}
