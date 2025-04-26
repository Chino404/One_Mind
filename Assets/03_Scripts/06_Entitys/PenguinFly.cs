using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinFly : MonoBehaviour, IInteracteable,IDamageable
{
    private Vector3 _newPosition;
    [SerializeField] private Vector3 _positionInBongo;

    [SerializeField]private bool _isInBongo;
    private bool _isDisable;
    [SerializeField]private Animator _animator;
    [Header("SIN BONGO")]
    public Transform[] waypoints;
    public float speed;
    public float rotationSpeed;
    [SerializeField] private float _secondsWaiting=1.5f;
    private int _actualIndex;
    private Vector3 _velocity;
    private Collider _boxCollider;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (_isInBongo && !_isDisable)
        {
            transform.position = GameManager.instance.modelBongo.gameObject.transform.position + _positionInBongo;
            transform.forward = GameManager.instance.modelBongo.gameObject.transform.forward;
        }

        if (_isDisable)
        {
            GameManager.instance.modelBongo.FlyPenguin(false);
            GameManager.instance.modelBongo.IsGetPenguin = false;
            gameObject.SetActive(false);
            GameManager.instance.modelBongo.penguin = null;
        }

        else if (!_isInBongo)
        {
            if (Vector3.Distance(transform.position, waypoints[_actualIndex].position) <= 0.5f)
            {
                StartCoroutine(StopWalk());
                _actualIndex++;
                if (_actualIndex >= waypoints.Length) _actualIndex = 0;
            }
            _velocity = waypoints[_actualIndex].position - transform.position;
            _velocity.Normalize();
            transform.position += _velocity * speed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(_velocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            _animator.SetTrigger("Walking");

        }
    }

    IEnumerator StopWalk()
    {
        var actualSpeed = speed;
        speed = 0;
        _animator.SetTrigger("Normal");
        yield return new WaitForSeconds(_secondsWaiting);
        speed = actualSpeed;
        

    }

    public void TakePenguin()
    {
        transform.position = GameManager.instance.modelBongo.gameObject.transform.position + _positionInBongo;
        transform.forward = GameManager.instance.modelBongo.gameObject.transform.forward;

        _isInBongo = true;
    }

    public void Active()
    {
        if (_isDisable) return;

        _boxCollider.enabled = false;
        GameManager.instance.modelBongo.penguin = this;
        GameManager.instance.modelBongo.IsGetPenguin = true;
        _animator.SetTrigger("Normal");
    }

    public void Deactive()
    {
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 16) _isDisable = true;
    }

    public void Fly()
    {
        _animator.SetTrigger("Flying");
    }

    public void StopFlying()
    {
        _animator.SetTrigger("Normal");
    }

    public void TakeDamageEntity(float dmg, Vector3 target)
    {
        
    }

    public void Dead()
    {
        GameManager.instance.modelBongo.Dead();
    }

    public void DeadByWater()
    {
        
    }

    public void DeadByShoot()
    {
        
    }
}
