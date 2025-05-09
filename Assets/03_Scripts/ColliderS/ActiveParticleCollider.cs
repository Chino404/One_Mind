using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveParticleCollider : MonoBehaviour
{
    public ParticleSystem particles;
    public int layer;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.GetComponent<Characters>() || other.gameObject.layer == layer)
        {
            Debug.Log("particulas");
            particles.Play();
        }
    }
}
