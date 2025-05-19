using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValueGeiser
{
    [Tooltip("-> Geiser que se va a modificar")]public Geiser refGeiser;

    [Space(5),Header("Como comienza el Geiser")]
    [Tooltip("Escala con la q va a iniciar el geiser")]public float startScaleGeiser = 2f;
    [Tooltip("Velocidad / distancia en tiempo de vida en la que comienza de las particulas")] public float startSpeedLifeTimeParticle;
    [Tooltip("Velocidad / distancia en la que comienza las particulas")] public float startSpeedParticle;

    [Space(5),Header("-> Como se va a actualizar")]
    [Tooltip("De que manera va se van a modificado el geiser")]public TypeModifyParticleGeiser typeModify;
    [Tooltip("Lo que se le va a sumar a su escala en Y")]public float addScaleY;
    [Tooltip("Velocidad / distancia en tiempo de vida de las particulas")] public float speedLifeTimeParticle;
    [Tooltip("La velociad / distancia de las particulas")]public float speedParticle;
    
}

public class SuckingHole : MonoBehaviour
{
    //[SerializeField, Tooltip("Geisers a los que va a afectar")] private Geiser[] _refGeisers;
    [Tooltip("Geisers a los que va a afectar")] public ValueGeiser[] geisers;
    [SerializeField] private ParticleSystem _myParticle;
    private bool _isChangingScale;
    private void Awake()
    {
        _myParticle = GetComponentInChildren<ParticleSystem>();
    }


    private void Start()
    {
        foreach (var item in geisers)
        {
            item.refGeiser.StartScale(item.startScaleGeiser, item.startSpeedParticle, item.startSpeedLifeTimeParticle);
        }
        Debug.Log("cambio escala");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3&&!_isChangingScale)
        {
            foreach (var item in geisers)
            {
                if(item.typeModify == TypeModifyParticleGeiser.substract) item.refGeiser.ModifyScaleYGeiser(- item.addScaleY, item.speedParticle, item.speedLifeTimeParticle, item.typeModify);

                else item.refGeiser.ModifyScaleYGeiser(item.addScaleY, item.speedParticle, item.speedLifeTimeParticle, item.typeModify);

                _myParticle.Stop();
            }
            _isChangingScale = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != 3)
        {

            foreach (var item in geisers)
            {
                //Si se sumo el valor de las particulas, mando para que se reste.
                if(item.typeModify == TypeModifyParticleGeiser.add) item.refGeiser.RevertChange(item.addScaleY, item.speedParticle, item.speedLifeTimeParticle, item.typeModify);

                //Sino lo mando para que se sobrescriba
                else item.refGeiser.RevertChange(item.addScaleY, item.startSpeedParticle, item.startSpeedLifeTimeParticle, item.typeModify);

                _myParticle.Play();

            }
            _isChangingScale = false;
        }
    }
}
