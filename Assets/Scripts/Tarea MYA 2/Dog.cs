using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal
{
    [SerializeField] private int _life;

    private void Awake()
    {
        _life = _lifeAnimal;
    }

    private void Start()
    {
        GameManager.Instance._animalsList.Add(this);
    }

    public override void Action()
    {
        Debug.Log("Guau");
    }

    public override void GetDamage()
    {
        _life -= 10;
    }
}
