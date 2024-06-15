using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveThings : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] private GameObject _explosiveObject;
    
   
    public void Explode()
    {

        foreach (var item in _particles)
        {
            item.Play();
        }
        Invoke("DisableParticles", 3f);

	}

    void DisableParticles()
    {
        foreach (var item in _particles)
        {
            item.Stop();
        }
    }
    
}
