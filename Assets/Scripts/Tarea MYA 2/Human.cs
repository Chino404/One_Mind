using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField]private int _lifeHuman = 100;

    private void Start()
    {
        GameManager.instance._humanList.Add(this);
    }

    public void GetDamage()
    {
        _lifeHuman -= 10;
    }

}
