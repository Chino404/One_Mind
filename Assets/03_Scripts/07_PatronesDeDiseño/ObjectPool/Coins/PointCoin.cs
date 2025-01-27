using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCoin : MonoBehaviour
{
    public GameObject coin;

    private void Start()
    {
        var level = CallJson.instance.refJasonSave.GetSaveData.levels;

        if (!level[0].coinsJSON.ContainsKey(gameObject.name)) level[0].coinsJSON.Add(gameObject.name, false);
        else
        {
            if (level[0].coinsJSON[gameObject.name]) coin.gameObject.SetActive(false);
            //Pido al objectPool una moneda
        }
    }
}
