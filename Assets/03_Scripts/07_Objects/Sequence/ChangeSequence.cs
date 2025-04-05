using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSequence : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Objeto que tenga la secuencia de las antorchas")] private TorchSequence _torchSequence;
    [SerializeField] private ChangeSequence _changeSequenceRef;

    [Space(10),SerializeField] private int[] _mySequence;
    [SerializeField, Range(0, 1.5f), Tooltip("Tiempo que las antorchas estan activadas antes de pasar a la otra")] private float _myActivedTime;

    private void Awake()
    {
        if (_torchSequence == null) Debug.LogError($"Falta referencia de la secuencia en: {gameObject.name}");
    }

    public void Active()
    {

        if (_changeSequenceRef && !_changeSequenceRef.isActiveAndEnabled) _changeSequenceRef.enabled = true;

        _torchSequence.sequenceList.Clear(); //Limpio la lista

        _torchSequence.activeTime = _myActivedTime;

        for (int i = 0; i < _mySequence.Length; i++)
        {
            _torchSequence.sequenceList.Add(_mySequence[i]); //Lleno la lista con los valores
        }

        _torchSequence.StartSequence(); //Arranco la secuencia

        this.enabled = false;
    }

    public void Deactive()
    {
        
    }
}
