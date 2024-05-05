using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse : MonoBehaviour
{
    [SerializeField] ForceMode _impulseMode;
    [Range(20, 70)]
    [SerializeField] private int _force;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ModelMonkey>())
        {
            var rb = other.GetComponent<Rigidbody>();
            var tr = other.GetComponent<Transform>();

            rb.velocity = Vector3.up * _force;

        }
    }
}
