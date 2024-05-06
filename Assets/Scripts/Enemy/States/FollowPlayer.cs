using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : IState
{
    Enemy _enemy;
    FSM _fsm;

    public FollowPlayer(Enemy enemy, FSM fsm)
    {
        _enemy = enemy;
        _fsm = fsm;
    }
    public void OnEnter()
    {
        Debug.Log("hago seek");
        
    }

    public void OnUpdate()
    {
        //if(!_enemy._inAir)
        _enemy.anim.SetBool("Walk", true);
        _enemy.AddForce(_enemy.Seek(_enemy.target.transform.position));
        _enemy.transform.position += _enemy._velocity * Time.deltaTime;

        if ((_enemy.transform.position - _enemy.target.transform.position).sqrMagnitude <= _enemy.attackDistance * _enemy.attackDistance)
            _fsm.ChangeState("Attack");

        if ((_enemy.transform.position - _enemy.target.transform.position).sqrMagnitude >= _enemy.viewRadius * _enemy.viewRadius)
            _fsm.ChangeState("Idle");
    }

    public void OnExit()
    {
        Debug.Log("cambio de estado");
    }

    
}
