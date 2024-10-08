using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField, Tooltip("La referencia del Checkpoint al que va a estar vinculado")] private CheckPoint _refOtherCheckPoint;

    private Transform _spawnPoint;
    public Transform SpawnPoint { get { return _spawnPoint; } }

    private Characters _playerRef;

    private bool _inPosition;
    public bool InPosition { get { return _inPosition; } }
    
    private void Awake()
    {
        _spawnPoint = transform.GetChild(0);

        if (!_refOtherCheckPoint) Debug.LogError($"Falra la referencia del otro checkpoint en: {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Characters>())
        {
            _playerRef = other.GetComponent<Characters>();

            _inPosition = true;

            if(_refOtherCheckPoint.InPosition)
            {
                SaveCheckpoint();
                _refOtherCheckPoint.SaveCheckpoint();
            }

            //other.GetComponent<Characters>().actualCheckpoint = this;

            //foreach (var item in GameManager.instance.rewinds)
            //{
            //    item.Save();
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Characters>())
        {
            _inPosition = false;
        }
    }

    public void SaveCheckpoint()
    {
        _playerRef.actualCheckpoint = this;

        foreach (var item in GameManager.instance.rewinds)
        {
            item.Save();
        }
    }
}
