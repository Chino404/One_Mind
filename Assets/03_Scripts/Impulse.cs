using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse : MonoBehaviour
{
    [SerializeField] ForceMode _impulseMode;
    [Range(20, 70)]
    [SerializeField] private int _force;
    private Animator _animator;
    [SerializeField] private Animation _clip;


    /*
     * int = numeros enteros
     * float = numeros con comas
     * string = Textos
     * 
     */
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ModelMonkey>())
        {
            var rb = other.GetComponent<Rigidbody>();

            //_animator.SetTrigger("Interact");
            //_animator.Play("Up");
            _clip.Play("Interact");
            
            rb.velocity = transform.up * _force;
        }
    }
}
