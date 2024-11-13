using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValueGeiser
{
    [Tooltip("Geiser que se va a modificar")]public Geiser refGeiser;

    [Space(5),Header("Como comienza el Geiser")]
    [Tooltip("Escala con la q va a iniciar el geiser")]public float startScaleGeiser = 2f;
    [Tooltip("Velocidad / distancia en la que comienza las particulas")] public float startSpeedParticle;

    [Space(5),Header("Como se va a actualizar")]
    [Tooltip("La escala en la que se va a modificar ahora")]public float newScaleGeiser;
    [Tooltip("La velociad / distancia de las particulas")]public float speedParticle;
}

public class SuckingHole : MonoBehaviour
{
    //[SerializeField, Tooltip("Geisers a los que va a afectar")] private Geiser[] _refGeisers;
    [Tooltip("Geisers a los que va a afectar")] public ValueGeiser[] geisers;



    private void Start()
    {
        foreach (var item in geisers)
        {
            item.refGeiser.ModifyScaleYGeiser(item.startScaleGeiser, item.startSpeedParticle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            foreach (var item in geisers)
            {
                item.refGeiser.ModifyScaleYGeiser(item.newScaleGeiser, item.speedParticle);
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
