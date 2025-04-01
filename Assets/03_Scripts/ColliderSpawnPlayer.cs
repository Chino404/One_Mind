using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawnPlayer : MonoBehaviour
{
    public Transform spawnPoint;
    private bool _isPlayerInzone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            if (!_isPlayerInzone)
            {
                _isPlayerInzone = true;
                other.transform.position = spawnPoint.position;
            }
        }
    }
}
