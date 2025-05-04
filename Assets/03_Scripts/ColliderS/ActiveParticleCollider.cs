using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveParticleCollider : MonoBehaviour
{
    public ParticleSystem particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
            particles.Play();
    }
}
