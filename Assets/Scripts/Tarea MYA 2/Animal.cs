using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    protected int _lifeAnimal = 100;

    public abstract void Action();

    public abstract void GetDamage();
}
