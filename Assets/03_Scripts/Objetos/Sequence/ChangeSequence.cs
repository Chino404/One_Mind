using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSequence : MonoBehaviour, IInteracteable
{
    [Space(10), SerializeField, Tooltip("Objeto que tenga la secuencia de las antorchas")] private TorchSequence _torchSequence;

    [SerializeField] private int[] _mySequence;

    //[Space(10), SerializeField, Tooltip("Q se elijan aleatoriamente los valores")] private bool _randomNumber;
    //[SerializeField, Tooltip("Valor maximo del index de las antorchas | ACLARACIÓN: Comienza desde 0 y el maximo es incluido")] private int _maxIndex;

    private Collider _myCollider;

    private void Awake()
    {
        _myCollider = GetComponent<Collider>();

        if (_torchSequence == null) Debug.LogError($"Falta referencia de la secuencia en: {gameObject.name}");
    }

    public void Active()
    {
        _torchSequence.sequenceList.Clear(); //Limpio la lista

        //if(_randomNumber)
        //{
        //    RandomIndex();
        //    return;
        //}

        for (int i = 0; i < _mySequence.Length; i++)
        {
            _torchSequence.sequenceList.Add(_mySequence[i]); //Lleno la lista con los valores
        }

        _torchSequence.StartSequence(); //Arranco la secuencia

        _myCollider.enabled = false;
    }

    //void RandomIndex()
    //{
    //    for (int i = 0; i < _mySequence.Length; i++)
    //    {
    //        _torchSequence.sequenceList.Add(Random.Range(0, _maxIndex + 1)); //Lleno la lista con valores aleatorios
    //    }

    //    _torchSequence.StartSequence();

    //    _myCollider.enabled = false;
    //}

    public void Deactive()
    {
        
    }
}
