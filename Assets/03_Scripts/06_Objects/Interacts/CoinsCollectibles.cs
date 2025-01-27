using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectibles : MonoBehaviour
{
    [SerializeField] private CoinsUI points;
    private LevelData currentLevel;

    private void Awake()
    {
        currentLevel = CallJson.instance.refJasonSave.GetCurrentLevel(GameManager.instance.IndexLevel);

        if (!currentLevel.coinsJSON.ContainsKey(gameObject.name)) currentLevel.coinsJSON.Add(gameObject.name, false); //Me agrego al diccionario
        else
        {
            //Si ya agarre esta moneda, la apago
            if (currentLevel.coinsJSON[gameObject.name]) gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");
            points.AddPoints(1);

            currentLevel.coinsJSON[gameObject.name] = true;

            currentLevel.totalCoin ++;

            Destroy(gameObject);
        }

    }
}
