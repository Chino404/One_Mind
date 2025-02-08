using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRotatingPlataform : Rewind
{
    public GameObject rotatingPlataform;

    public void Start()
    {
        rotatingPlataform.SetActive(false);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
            rotatingPlataform.SetActive(true);
    }

    public override void Save()
    {
        Debug.Log("guardo la rotacion de la plataforma");
        _currentState.Rec(rotatingPlataform.activeInHierarchy,rotatingPlataform.GetComponent<SpinButton>()._pivotPlatforms.rotation);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;
        var col = _currentState.Remember();
        rotatingPlataform.SetActive((bool)col.parameters[0]);
        rotatingPlataform.GetComponent<SpinButton>()._pivotPlatforms.rotation = (Quaternion)col.parameters[1];
    }
}
