using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBanana : MonoBehaviour
{
    [SerializeField] Vector3 _normalTarget;
    [SerializeField] Vector3 _chargedTarget;

    private void Awake()
    {
        _normalTarget = transform.localPosition;
    }

    public void ChargedAttack()
    {
        transform.localPosition = _chargedTarget;
    }

    public void NormalPosition()
    {
        transform.localPosition = _normalTarget;
    }
}
