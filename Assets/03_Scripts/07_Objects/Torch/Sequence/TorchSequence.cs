using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSequence : MonoBehaviour, IInteracteable
{
    [SerializeField,Tooltip("Las anotrchas que van en la secuencia")] private TorchSwitch[] _torchs;
    [SerializeField, Tooltip("Si se repite la secuencia")] private bool _isLoop;


    [Space(10), SerializeField] private bool _isChangeColor;
    [SerializeField, Tooltip("Color del fuego para cuando arranque la secuencia")] private Color _changeColorFire;
    /*[SerializeField]*/ private List<Color> _saveColorFire;

    [Space(10),SerializeField, Tooltip(" 0 Izq. | 1 Der.")] public List<int> sequenceList;
    [Space(10),Range(0, 1.5f), Tooltip("Tiempo que las antorchas estan activadas antes de pasar a la otra | VARIABLE PÚBLICA")] public float activeTime;

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
        Debug.Log("ACTIVAR ANTORCHAS");
        StartSequence();
    }

    public void Deactive()
    {
        
    }

    public void StartSequence()
    {
        StopAllCoroutines();

        foreach (var item in sequenceList)
        {
            if(item > _torchs.Length || item < 0)
            {
                Debug.LogError($"En el indice {item} hay un valor que no existe en el array de: {gameObject.name}");
                return;
            }
        }

        if (_isLoop) StartCoroutine(ContinueLoopSequence());
        else StartCoroutine(ActiveTorchs());
    }

    IEnumerator ActiveTorchs()
    {
        for (int i = 0; i < sequenceList.Count; i++)
        {
            _torchs[sequenceList[i]].Active();

            yield return new WaitForSeconds(activeTime);
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

                yield return new WaitForSeconds(activeTime);

                _torchs[sequenceList[i]].Deactive();

                yield return new WaitForSeconds(activeTime);
            }
        }
    }

}
