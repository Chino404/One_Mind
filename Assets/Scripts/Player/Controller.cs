using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    private Model _model;
    private Vector3 _dirRaw = new Vector3();
    private Vector3 _dir = new Vector3();

    private View _view;

    public Controller(Model model, View view)
    {
        _model = model;
        _view = view;
    }

    public void ArtificialUpdate()
    {
        if (Input.GetButtonDown("Jump")) _model.Jump();

        if (Input.GetButtonUp("Jump")) _model.CutJump();


        if (Input.GetButtonDown("Fire1")) _model.Punch();
    }

    public void ListenFixedKeys()
    {
        _dirRaw.x = Input.GetAxisRaw("Horizontal");
        _dirRaw.z = Input.GetAxisRaw("Vertical");

        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxisRaw("Vertical");

        _model.Movement(_dirRaw, _dir);
    }
}
