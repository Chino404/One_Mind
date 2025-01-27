using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectibles : MonoBehaviour
{
    [SerializeField] private CoinsUI points;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");
            points.AddPoints(1);

            var level = CallJson.instance.refJasonSave.GetSaveData.levels;
            level[0].totalCoin ++;

            Destroy(gameObject);
        }

    }
}
