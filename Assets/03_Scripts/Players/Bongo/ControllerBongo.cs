using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBongo
{
    private ModelBongo _model;
    private ViewBongo _viewBongo;
    private Vector3 _dirRaw = new Vector3();
    private Vector3 _dir = new Vector3();

    public ControllerBongo (ModelBongo model, ViewBongo viewBongo)
    {
        _model = model;
        _viewBongo = viewBongo;
    }

    public void ArtificialUpdate()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift) && _model.actualStatePlayer == EstadoDePlayer.Escalando)
        {
            _model.ActualMove = _model.NormalMovement;
        }

        if (Input.GetButtonDown("Jump")) _model.Jump();

        if (Input.GetMouseButtonDown(0)) _model.Attack();

        _dirRaw.x = Input.GetAxisRaw("Horizontal");
        _dirRaw.z = Input.GetAxisRaw("Vertical");

        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxis("Vertical");
        

        //if (!GameManager.instance.frank.stopMove)
        //{
        //    _dirRaw.x = Input.GetAxisRaw("Horizontal");
        //    _dirRaw.z = Input.GetAxisRaw("Vertical");

        //    _dir.x = Input.GetAxis("Horizontal");
        //    _dir.z = Input.GetAxis("Vertical");
        //}
        //else Debug.Log("BONGO NO entro a CONTROLLER");
    }

    public void ListenFixedKeys()
    {
        if (GameManager.instance.frank.stopMove /*|| _model.stopMove*/)
        {
            _viewBongo.Walking(false);
            return;
        }

        if(_dirRaw.sqrMagnitude != 0)
        {
            _viewBongo.Walking(true);
            _model.Movement(_dirRaw, _dir);
        }
        else _viewBongo.Walking(false);
    }
}
