using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hits : MonoBehaviour
{
    protected int _damagePunch;
    public int Damage { set { _damagePunch = value; } }
    protected Entity _entity;

    private void Start()
    {
        _entity = GetComponentInParent<Entity>();
    }

}
