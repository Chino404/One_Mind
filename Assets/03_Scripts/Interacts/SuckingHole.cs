using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValueGeiser
{
    [Tooltip("Geiser que se va a modificar")]public Geiser refGeiser;
    [Tooltip("La escala en la que se va a modificar ahora")]public float scaleGeiser;
    [Tooltip("La velociad / distancia de las particulas")]public float speedParticle;
}

public class SuckingHole : MonoBehaviour
{
    //[SerializeField, Tooltip("Geisers a los que va a afectar")] private Geiser[] _refGeisers;
    [Tooltip("Geisers a los que va a afectar")] public ValueGeiser[] geisers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            foreach (var item in geisers)
            {
                item.refGeiser.ModifyScaleYGeiser(item.scaleGeiser, item.speedParticle);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != 3)
        {

            foreach (var item in geisers)
            {
                item.refGeiser.RevertChange();
            }
        }
    }
}
