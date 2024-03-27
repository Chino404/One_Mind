using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Model : MonoBehaviour
{
    [Header("Values General")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _forceGravity;
    [SerializeField] private float _jumpForce;

    [Header("Coyote Time")]
    public float groundDistance = 2;
    [SerializeField, Range(0, 0.2f)] private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;

    [Header("Reference")]
    [SerializeField] private LayerMask _floorLayer;


    private Rigidbody _rb;
    private Controller _controller;
    private View _view;
    public Animator _animator;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //De esta manera para que se freezeen los dos
        _rb.angularDrag = 1f; //Friccion de la rotacion

        _view = new View(_animator);
        _controller = new Controller(this, _view);
    }

    private void Update()
    {
        if(IsGrounded()) _coyoteTimeCounter = _coyoteTime;
        else _coyoteTimeCounter -= Time.deltaTime;

        _controller.ArtificialUpdate();

    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.down * _forceGravity, ForceMode.Impulse);
        _controller.ListenFixedKeys();
    }

    public void Movement(Vector3 dirRaw, Vector3 dir)
    {
        if (dirRaw.sqrMagnitude != 0)
        {
            _rb.MovePosition(transform.position + dir.normalized * _moveSpeed * Time.fixedDeltaTime);
            Rotate(dir);
        }
    }

    public void Rotate(Vector3 dirForward)
    {
        transform.forward = dirForward;
    }

    #region JUMP
    public bool IsGrounded()
    {
        Vector3 pos = transform.position;
        Vector3 dir = Vector3.down;
        float dist = groundDistance;

        Debug.DrawLine(pos, pos + (dir * dist));

        return Physics.SphereCast(pos, 1, dir, out RaycastHit hit, dist, _floorLayer);
    }

    public void Jump()
    {
        if (_coyoteTimeCounter > 0f)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce);
        }
    }

    public void CutJump()
    {
        _coyoteTimeCounter = 0;
        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y * 0.5f);
    }
    #endregion
}
