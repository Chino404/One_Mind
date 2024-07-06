using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colectionable : MonoBehaviour
{
    [SerializeField] private GemPoints points;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==3)
        {
            Debug.Log("agarre coleccionable");
            points.AddPoints(1);
            Destroy(gameObject);
        }

    }
}
