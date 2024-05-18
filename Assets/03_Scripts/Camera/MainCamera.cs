using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Animator _animatorCamera;

    private void Awake()
    {
        _animatorCamera = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.instance.camerasPlayers[0] = gameObject;
    }
}
