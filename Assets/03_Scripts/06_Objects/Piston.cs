using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : Mechanism
{
    [SerializeField] private GameObject _pistonModel;
    [SerializeField] private GameObject _deathZone;
    [SerializeField] private bool _isTouchDead;

    [Space(10), SerializeField] private Transform _posIni;
    [SerializeField] private Transform _posEnd;

    [Space(5), SerializeField] private float _timeToArrive = 0;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    public override void ActiveMechanism()
    {
        StartCoroutine(ToMovePiston(true));
    }

    public override void DeactiveMechanism()
    {
        StartCoroutine(ToMovePiston(false));
        _deathZone.gameObject.SetActive(false);
    }

    IEnumerator ToMovePiston(bool isActive)
    {
        float elapsedTime = 0;
        Transform pos;

        if (isActive) pos = _posEnd;
        else pos = _posIni; 

        while(elapsedTime < _timeToArrive)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _timeToArrive;

            //_pistonModel.transform.position = Vector3.Lerp(_pistonModel.transform.position, pos.position, t);
            Vector3 newPosition = Vector3.Lerp(_pistonModel.transform.position, pos.position, t);

            _rigidbody.MovePosition(newPosition);

            var distance = Vector3.Distance(_rigidbody.position, _posEnd.position);

            if (_isTouchDead && isActive && distance <= 0.3) _deathZone.gameObject.SetActive(true);

            yield return null;
        }

        _pistonModel.transform.position = pos.position;
    }
}
