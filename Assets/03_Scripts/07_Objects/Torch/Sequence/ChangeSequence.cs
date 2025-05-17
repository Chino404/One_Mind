using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSequence : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Objeto que tenga la secuencia de las antorchas")] private TorchSequence _torchSequence;

    [Space(5),SerializeField, Tooltip("Nueva secuencia")] private ChangeSequence _changeToNewSequenceRef;

    [Space(10),SerializeField, Tooltip(" 0 Izq. | 1 Der.")] private int[] _mySequence;
    [SerializeField, Range(0, 1.5f), Tooltip("Tiempo que las antorchas estan activadas antes de pasar a la otra")] private float _myActivedTime;

    private void Awake()
    {
        if (_torchSequence == null) Debug.LogError($"Falta referencia de la secuencia en: {gameObject.name}");
    }

    public void Active()
    {

        if (_changeToNewSequenceRef && !_changeToNewSequenceRef.isActiveAndEnabled) _changeToNewSequenceRef.enabled = true;

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
