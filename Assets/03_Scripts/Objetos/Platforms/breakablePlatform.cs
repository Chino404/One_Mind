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
    [SerializeField,Tooltip("Trigger actual activado")]private List<string> _listTirggers;
    [SerializeField] private string _currentTirgger;
    [SerializeField]private bool _isBreaking;

    [Space(10),SerializeField, Tooltip("Tiempo para la animacion de ADVERTENCIA"), Range(0, 3f)] private float _timeToWarning = 1f;
    [SerializeField, Tooltip("Tiempo para la animacion de ROMPERSE"), Range(0,1f)] private float _timeToBreaking = 0.5f;
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
            //if(!_isBreaking) StartCoroutine(Breaking());
            if (_isBreaking) return;
            _isBreaking = true;

            Onplatform();
        }
    }

    private void Onplatform()
    {
        //if (_isBreaking) return;

        if (_myState == BreakingState.Subido) return;
        _myState = BreakingState.Subido;

        _myAnimator.SetBool("NextState", true);
        _myAnimator.SetTrigger("OnPlatform");
        AudioManager.instance.Play(SoundId.IceBreak);
    }

    public void WarningBreaking()
    {
        if (_myState == BreakingState.Advertencia) return;

        _myState = BreakingState.Advertencia;

        StartCoroutine(TimeToNextAnimation(_timeToWarning, "Warning"));
    }

    public void Break()
    {
        if (_myState == BreakingState.Roto) return;
        _myState = BreakingState.Roto;

        StartCoroutine(TimeToNextAnimation(_timeToBreaking, "Break"));
    }

    public void Recover()
    {
        if (_myState == BreakingState.Recomponerse) return;
        _myState = BreakingState.Recomponerse;

        StartCoroutine(TimeToNextAnimation(_timeToRecover, "Recover"));
    }

    public void Idle()
    {
        if (_myState == BreakingState.Normal) return; 
        _myState = BreakingState.Normal;

        //_myAnimator.ResetTrigger(_currentTirgger);

        foreach (var trigger in _listTirggers)
        {
            _myAnimator.ResetTrigger(trigger);
        }

        _isBreaking = false;
        _myAnimator.SetTrigger("Idle");
    }

    IEnumerator TimeToNextAnimation(float time, string valuTrigger)
    {
        float t = 0;

        _myAnimator.SetBool("NextState", false);

        while (t < time)
        {
            t += Time.deltaTime;

            yield return null;
        }

        _myAnimator.SetBool("NextState", true);
        _myAnimator.SetTrigger(valuTrigger);
        //_myAnimator.ResetTrigger(valuTrigger);
    }


    private IEnumerator BreakingCorutine()
    {

        //AudioManager.instance.Play(SoundId.IceBreak);
        _myAnimator.SetTrigger("OnPlatform");

        yield return new WaitForSeconds(_timeToBreaking);

        _myAnimator.SetBool("IsBreaking", true);

        yield return new WaitForSeconds(_timeToWarning);

        _myAnimator.SetBool("IsBreaking", false);

        _myCollider.enabled = false;
        _ice.SetActive(false);

        yield return new WaitForSeconds(_timeToRecover);

        _myCollider.enabled = true;
        _ice.SetActive(true);
    }

    #region Memento

    public override void Save()
    {
        _currentState.Rec(_myCollider.enabled, _ice.activeInHierarchy);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;
        //StopAllCoroutines();
        var col = _currentState.Remember();
        _myCollider.enabled = (bool)col.parameters[0];
        _ice.SetActive((bool)col.parameters[1]);

        //_myAnimator.SetTrigger("Idle");
        Idle();
    }

    #endregion
}
