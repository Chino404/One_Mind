using System.Collections;
using System.Net;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public static CameraTracker Instance;

    [SerializeField]private CharacterTarget _myCharacterTarget;
    public CharacterTarget MyCharacterTarget { get { return _myCharacterTarget; }set { _myCharacterTarget = value; } }

    [Header("Components")]
    [SerializeField] private Transform _point;
    public Transform Point {  get { return _point; } }

    [SerializeField] private Transform _target;
    public Transform Target { get { return _target; } set { _target = value; } }

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeedPosition = 0.075f;
    
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeedRotation = 0.075f;

    Vector3 _desiredPos, _smoothPos;
    Quaternion _smoothRot;

    public bool isTeleporting;
    public bool isDeathCamera;
    [HideInInspector] public bool isPlayerDead;

    private void Awake()
    {
        Instance = this;
        //else if (_myCharacterTarget==Instance.MyCharacterTarget) Destroy(gameObject);

        transform.position = _point.position;

        if (_myCharacterTarget == CharacterTarget.Bongo)
        {
            if (GameManager.instance.bongoNormalCamera) Destroy(gameObject);
            else GameManager.instance.bongoNormalCamera = this;

            if(!_target) _target = GameManager.instance.modelBongo.transform;

            if(isDeathCamera)CamerasManager.instance.deathCameraBongo = this;
        }

        else if (_myCharacterTarget == CharacterTarget.Frank)
        {
            if (GameManager.instance.frankNormalCamera) Destroy(gameObject);
            else GameManager.instance.frankNormalCamera = this;

            if(!_target)_target = GameManager.instance.modelFrank.transform;

            if(isDeathCamera)CamerasManager.instance.deathCameraFrank = this;
        }

        if (_target == null) Debug.LogError($"Falta target en: {gameObject.name}");
        else gameObject.GetComponent<CameraTransparency>().target = _target;

        //gameObject.SetActive(false);
        if(isDeathCamera) gameObject.GetComponent<Camera>().enabled = false;

    }

    public void PlayerDeath()
    {
        isPlayerDead = true;
        gameObject.GetComponent<Camera>().enabled = true;
    }

    public void PlayerAlive()
    {
        isPlayerDead= false;
        gameObject.GetComponent<Camera>().enabled = false;
    }

    private void Start()
    {

        if(_point == null)
        {
            Debug.LogError($"Asignar punto en la c�mara: {gameObject.name}");
            return;
        }

    }

    //private void FixedUpdate()
    //{
    //    if (_target == null || _point == null) return;

    //    SetPositionAndRotationTarget();
    //}

    private void LateUpdate()
    {
        if (_target == null || _point == null) return;


        SetPositionAndRotationTarget();
    }

    public void SetPositionAndRotationTarget()
    {
        if (isTeleporting || isDeathCamera)
        {
            transform.SetPositionAndRotation(_point.position, _point.rotation);
        }
        else
        {
            // Calcular la posici�n deseada relativa al punto
            _desiredPos = _target.position + (_point.position - _target.position);

            // Suavizado en base a Time.deltaTime para asegurar suavidad continua
            _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeedPosition * Time.deltaTime * 60); //Multiplicando por 60 asegura que las velocidades del Lerp sean proporcionales y consistentes
            _smoothRot = Quaternion.Lerp(transform.rotation, _point.rotation, _smoothSpeedRotation * Time.deltaTime * 60);

            transform.SetPositionAndRotation(_smoothPos, _smoothRot);
        }

    }

    public void TransicionPoint(Transform newPoint)
    {
        Debug.LogWarning("Nuevo punto");
        _point = newPoint;
    }

}
