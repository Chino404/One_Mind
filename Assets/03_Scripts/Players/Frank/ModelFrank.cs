using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
[RequireComponent(typeof(Rigidbody))]
public class ModelFrank : Characters
{
    //Referencias
    private ControllerFrank _controller;
    private ViewFrank _view;
    public static ModelFrank instance;

    public override void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        base.Awake();

        GameManager.instance.frank = this;

        _view = new ViewFrank(_animPlayer);
        _controller = new ControllerFrank(this, _view);

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

        //if (GameManager.instance.bongo.stopMove) return;
        _controller.ArtificialUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _controller.ListenFixedKeys();
    }
}
