using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualDoor : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenTheDoor()
    {
        _animator.SetTrigger("Open");
    }
}
