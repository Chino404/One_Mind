using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBongo
{
    private ModelBongo _model;
    private ViewBongo _viewBongo;
    private Vector3 _dirRaw = new Vector3();
    private Vector3 _dir = new Vector3();

    private bool _isFly;

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

        #region Jump
        if (Input.GetButtonDown("Jump")) _model.Jump();

        if (Input.GetButton("Jump")) _isFly = true;
        else _isFly = false;
        #endregion

        #region Move

        _dirRaw.x = Input.GetAxisRaw("Horizontal");
        _dirRaw.z = Input.GetAxisRaw("Vertical");

        if (_model.isMovementInverse == true) _dir.x = -Input.GetAxis("Horizontal");
        else _dir.x = Input.GetAxis("Horizontal");

        _dir.z = Input.GetAxis("Vertical");

        #endregion

        _model.valueScroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(0))
            _model.Attack();
       
    }

    public void ListenFixedKeys()
    {

        if (_dirRaw.sqrMagnitude != 0)
        {
            _viewBongo.Walking(true);
            //_model.Movement(_dirRaw);
            _model.Rotate(_dir.normalized);
        }
        else
        {
            _viewBongo.Walking(false);
        }

        _model.Movement(_dirRaw);

        _model.FlyPenguin(_isFly);
    }
}
