using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBanana : Characters
{
    [Header("Camara")]
    [Range(1f, 1000f), SerializeField] private float _mouseSensivility = 100f;

    [Header("Componentes")]
    [SerializeField] private Transform _headTransform;

    [Header("Valores Perosnaje")]
    [SerializeField, Tooltip("Rango para evitar pegarme al objeto de _moveMask")] private float _moveRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    [SerializeField] private float _speed = 5f;
    [SerializeField] private LayerMask _moveMask; //Para indicar q layer quierp q no se acerque mucho
    [SerializeField] private float _speedUp = 2f;

    private float _mouseRotationX;

    [SerializeField]private FPCamera _camera;
    private Rigidbody _rb;

    private Ray _moveRay;

    //Constructores
    private ViewBanana _view;
    private ControllerBanana _controller;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation; //Me bloquea los 3 ejes a al vez
        _camera = GetComponentInChildren<FPCamera>();


        _view = new ViewBanana();
        _controller = new ControllerBanana(this);
    }

    private void Start()
    {
        GameManager.instance.possibleCharacters[1] = this;
        _camera.gameObject.SetActive(false);

    }

    private void Update()
    {
        _controller.ArtificialUpdate();
    }

    private void FixedUpdate()
    {

        _controller.ListenFixedKeys();
    }

    #region Movement
    public void Movement(float xAxis, float zAxis)
    {

        Debug.Log("Entro");
        var dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        if (IsBlocked(dir)) return;
        _rb.MovePosition(transform.position + dir.normalized * _speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Si el Ray choca con un objeto de la _moveMask, me lo indica con un TRUE
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private bool IsBlocked(Vector3 dir)
    {
        _moveRay = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * _moveRange, Color.red);

        return Physics.Raycast(_moveRay, _moveRange, _moveMask);
    }

    public void Rotation(float x, float y)
    {
        _mouseRotationX += x * _mouseSensivility * Time.deltaTime; 

        if(_mouseRotationX >= 360 || _mouseRotationX <= -360)
        {
            _mouseRotationX -= 360 * Mathf.Sign(_mouseRotationX); //Si excedo tal valor, me lo resta
        }

        y *= _mouseSensivility * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, _mouseRotationX, 0); //Para que me rote en base al eje y

        _camera?.RotationCamera(_mouseRotationX, y);
    }
    #endregion

    public void FlyingUp() =>_rb.velocity = Vector3.up * _speedUp * Time.fixedDeltaTime;

    public void StopFly() => _rb.velocity = Vector3.zero;

    public void FlyingDown() => _rb.velocity = Vector3.down * _speedUp * Time.fixedDeltaTime;
}
