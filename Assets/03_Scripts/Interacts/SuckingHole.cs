using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckingHole : MonoBehaviour
{
    [SerializeField, Tooltip("Geisers a los que va a afectar")] private Geiser[] _refGeisers;
    [SerializeField, Tooltip("Capas que van a interactuar")] private LayerMask _layersAffect;

    [Space(10), SerializeField] private float _newScaleGeiser;
    [SerializeField] private float _newSpeedParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            for (int i = 0; i < _refGeisers.Length; i++)
            {
                _refGeisers[i].ModifyScaleYGeiser(_newScaleGeiser, _newSpeedParticle);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != 3)
        {
            for (int i = 0; i < _refGeisers.Length; i++)
            {
                _refGeisers[i].RevertChange();
            }
        }
    }
}
