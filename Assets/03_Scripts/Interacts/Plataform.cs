using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour, IInteractable
{
    //[SerializeField] float _secondsWaiting=2f;
    //[SerializeField] Transform[] _waypoints;
    //[SerializeField] float _maxVelocity;

    
    //[SerializeField]private int _actualIndex;
    //[SerializeField]private float _maxForce=0.01f;
    //[SerializeField]private Vector3 _velocity;


    //private Rigidbody _rb;

    private bool _isObjectAttached;
    [SerializeField] private float _minLimit = -5f;
    [SerializeField] private float _maxLimit = 5f;
    public enum Axis { X,Y,Z}
    public Axis movementAxis = Axis.Z;

    

    
    void Update()
    {
        if (_isObjectAttached)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //switch (movementAxis)
            //{
            //    case Axis.X:
            //        transform.rotation = Quaternion.Euler(transform.rotation.x, 0, 0);
            //            break;
            //    case Axis.Y:
            //        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            //        break;
            //    case Axis.Z:
            //        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
            //        break;
            //}
            //Vector3 localPosition = transform.localPosition;

            //    switch (movementAxis)
            //    {
            //        case Axis.X:
            //            Debug.Log("me muevo en eje X");
            //            localPosition.x = Mathf.Clamp(localPosition.x, _minLimit, _maxLimit);
            //            localPosition.y = transform.localPosition.y;
            //            localPosition.z = transform.localPosition.z;
            //            break;
            //        case Axis.Y:
            //            Debug.Log("me muevo en eje Y");
            //            localPosition.y = Mathf.Clamp(localPosition.y, _minLimit, _maxLimit);
            //            localPosition.x = transform.localPosition.x;
            //            localPosition.z = transform.localPosition.z;
            //            break;
            //        case Axis.Z:
            //            localPosition.x = transform.localPosition.x;
            //            localPosition.y = transform.localPosition.y;
            //            localPosition.z = Mathf.Clamp(localPosition.z, _minLimit, _maxLimit);
            //            Debug.Log("me muevo en eje Z");
            //            break;

            //    }
            //    transform.localPosition = localPosition;
        }
    }

    //private void Awake()
    //{
    //    _rb = GetComponent<Rigidbody>();
    //}

    //private void FixedUpdate()
    //{
    //    //AddForce(Seek(_waypoints[_actualIndex].position));
  
    //    if (Vector3.Distance(transform.position, _waypoints[_actualIndex].position)<=0.4f)
    //    {
    //        StartCoroutine(WaitSeconds());
    //        _actualIndex++;
    //        if (_actualIndex >= _waypoints.Length)
    //        {

    //            _actualIndex = 0;
    //        }
            
    //    }
    //    _velocity = _waypoints[_actualIndex].position - transform.position;
    //    _velocity.Normalize();
    //    //transform.position += _velocity * Time.fixedDeltaTime;
    //    //_rb.MovePosition(transform.position + _velocity*_maxVelocity * Time.fixedDeltaTime);
    //}

    

    //IEnumerator WaitSeconds()
    //{
    //    var velocity = _maxVelocity;
    //    _maxVelocity = 0;
    //    yield return new WaitForSeconds(_secondsWaiting);
    //    _maxVelocity = velocity;

    //}

    //Vector3 Seek(Vector3 target)
    //{
    //    var desired = target - transform.position;
    //    desired.Normalize();
    //    desired *= _maxVelocity;
    //    return CalculateSteering(desired);
    //}

    //Vector3 CalculateSteering(Vector3 desired)
    //{
    //    var steering = desired - _velocity;
    //    steering = Vector3.ClampMagnitude(steering, _maxForce);
    //    return steering;
    //}

    //void AddForce(Vector3 dir)
    //{
    //    _velocity += dir;

    //    _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<ModelMonkey>())
        collision.transform.SetParent(transform);
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
            transform.SetParent(parent);
            _isObjectAttached = true;
        }
        else if (_isObjectAttached)
        {
            transform.SetParent(null);
            _isObjectAttached = false;
        }
    }

    
}
