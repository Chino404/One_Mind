using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morse : MonoBehaviour
{
    private Animator _myAnimator;

    private void Awake()
    {
        _myAnimator = GetComponentInChildren<Animator>();
    }
}
