using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationCameras : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //GameManager.instance.AnimSeparationCameras = _animator;
    }
}
