using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawnPlayer : MonoBehaviour
{
    public Transform spawnPoint;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            other.transform.position = spawnPoint.position;
        }
    }
}
