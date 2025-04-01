using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectibles : MonoBehaviour
{
    [SerializeField] private CharacterTarget _targetCharacter;
    private UICoins uiPoints;
    private LevelData myCurrentLevel;

    private void Awake()
    {
        //Sumo el total de monedas que hay en el nivel
        GameManager.instance.totalCoinsInLevel++;

        myCurrentLevel = GameManager.instance.currentLevel;

        //Si no estoy guardado en el Dict
        if (!myCurrentLevel.dictCoinsJSON.ContainsKey(gameObject.name)) myCurrentLevel.dictCoinsJSON.Add(gameObject.name, false); //Me agrego al diccionario

        //Si ya agarre esta moneda o estoy jugando en modo cronometro, la apago
        else if (myCurrentLevel.dictCoinsJSON[gameObject.name] || GameManager.instance.isChronometerActive) gameObject.SetActive(false);

        if (_targetCharacter == CharacterTarget.Bongo) GameManager.instance.totalCoinsBongoSide++;

        else GameManager.instance.totalCoinsFrankSide++;
    }

    private void Start()
    {
        if (_targetCharacter == CharacterTarget.Bongo) uiPoints = GameManager.instance.uiCoinBongo;

        else uiPoints = GameManager.instance.uiCoinFrank;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");

            //Sumo un punto a la UI
            uiPoints.AddPoints(1);

            //myCurrentLevel.TakeMoneyLevelData(gameObject.name);
            GameManager.instance.coinsNameList.Add(gameObject.name);


            if (_targetCharacter == CharacterTarget.Bongo)
            {
                GameManager.instance.currentCollectedCoinsBongo++;
                //myCurrentLevel.currentCoinsBongoSide++;
            }

            else
            {
                GameManager.instance.currentCollectedCoinsFrank++;
                //myCurrentLevel.currentCoinsFrankSide++;
            }

            Destroy(gameObject);
        }

    }
}
