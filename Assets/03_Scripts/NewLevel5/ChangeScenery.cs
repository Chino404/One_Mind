using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenery : MonoBehaviour
{
    [SerializeField] private GameObject[] _scenery1;
    [SerializeField] private GameObject[] _scenery2;

    public KeyCode keyForChangeScenery=KeyCode.Q;


    private GameObject[] _activeScenery;
    private GameObject[] _desactiveScenery;
    private bool _isInZone;

    private void Start()
    {
        _activeScenery = _scenery1;
        _desactiveScenery = _scenery2;

        foreach (var item in _scenery1)
        {
            item.SetActive(true);
        }

        foreach (var item in _scenery2)
        {
            item.SetActive(false);
        }

        
    }

    private void Update()
    {
        if (GameManager.instance.modelBongo.isDead || GameManager.instance.modelFrank.isDead) return;
        if (!_isInZone) return;

        if (Input.GetKeyDown(keyForChangeScenery))
        {
            Change(ref _activeScenery, ref _desactiveScenery);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>()) _isInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Characters>()) _isInZone = false;
    }

    void Change(ref GameObject[]sceneryToDeactive,ref GameObject[]sceneryToActive)
    {
        var activeScenery = sceneryToDeactive;
        var desactiveScenery = sceneryToActive;

        foreach (var item in sceneryToDeactive)
        {
            item.SetActive(false);
        }
        
        foreach (var item in sceneryToActive)
        {
            item.SetActive(true);
        }

        sceneryToDeactive = desactiveScenery;
        sceneryToActive = activeScenery;
    }
}
