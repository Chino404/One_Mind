using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BreakablePlatform : Rewind
{
    [SerializeField, Tooltip("Tiempo que tarda para ROMPERSE"), Range(0,5f)] private float _timeToBreaking = 3;
    [SerializeField, Tooltip("Tiempo de animacion de advertencia"), Range(0, 0.5f)] private float _timeToWarning;
    [SerializeField, Tooltip("Tiempo que tarda para RECOMPONERSE"), Range(0,5f)] private float _timeToRecover = 3;
    private bool _isBreaking;

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
            if(!_isBreaking) StartCoroutine(Breaking());
        }
    }

    IEnumerator Breaking()
    {

        _isBreaking = true;
        AudioManager.instance.Play(SoundId.IceBreak);
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

        _isBreaking = false;
    }

    public override void Save()
    {
        _currentState.Rec(_myCollider.enabled, _ice.activeInHierarchy, _isBreaking);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;
        StopAllCoroutines();
        var col = _currentState.Remember();
        _myCollider.enabled = (bool)col.parameters[0];
        _ice.SetActive((bool)col.parameters[1]);
        _isBreaking = (bool)col.parameters[2];
    }
}
