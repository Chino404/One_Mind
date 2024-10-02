using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinButton : MonoBehaviour, IInteracteable
{
    [Header("-> PRESSED")]
    [SerializeField] private GameObject[] _floors;

    [SerializeField] private Transform _pivotPlatforms;
    [SerializeField, Range(-360f, 360f) , Tooltip("Grados a rotar")] private float _degrees = 90f;
    [SerializeField] private float _rotationTime = 0.5f;

    [Header("-> BUCLE")]
    [SerializeField, Tooltip("Para que giren")] private bool _spin;
    [SerializeField] private float _speedRotation;
    

    private bool _isRotating = false;

    private void Awake()
    {
        if (!_pivotPlatforms) Debug.LogWarning($"Falta la referencia del pivot para las plataformas en: {gameObject.name}");
    }

    private void Update()
    {
        //Rotacion de plataformas
        if(_spin)_pivotPlatforms.Rotate(0, _speedRotation * Time.deltaTime ,0);
    }

    public void Active()
    {
        //if (_isPressing) return;
        if(!_isRotating) StartCoroutine(MoveFloors());


        //_pivotPlatforms.Rotate(0, _speedRotation * Time.deltaTime, 0);


        //Vector3 pos = default;
        //for (int i = 0; i < _floors.Length; i++)
        //{

        //    //Vector3 pos = _floors[i].transform.position;

        //    Vector3 newPos = _floors[i + 1].transform.position;

        //    if (i == _floors.Length)
        //        newPos = _floors[0].transform.position;

        //    pos = newPos;
        //}
        //for (int i = 0; i < _floors.Length; i++)
        //{
        //    _floors[i].transform.position = pos;
        //}
    }

    IEnumerator MoveFloors()
    {
        _isRotating = true;

        float elapsedTime = 0f;

        // Guardamos la rotación inicial
        Quaternion initialRotation = _pivotPlatforms.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, _degrees, 0f); // Grados que quiero rotar

        while (elapsedTime < _rotationTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / _rotationTime);

            // Rotoel objeto alrededor de su eje Y
            _pivotPlatforms.rotation = Quaternion.Slerp(initialRotation, targetRotation, t) ;

            yield return null;
        }

        _isRotating = false;
    }

    public void Desactive()
    {

    }
}
