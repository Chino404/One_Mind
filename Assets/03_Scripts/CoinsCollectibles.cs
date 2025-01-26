using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollectibles : MonoBehaviour
{
    [SerializeField] private CoinsPoints points;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            Debug.Log("agarre coleccionable");
            points.AddPoints(1);
            Destroy(gameObject);
        }

    }
}
