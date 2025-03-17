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
                //OldAudioManager.instance.PlaySFX(OldAudioManager.instance.fallIntoTheVoid);
                AudioManager.instance.Play(SoundId.Fall);
                target.Dead();

            }

            else target.Dead();
        }      
    }
}
