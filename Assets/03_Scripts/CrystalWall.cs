using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalWall : MonoBehaviour
{
    [SerializeField] GameObject _crystalWall; 
    [SerializeField] private WayPoints _point;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
            _crystalWall.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.instance.enemies.Count < 1)
        {
            _crystalWall.SetActive(false);
            _point.enemies = false;
        }

    }
}
