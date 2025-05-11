using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummieAnimation : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            int random = Random.Range(0, 2);
            //Debug.Log(random);
            if (random == 0) _animator.SetTrigger("Hit1");
            if (random == 1) _animator.SetTrigger("Hit2");
        }
    }
}
