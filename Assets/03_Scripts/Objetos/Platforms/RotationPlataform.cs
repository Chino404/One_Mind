using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlataform : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    collision.transform.SetParent(transform);
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    collision.transform.SetParent(null);
    //}
}
