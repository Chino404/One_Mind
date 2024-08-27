using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerFrank
{
    private ModelFrank _model;
    private ViewFrank _viewFrank;
    private Vector3 _dirRaw = new Vector3();
    private Vector3 _dir = new Vector3();

    public ControllerFrank(ModelFrank model, ViewFrank viewFrank)
    {
        _model = model;
        _viewFrank = viewFrank;
    }

    public void ArtificialUpdate()
    {
        if (Time.timeScale == 0) return;
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            _model.ActualMove = _model.NormalMovement;
        }

        if (Input.GetButtonDown("Jump")) _model.Jump();

        if (Input.GetMouseButtonDown(1)) _model.Attack();

        _dirRaw.x = Input.GetAxisRaw("Horizontal");
        _dirRaw.z = Input.GetAxisRaw("Vertical");

        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxis("Vertical");

        //if(!GameManager.instance.bongo.stopMove)
        //{
        //    _dirRaw.x = Input.GetAxisRaw("Horizontal");
        //    _dirRaw.z = Input.GetAxisRaw("Vertical");

        //    _dir.x = Input.GetAxis("Horizontal");
        //    _dir.z = Input.GetAxis("Vertical");
        //}
        //else Debug.Log("FRANK NO entro a CONTROLLER");
    }

    public void ListenFixedKeys()
    {
        if (GameManager.instance.bongo.stopMove /*|| _model.stopMove*/)
        {
            _viewFrank.Walking(false);
            return;
        }

        if(_dirRaw.sqrMagnitude != 0)
        {
            _viewFrank.Walking(true);
            _model.Movement(_dirRaw, _dir);
        }
        else _viewFrank.Walking(false);
    }
}
