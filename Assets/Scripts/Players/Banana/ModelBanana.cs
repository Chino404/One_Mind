using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBanana : Characters
{
    private ViewBanana _view;
    private ControllerBanana _controller;

    private void Awake()
    {
        GameManager.instance.possibleCharacters[1] = this;

        _animatorCharacter = GetComponentInChildren<Animator>();

        _rbCharacter = GetComponent<Rigidbody>();
        _rbCharacter.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //De esta manera para que se freezeen los dos
        _rbCharacter.angularDrag = 1f; //Friccion de la rotacion


        _view = new ViewBanana(_animatorCharacter);
        _controller = new ControllerBanana(this);
    }
    void Start()
    {
        

    }

    void Update()
    {
        
    }
}
