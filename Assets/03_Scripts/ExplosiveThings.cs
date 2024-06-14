using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveThings : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] private GameObject _explosiveObject;
    
   

    public IEnumerator PlayParticles()
    {
        foreach (var item in _particles)
        {
            item.Play();
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
