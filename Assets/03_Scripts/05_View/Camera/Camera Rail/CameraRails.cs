using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRails : MonoBehaviour
{
    public static CameraRails Instance;

    [SerializeField] private CharacterTarget _myCharacterTarget;
    public CharacterTarget MyCharacterTarget { get { return _myCharacterTarget; } set { _myCharacterTarget = value; } }

    [Header("Components")]
    [SerializeField] private PointsOfTheCameraRails _currentPoint;
    public PointsOfTheCameraRails CurrentPoint { get { return _currentPoint; } }

    [SerializeField] public PointsOfTheCameraRails nextPoint;

    [Space(8),SerializeField] private Transform _target;
    public Transform Target { get { return _target; } set { _target = value; } }

    [Space(10), Header("Smoothing Values")]
    [Range(0.01f, 0.125f)][SerializeField] float _smoothSpeedPosition = 0.075f;
    public float speed;

    [Range(0.01f, 0.125f)][SerializeField] float _smoothSpeedRotation = 0.075f;

    Vector3 _desiredPos, _smoothPos;
    Quaternion _smoothRot;

    private void Awake()
    {
        Instance = this;

        if (_currentPoint != null)
        {
            transform.position = _currentPoint.transform.position;

            if(_currentPoint.nextPoints.Length != 0) nextPoint = _currentPoint.nextPoints[0];
        }
        else Debug.LogError($"Asignar punto en la cámara: {gameObject.name}");

        if (_myCharacterTarget == CharacterTarget.Bongo)
        {
            if (GameManager.instance.bongoRails) Destroy(gameObject);
            else GameManager.instance.bongoRails = this;

            if (!_target) _target = GameManager.instance.bongo.transform;
        }

        else if (_myCharacterTarget == CharacterTarget.Frank)
        {
            if (GameManager.instance.frankRails) Destroy(gameObject);
            else GameManager.instance.frankRails = this;

            if (!_target) _target = GameManager.instance.frank.transform;
        }

        if (_target == null) Debug.LogError($"Falta target en: {gameObject.name}");
        //else gameObject.GetComponent<CameraTransparency>().target = _target;
    }

    private void LateUpdate()
    {
        if (_target == null || _currentPoint == null) return;


        //SetPositionAndRotationTarget();
    }

    private void SetPositionAndRotationTarget()
    {

        //// Calcular la posición deseada relativa al punto
        //_desiredPos = _target.position + (_currentPoint.transform.position - _target.position);

        //// Suavizado en base a Time.deltaTime para asegurar suavidad continua
        //_smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeedPosition * Time.deltaTime * 60); //Multiplicando por 60 asegura que las velocidades del Lerp sean proporcionales y consistentes
        //_smoothRot = Quaternion.Lerp(transform.rotation, _currentPoint.transform.rotation, _smoothSpeedRotation * Time.deltaTime * 60);

        //transform.SetPositionAndRotation(_smoothPos, _smoothRot);


        transform.position = Vector3.MoveTowards(transform.position, nextPoint.transform.position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, nextPoint.transform.position) < 0.02f)
        {
            _currentPoint = nextPoint;

            nextPoint = _currentPoint.nextPoints[0];
        }

    }

    //public void TransicionPoint(Transform newPoint)
    //{
    //    _currentPoint = newPoint;
    //}
}
