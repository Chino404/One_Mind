using System.Collections;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public static CameraTracker Instance;

    public CharacterTarget characterTarget;

    [Header("Components")]
    [SerializeField] private Transform _point;
    public Transform Point {  get { return _point; } }

    private Transform _target;

    [Header("Smoothing Values")]
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeedPosition = 0.075f;
    [Range(0.01f, 0.125f)] [SerializeField] float _smoothSpeedRotation = 0.075f;

    Vector3 _desiredPos, _smoothPos;
    Quaternion _smoothRot;


    private void Awake()
    {
        Instance = this;

        transform.position = _point.position;

        if (characterTarget == CharacterTarget.Bongo)
        {
            _target = GameManager.instance.bongo.transform;

            GameManager.instance.bongoCamera = this;
        }

        else if (characterTarget == CharacterTarget.Frank)
        {
            _target = GameManager.instance.frank.transform;

            GameManager.instance.frankCamera = this;
        }

        if (_target == null) Debug.LogError($"Falta target en: {gameObject.name}");
        else gameObject.GetComponent<CameraTransparency>().target = _target;
    }

    private void Start()
    {

        if(_point == null)
        {
            Debug.LogError($"Asignar punto en la cámara: {gameObject.name}");
            return;
        }

        //StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();

        transform.position = _point.position;

        if (characterTarget == CharacterTarget.Bongo)
        {
            _target = GameManager.instance.bongo.transform;

            GameManager.instance.bongoCamera = this;
        }

        else if (characterTarget == CharacterTarget.Frank)
        {
            _target = GameManager.instance.frank.transform;

            GameManager.instance.frankCamera = this;
        }

        if (_target == null) Debug.LogError("FALTA TARGET");
        else gameObject.GetComponent<CameraTransparency>().target = _target;

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

    private void SetPositionAndRotationTarget()
    {
        // Calcular la posición deseada relativa al punto
        _desiredPos = _target.position + (_point.position - _target.position);

        // Suavizado en base a Time.deltaTime para asegurar suavidad continua
        _smoothPos = Vector3.Lerp(transform.position, _desiredPos, _smoothSpeedPosition * Time.deltaTime * 60); //Multiplicando por 60 asegura que las velocidades del Lerp sean proporcionales y consistentes
        _smoothRot = Quaternion.Lerp(transform.rotation, _point.rotation, _smoothSpeedRotation * Time.deltaTime * 60);

        transform.SetPositionAndRotation(_smoothPos, _smoothRot);

        Debug.DrawLine(transform.position, _target.position, Color.green);
    }

    public void TransicionPoint(Transform newPoint)
    {
        _point = newPoint;
    }

}
