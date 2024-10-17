using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse : MonoBehaviour
{
    [SerializeField] ForceMode _impulseMode;
    [Range(20, 150)]
    [SerializeField] private int _force = 40;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Characters>())
        {
            var player = other.gameObject.GetComponent<Characters>();
            var rb = other.GetComponent<Rigidbody>();

            _animator.SetTrigger("Interact");

            //OldAudioManager.instance.PlaySFX(OldAudioManager.instance.mushroom);

            rb.velocity = transform.up * _force;


            //rb.AddForce(transform.up * _force, _impulseMode);

            //rb.velocity = new Vector3(rb.velocity.x, _force, rb.velocity.z); 
            
        }
    }
}
