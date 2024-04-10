using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamageEntity(float dmg, Vector3 target);

    public void GetUpDamage();
}
