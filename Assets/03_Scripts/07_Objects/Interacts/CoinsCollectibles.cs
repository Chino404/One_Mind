using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectibles : MonoBehaviour
{
    [SerializeField] private CharacterTarget _targetCharacter;
    private UICoins points;
    private LevelData myCurrentLevel;

    private void Awake()
    {
        //Sumo el total de monedas que hay en el nivel
        GameManager.instance.totalCoinsInLevel++;

        myCurrentLevel = GameManager.instance.currentLevel;

        //Si no estoy guardado en el Dict
        if (!myCurrentLevel.dictCoinsJSON.ContainsKey(gameObject.name)) myCurrentLevel.dictCoinsJSON.Add(gameObject.name, false); //Me agrego al diccionario

        else
        {
            //Si ya agarre esta moneda, la apago
            if (myCurrentLevel.dictCoinsJSON[gameObject.name]) gameObject.SetActive(false);
        }

        if (_targetCharacter == CharacterTarget.Bongo)
        {
            GameManager.instance.totalCoinsBongoSide++;
        }

        else
        {
            GameManager.instance.totalCoinsFrankSide++;
        }
    }

    private void Start()
    {
        if (_targetCharacter == CharacterTarget.Bongo) points = GameManager.instance.uiCoinBongo;

        else points = GameManager.instance.uiCoinFrank;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");

            //Sumo un punto a la UI
            points.AddPoints(1);

            myCurrentLevel.TakeMoneyLevelData(gameObject.name);

            if(_targetCharacter == CharacterTarget.Bongo) myCurrentLevel.currentCoinsBongoSide ++;

            else myCurrentLevel.currentCoinsFrankSide ++;

            Destroy(gameObject);
        }

    }
}
