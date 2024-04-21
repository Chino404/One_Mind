using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attack : IState
{
    Enemy _enemy;
    bool _isHitting;

    public Attack (Enemy enemy)
    {
        _enemy = enemy;
    }
    public void OnEnter()
    {
        Debug.Log("ataco");
    }

    public void OnUpdate()
    {
        Debug.Log("estoy atacando");
        _isHitting = true;
    }

    public void OnExit()
    {
        Debug.Log("dejo de atacar");
    }

    void Hit()
    {
        if (_isHitting == true)
        {
            _enemy.hit.SetActive(true);
            new WaitForSeconds(0.5f);
            _enemy.hit.SetActive(false);
        }
    }
}
