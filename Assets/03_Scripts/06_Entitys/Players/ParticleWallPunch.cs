using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWallPunch : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void ActiveParticle()
    {
        _particleSystem?.Play();
    }
}
