using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse : MonoBehaviour
{
    [SerializeField] ForceMode _impulseMode;
    [Range(20, 70)]
    [SerializeField] private int _force = 40;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ModelMonkey>())
        {
            var rb = other.GetComponent<Rigidbody>();

            _animator.SetTrigger("Interact");

            AudioManager.instance.PlaySFX(AudioManager.instance.mushroom);
            
            rb.velocity = transform.up * _force;
        }
    }
}
