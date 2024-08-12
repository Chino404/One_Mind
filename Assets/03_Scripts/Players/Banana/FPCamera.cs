using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour
{
    [Header("Clamping/Reprimir")]
    [SerializeField] private float _minRotation= -45f;
    [SerializeField] private float _maxRotation= 75f;
    private Animator _animatorCam;

    private float _mouseY;

    private void Awake()
    {
        _animatorCam = GetComponent<Animator>();
    }

    private void Start()
    {
        //GameManager.instance._animCamBanana = _animatorCam;
    }

    public void RotationCamera(float x, float y)
    {
        _mouseY += y;
        _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);

        transform.rotation = Quaternion.Euler(-_mouseY, x, 0f); //Se pone negativo para evitar los controles inversos
    }
}
