using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour, IInteractable
{
    //[SerializeField] float _secondsWaiting=2f;
    //[SerializeField] Transform[] _waypoints;
    [SerializeField] float _maxVelocity;

    
    //[SerializeField]private int _actualIndex;
    //[SerializeField]private float _maxForce=0.01f;
    //[SerializeField]private Vector3 _velocity;


    private Rigidbody _rb;

    private bool _isObjectAttached;
    //[SerializeField] private float _minLimit = -5f;
    //[SerializeField] private float _maxLimit = 5f;
    public enum Axis { X,Y,Z}
    public Axis movementAxis = Axis.Z;

    [SerializeField] Transform banana;
    private Vector3 _startPos;

    public ModelBanana modelBanana;
    private Ray _moveRay;
    private float _moveRange=0.75f;
    [SerializeField]private LayerMask _moveMask;

    private Vector3 _velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        modelBanana = GameManager.instance.players[1].GetComponent<ModelBanana>();
        _startPos = transform.position;
    }
    
    void FixedUpdate()
    {
        if (IsBlocked(modelBanana.Velocity)) modelBanana.Velocity=Vector3.zero;
        if (_isObjectAttached && banana!=null)
        {
            
                
            //localPosition.x = _startPos.x;
            //localPosition.y = _startPos.y;
            //bananaPosition.x = _startPos.x;
            //bananaPosition.y = _startPos.y;
            //localPosition.z = bananaPosition.z;
            _rb.MovePosition(transform.position + modelBanana.Velocity * Time.fixedDeltaTime);
            
                    
            
        }
        
    }

    private bool IsBlocked(Vector3 dir)
    {
        _moveRay = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * _moveRange, Color.red);

        return Physics.Raycast(_moveRay, _moveRange, _moveMask);
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ModelMonkey>())
        {
            
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<ModelMonkey>())
            collision.transform.SetParent(null);
    }

    public void LeftClickAction()
    {
        
    }

    public void RightClickAction(Transform parent)
    {        
        if (!_isObjectAttached)
        {
            //transform.SetParent(parent);
            banana = parent;
            _isObjectAttached = true;
        }
        else if (_isObjectAttached)
        {
            ReleaseObject();
        }
    }

    public void ReleaseObject()
    {
        transform.SetParent(null);
        banana = null;
        _isObjectAttached = false;
    }
}
