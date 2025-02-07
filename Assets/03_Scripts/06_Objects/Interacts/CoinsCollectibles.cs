using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectibles : MonoBehaviour
{
    [SerializeField] private CoinsUI points;
    private LevelData myCurrentLevel;

    private void Awake()
    {
        GameManager.instance.totalCoinsInLevel++;

        myCurrentLevel = GameManager.instance.currentLevel;

        if (!myCurrentLevel.coinsJSON.ContainsKey(gameObject.name)) myCurrentLevel.coinsJSON.Add(gameObject.name, false); //Me agrego al diccionario
        else
        {
            //Si ya agarre esta moneda, la apago
            if (myCurrentLevel.coinsJSON[gameObject.name]) gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");
            points.AddPoints(1);

            myCurrentLevel.coinsJSON[gameObject.name] = true;

            myCurrentLevel.totalCoin ++;

            Destroy(gameObject);
        }

    }
}
