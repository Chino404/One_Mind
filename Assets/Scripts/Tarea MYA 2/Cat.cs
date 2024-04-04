using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Animal
{
    [SerializeField] private int _life;

    private void Awake()
    {
        _life = _lifeAnimal;
    }

    private void Start()
    {
        GameManager.instance._animalsList.Add(this);
    }

    public override void Action()
    {
        Debug.Log("Miau");

    }

    public override void GetDamage()
    {
        _life -= 10;
    }
}
