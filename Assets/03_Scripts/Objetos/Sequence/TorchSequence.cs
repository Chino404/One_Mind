using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSequence : MonoBehaviour
{
    [SerializeField,Tooltip("Las anotrchas que van en la secuencia")] private ActiveTorch[] _torchs;

    [SerializeField,Tooltip("El indice de la antorcha a prenderse")] public List<int> sequenceList;
    [Space(10),Range(0, 1.5f), Tooltip("Tiempo que las antorchas estan activadas antes de pasar a la otra | PÚBLICA")] public float activeTime;

    private void Awake()
    {
        for (int i = 0; i < _torchs.Length; i++)
        {
            if (_torchs[i] == null)Debug.LogError($"Hay un indice vacio en {gameObject.name}");
        }
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

        StartCoroutine(ContinueSequence());
    }

    IEnumerator ContinueSequence()
    {

        while (true)
        {
            #region PARPADEO
            for (int i = 0; i < _torchs.Length; i++)
            {
                //_torchs[i].myParticleSystem = _torchs[i].purpleFire;
                _torchs[i].Active();
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Deactive();
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Active();
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Deactive();
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < _torchs.Length; i++)
            {
                _torchs[i].Active();
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < _torchs.Length; i++)
            {
                //_torchs[i].myParticleSystem = _torchs[i].orangeFire;
                _torchs[i].Deactive();
            }

            yield return new WaitForSeconds(0.1f);
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
