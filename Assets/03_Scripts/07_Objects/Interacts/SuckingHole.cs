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
    [Tooltip("Lo que se le va a sumar a su escala en Y")]public float addScaleY;
    [Tooltip("Si en vez de SUMAR va a RESTAR")] public bool substract;

    [Space(5),Tooltip("La velociad / distancia de las particulas")]public float speedParticle;
}

public class SuckingHole : MonoBehaviour
{
    //[SerializeField, Tooltip("Geisers a los que va a afectar")] private Geiser[] _refGeisers;
    [Tooltip("Geisers a los que va a afectar")] public ValueGeiser[] geisers;
    [SerializeField] private ParticleSystem _myParticle;

    private void Awake()
    {
        _myParticle = GetComponentInChildren<ParticleSystem>();
    }


    private void Start()
    {
        foreach (var item in geisers)
        {
            item.refGeiser.StartScale(item.startScaleGeiser, item.startSpeedParticle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            foreach (var item in geisers)
            {
                if(item.substract) item.refGeiser.ModifyScaleYGeiser(- item.addScaleY, item.speedParticle);
                else item.refGeiser.ModifyScaleYGeiser(item.addScaleY, item.speedParticle);
                _myParticle.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != 3)
        {

            foreach (var item in geisers)
            {
                if(item.substract) item.refGeiser.RevertChange(- item.addScaleY, item.startSpeedParticle);
                else item.refGeiser.RevertChange(item.addScaleY, item.startSpeedParticle);
                _myParticle.Play();

            }
        }
    }
}
