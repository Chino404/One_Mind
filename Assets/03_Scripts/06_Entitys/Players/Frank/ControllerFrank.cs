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
        if (_model.IsDead) return;
        if (Time.timeScale == 0) return;
        //if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    _model.ActualMove = _model.NormalMovement;
        //}

        if (Input.GetKeyDown(KeyCode.Tab)) GameManager.instance.UICollFrank.ShowUI();

        #region Jump

        if (Input.GetButtonDown("Jump")) _model.Jump();

        #endregion

        #region Movimiento

        _dirRaw.x = Input.GetAxisRaw("Horizontal");
        _dirRaw.z = Input.GetAxisRaw("Vertical");

        if (_model.isMovementInverse == true)
            _dir.x = -Input.GetAxis("Horizontal");
        else
            _dir.x = Input.GetAxis("Horizontal");

        _dir.z = Input.GetAxis("Vertical");

        #endregion

        if (Input.GetMouseButton(1)) _model.Attack();
    }

    public void ListenFixedKeys()
    {
        if (_model.IsDead) return;
        if (_dirRaw.sqrMagnitude != 0)
        {
            _viewFrank.Walking(true);
            //_model.Movement(_dirRaw);
            _model.Rotate(_dir.normalized);
        }
        else _viewFrank.Walking(false);

        _model.Movement(_dirRaw);
    }
}
