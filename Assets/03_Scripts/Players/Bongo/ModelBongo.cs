using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
[RequireComponent(typeof(Rigidbody))]
public class ModelBongo : Characters
{
    //Referencias
    private ControllerBongo _controller;
    private ViewBongo _view;
    public static ModelBongo instance;

    public override void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        base.Awake();

        GameManager.instance.bongo = instance;

        _view = new ViewBongo(_animPlayer);
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
}
