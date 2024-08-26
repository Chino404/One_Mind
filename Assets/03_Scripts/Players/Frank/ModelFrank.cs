using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ModelFrank : Characters
{
    //Referencias
    private ControllerFrank _controller;
    private ViewFrank _view;

    public override void Awake()
    {
        base.Awake();

        GameManager.instance.frank = transform;

        _view = new ViewFrank(_animPlayer);
        _controller = new ControllerFrank(this);

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
