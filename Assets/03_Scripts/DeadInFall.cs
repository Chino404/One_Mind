using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadInFall : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {

            if (other.gameObject.layer == 3)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.fallIntoTheVoid);
                target.Dead();
            }

            else target.Dead();
        }      
    }
}
