using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
[RequireComponent(typeof(Rigidbody))]
public class ModelBongo : Characters
{
    //Referencias
    public static ModelBongo instance;
    private ControllerBongo _controller;
    private ViewBongo _view;

    [Space(15),Header("--- VALUE BONGO ---")]
    private Collider _myBoxCollider;

    public override void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        GameManager.instance.bongo = instance;
        base.Awake();

        _myBoxCollider = GetComponent<BoxCollider>();

        _view = new ViewBongo(this, _animPlayer);
        _controller = new ControllerBongo(this, _view);

        _currentState = new MementoState();
    }

    public override void Start()
    {
        base.Start();
        ActualMove = NormalMovement;
    }

    public override void Update()
    {
        base.Update();

        _controller.ArtificialUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _controller.ListenFixedKeys();
    }

    public override void Attack()
    {
        //base.Attack();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            Debug.Log("Fije algo");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            Debug.Log("Deje de fijar");
        }    
    }

}
