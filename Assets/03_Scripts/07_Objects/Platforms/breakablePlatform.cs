using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum BreakingState
//{
//    Normal,
//    Subido,
//    Advertencia,
//    Roto,
//    Recomponerse
//}

[RequireComponent(typeof(BoxCollider))]
public class BreakablePlatform : Rewind
{
    //[SerializeField]private BreakingState _myState = BreakingState.Normal;
    [SerializeField] private string _currentTrigger;

    [Space(10),SerializeField, Tooltip("Tiempo para la animacion de ADVERTENCIA"), Range(0, 5f)] private float _timeToWarning = 1f;
    //[SerializeField, Tooltip("Tiempo para la animacion de ROMPERSE"), Range(0,5f)] private float _timeToBreaking = 0.5f;
    [SerializeField, Tooltip("Tiempo que tarda para RECOMPONERSE"), Range(0,5f)] private float _timeToRecover = 2;

    //private Collider _myCollider;
    private Animator _myAnimator;

    //Provisorio hasta tener animaciones
    //[Space(10),SerializeField] private GameObject _ice;
    [Header("BREAK ANIMATION")]
    [SerializeField] private Transform _waypoint;
    private Vector3 _startPos;
    [SerializeField] private float _speed;
    private Vector3 _velocity;
    private bool _isBreaking;
    private bool _isRecovering;


    public override void Awake()
    {
        base.Awake();
        //_myCollider = GetComponent<Collider>();
        _myAnimator = GetComponentInChildren<Animator>();
        _startPos = transform.position;

    }

    private void OnEnable()
    {
        _isBreaking = false;
        transform.position = _startPos;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
        {
            if (_isBreaking) return;
            //_isBreaking = true;

            //Onplatform();

            StartCoroutine(ActionBreaking());
        }
    }

    private void Update()
    {
        if (_isBreaking)
        {
            MoveTowardsWaypoint(_waypoint.position);
        }
        if (_isRecovering)
        {
            MoveTowardsWaypoint(_startPos);
        }
    }

    private void Onplatform()
    {
        

        //if (_myState == BreakingState.Subido) return;
        //_myState = BreakingState.Subido;

        _currentTrigger = "OnPlatform";
        _myAnimator.SetTrigger("OnPlatform");


    }

    public void WarningBreaking()
    {
        //if (_myState == BreakingState.Advertencia) return;

        //_myState = BreakingState.Advertencia;

        //StartCoroutine(TimeToNextAnimation(_timeToWarning, "Warning"));

        StartCoroutine(ActionBreaking());
    }

    IEnumerator ActionBreaking()
    {
        _myAnimator.SetTrigger("OnPlatform");

        _isRecovering = false;
        _myAnimator.SetTrigger("Warning");
        AudioManager.instance.Play(SoundId.OnlyActive);


        yield return new WaitForSeconds(_timeToWarning);
        _isBreaking = true;
        _myAnimator.SetTrigger("Idle");
        //_myAnimator.SetTrigger("Break");

        yield return new WaitForSeconds(_timeToRecover);
        _isBreaking = false;
        _isRecovering = true;
        //_myAnimator.SetTrigger("Recover");

    }

    void MoveTowardsWaypoint(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) >= 0.5f)
        {
            _velocity = target - transform.position;
            _velocity.Normalize();

            transform.position += _velocity * _speed * Time.deltaTime;
        }
    }

    public void Idle()
    {
        //if (_myState == BreakingState.Normal) return; 
        //_myState = BreakingState.Normal;

        _myAnimator.SetTrigger("Idle");

        _isBreaking = false;
    }

    IEnumerator TimeToNextAnimation(float time, string valuTrigger)
    {
        _myAnimator.SetBool(_currentTrigger, false);

        _currentTrigger = valuTrigger;

        yield return new WaitForSeconds(time);

        _myAnimator.SetTrigger(valuTrigger);
    }

    #region Memento

    public override void Save()
    {
        _currentState.Rec( _currentTrigger,_isBreaking);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;
        var col = _currentState.Remember();
        //gameObject.SetActive((bool)col.parameters[0]);

        //_myState = (BreakingState)col.parameters[0];
        //_currentTrigger = (string)col.parameters[1];
        _isBreaking = (bool)col.parameters[1];
        transform.position = _startPos;
        //_isRecovering = (bool)col.parameters[4];
        StopAllCoroutines();
        _myAnimator.SetTrigger("Idle");

        //_myAnimator.SetTrigger("Recover");
        //Idle();

    }

    #endregion
}
