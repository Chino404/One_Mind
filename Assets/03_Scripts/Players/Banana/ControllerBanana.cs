using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBanana
{
    private float _xAxis, _zAxis, _inputMouseX, _inputMouseY;

    private ModelBanana _model;

    [SerializeField] int _bulletQuantity;

   


    public ControllerBanana(ModelBanana model)
    {
        _model = model;
    }

    

    public void ArtificialUpdate()
    {
        _inputMouseX = Input.GetAxisRaw("Mouse X");
        _inputMouseY = Input.GetAxisRaw("Mouse Y");

        if(_inputMouseX != 0 || _inputMouseY != 0) _model.Rotation(_inputMouseX, _inputMouseY);

        if (Input.GetKeyDown(KeyCode.C)) GameManager.instance.Swap();

    }

    public void ListenFixedKeys()
    {
        _xAxis = Input.GetAxisRaw("Horizontal");
        _zAxis = Input.GetAxisRaw("Vertical");

        if(_xAxis != 0 || _zAxis != 0)
            _model.Movement(_xAxis, _zAxis);


        if(Input.GetKey(KeyCode.Space)) _model.FlyingUp();
        else if(Input.GetKey(KeyCode.LeftControl)) _model.FlyingDown();
        else _model.StopFly();
    }
}
