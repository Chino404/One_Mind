using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBox : Rewind
{
    private Vector3 _position;
    public Transform desiredPosition;

    private void Start()
    {
        _position = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17)
            transform.position = _position;
        if (other.gameObject.layer == 19&&desiredPosition)
            transform.position = desiredPosition.position;
    }

    public override void Save()
    {
        _currentState.Rec(transform.position);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        transform.position = (Vector3)col.parameters[0];
    }

    
}
