using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Characters target;

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeed;

    
    Vector3 _offset, _desiredPos, _smoothPos;
  
    private void Start()
    {
        _offset = transform.position;
    }

    

    private void FixedUpdate()
    {
        target = GameManager.instance.actualCharacter;

        _desiredPos = target.transform.position + _offset;
        _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeed);
        transform.position = _smoothPos;
    }

  
}
