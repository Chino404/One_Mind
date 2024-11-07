using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BreakingState
{
    Normal,
    Subido,
    Advertencia,
    Roto,
    Recomponerse
}

[RequireComponent(typeof(BoxCollider))]
public class BreakablePlatform : Rewind
{
    [SerializeField]private BreakingState _myState = BreakingState.Normal;
    [SerializeField] private string _currentTrigger;
    [SerializeField]private bool _isBreaking;

    [Space(10),SerializeField, Tooltip("Tiempo para la animacion de ADVERTENCIA"), Range(0, 5f)] private float _timeToWarning = 1f;
    [SerializeField, Tooltip("Tiempo para la animacion de ROMPERSE"), Range(0,5f)] private float _timeToBreaking = 0.5f;
    [SerializeField, Tooltip("Tiempo que tarda para RECOMPONERSE"), Range(0,5f)] private float _timeToRecover = 2;

    private Collider _myCollider;
    private Animator _myAnimator;

    //Provisorio hasta tener animaciones
    [Space(10),SerializeField] private GameObject _ice;

    public override void Awake()
    {
        base.Awake();
        _myCollider = GetComponent<Collider>();
        _myAnimator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
        {
            if (_isBreaking) return;
            _isBreaking = true;

            Onplatform();
        }
    }

    private void Onplatform()
    {

        if (_myState == BreakingState.Subido) return;
        _myState = BreakingState.Subido;

        _currentTrigger = "OnPlatform";
        _myAnimator.SetTrigger("OnPlatform");


        AudioManager.instance.Play(SoundId.IceBreak);
    }

    public void WarningBreaking()
    {
        if (_myState == BreakingState.Advertencia) return;

        _myState = BreakingState.Advertencia;

        //StartCoroutine(TimeToNextAnimation(_timeToWarning, "Warning"));

        StartCoroutine(ActionBreaking());
    }

    IEnumerator ActionBreaking()
    {
        _myAnimator.SetTrigger("Warning");

        yield return new WaitForSeconds(_timeToWarning);

        _myAnimator.SetTrigger("Break");

        yield return new WaitForSeconds(_timeToRecover);

        _myAnimator.SetTrigger("Recover");

    }

    public void Idle()
    {
        if (_myState == BreakingState.Normal) return; 
        _myState = BreakingState.Normal;

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
        _currentState.Rec(_myCollider.enabled, _ice.activeInHierarchy);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;
        StopAllCoroutines();
        var col = _currentState.Remember();
        _myCollider.enabled = (bool)col.parameters[0];
        _ice.SetActive((bool)col.parameters[1]);

        //_myAnimator.SetTrigger("Idle");
        Idle();
    }

    #endregion
}
