using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peak : Weapons
{
    public GameObject hit;
    public override void Attack()
    {
        
        base.Attack();
        Debug.Log("ataco con el pico");
        StartCoroutine(Hit());
    }

    

    IEnumerator Hit()
    {
        _isAttacking = true;
        hit.SetActive(true);
        yield return new WaitForSeconds(1f);
        hit.SetActive(false);
        _isAttacking = false;
    }
}
