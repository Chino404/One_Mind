using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalWall : MonoBehaviour
{
    [SerializeField] GameObject _crystalWall; 
    [SerializeField] private WayPoints _point;

    private void Start()
    {
        _crystalWall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
            _crystalWall.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.instance.enemies.Count < 1)
        {
            _point.action = false;
            _crystalWall.SetActive(false);
        }

    }
}
