using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSequence : MonoBehaviour, IInteracteable
{
    [SerializeField, Tooltip("Si solo se activa una sola vez")] private bool _isOneActive;
    private bool _isActive;
    [Space(3), SerializeField,Tooltip("Las anotrchas que van en la secuencia")] private TorchSwitch[] _torchs;

    [Space(10),SerializeField, Tooltip("Si es una secuencia en el que solo se utilizan 2 antorchas para mostrar todo el camino.")] private bool _isSequenceWithTwoTorches;

    [Space(2), SerializeField] private bool _isChangeColor;
    [Space(5),SerializeField, Tooltip("Color del fuego para cuando arranque la secuencia")] private Color _changeColorFire;
    [SerializeField] private List<Color> _saveColorFire;

    [Space(7),SerializeField, Tooltip(" 0 Izq. | 1 Der.")] public List<int> sequenceList;

    [Space(10),Range(0, 1.5f), Tooltip("Tiempo que las antorchas estan activadas antes de pasar a la otra | VARIABLE PÚBLICA")] public float delayActive;

    private void Awake()
    {
        for (int i = 0; i < _torchs.Length; i++)
        {
            if (_torchs[i] == null) Debug.LogError($"Hay un indice vacio en {gameObject.name}");
            else _saveColorFire.Add(_torchs[i].ColorFire);
        }
    }

    public void Active()
    {
        if(_isOneActive && !_isActive)
        {
            _isActive = true;
            StartSequence();
        }
        else if (!_isOneActive) StartSequence();
    }

    public void Deactive()
    {
        
    }

    public void StartSequence()
    {
        StopAllCoroutines();

        if(_isSequenceWithTwoTorches)
        {
            foreach (int index in sequenceList)
            {
                if(index > _torchs.Length || index < 0)
                {
                    Debug.LogError($"En el indice {index} hay un valor que no existe en el array de: {gameObject.name}");
                    return;
                }
            }
        }

        if (_isSequenceWithTwoTorches) StartCoroutine(ContinueLoopSequence());
        else StartCoroutine(ActiveTorchs());
    }

    IEnumerator ActiveTorchs()
    {
        for (int i = 0; i < _torchs.Length; i++)
        {
            _torchs[i].Active();

            yield return new WaitForSeconds(delayActive);
        }

        yield return null;
    }

    IEnumerator ContinueLoopSequence()
    {

        while (true)
        {
            #region PARPADEO
            float t = 0.25f;

            for (int i = 0; i < _torchs.Length; i++)
            {
                if(_isChangeColor) _torchs[i].ChangeColorFire(_changeColorFire);
                _torchs[i].Active();
            }

            yield return new WaitForSeconds(t);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Deactive();
            }

            yield return new WaitForSeconds(t);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Active();
            }

            yield return new WaitForSeconds(t);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Deactive();
            }

            yield return new WaitForSeconds(t);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Active();
            }

            yield return new WaitForSeconds(t);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Deactive();
            }

            yield return new WaitForSeconds(t);

            if (_isChangeColor)
            {
                for (int i = 0; i < _torchs.Length; i++) _torchs[i].ChangeColorFire(_saveColorFire[i]);
            }
            #endregion

            for (int i = 0; i < sequenceList.Count; i++)
            {
                _torchs[sequenceList[i]].Active();

                yield return new WaitForSeconds(delayActive);

                _torchs[sequenceList[i]].Deactive();

                yield return new WaitForSeconds(delayActive);
            }
        }
    }

}
