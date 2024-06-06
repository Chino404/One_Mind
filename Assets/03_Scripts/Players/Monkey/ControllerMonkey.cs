using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMonkey
{
    private ModelMonkey _model;
    private Vector3 _dirRaw = new Vector3();
    private Vector3 _dir = new Vector3();



    public ControllerMonkey(ModelMonkey model)
    {
        _model = model;
    }

    public void ArtificialUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift)) _model._holdPower = true;
        else _model._holdPower = false;

        if (Input.GetButtonDown("Jump")) _model.Jump();

        else if (Input.GetButtonUp("Jump")) _model.CutJump();

        if (Input.GetMouseButtonDown(0)) _model.Attack();

        if(Input.GetMouseButton(1)) _model.SpinAttack();



        //if (Input.GetButton("Fire1")) _model.GoToUpAttack();
        //else
        //{
        //    _model.TimePressed = 0;
        //    _model.chargeGetUp = false;
        //}


        //if (Input.GetButton("Fire2")) _model.Grab();
        //else if (Input.GetButtonUp("Fire2")) _model.StopGrab();

        if (Input.GetKeyDown(KeyCode.C)) GameManager.instance.Swap();
    }

    public void ListenFixedKeys()
    {
        _dirRaw.x = Input.GetAxisRaw("Horizontal");
        _dirRaw.z = Input.GetAxisRaw("Vertical");

        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxis("Vertical");

        _model.Movement(_dirRaw, _dir);
    }
}
