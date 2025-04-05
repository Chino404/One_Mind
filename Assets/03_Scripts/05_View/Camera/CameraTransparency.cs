using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class CameraTransparency : MonoBehaviour
{
    [Header("PARAMETERS")]
    [Space(5)]

    [SerializeField] private LayerMask _maskTransparent;
    [SerializeField, Range(0, 1)] private float _durationFade = 0.3f;
    [HideInInspector] public Transform target;

    private float _dist;
    [SerializeField]private Vector3 _dir;
    //private TransparencyMaterial _transMaterial;
    private ITransparency _transMaterial;

    [Space(10), SerializeField, Tooltip("Escala del BoxCast")] private Vector3 _boxCastSize = new Vector3(1, 1, 1); // Tamaño del cubo
    [Tooltip("Centro del BoxCast")] private Vector3 _centerBoxCast;
    [SerializeField,Tooltip("Rotación del Boxcast")] private Quaternion _rotationBoxCast;
    private Transform _target;

    private void Start()
    {
        _target = gameObject.GetComponent<CameraTracker>().Target;

        //_rotationBoxCast = Quaternion.Euler(-transform.rotation.eulerAngles.x - 23, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);


    }

    private void Update()
    {
        if (target != null)
        {
            _dist = Vector3.Distance(transform.position, target.position);
            _dir = (_target.position - transform.position).normalized;

            _centerBoxCast = transform.position - _dir * (_dist / 2);
            _boxCastSize = new Vector3(_boxCastSize.x, _dist, _boxCastSize.z);
            _rotationBoxCast = Quaternion.Euler(-transform.rotation.eulerAngles.x - 23, transform.rotation.eulerAngles.y + _dir.y, transform.rotation.eulerAngles.z + _dir.z);

            if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out RaycastHit hit, _dist, _maskTransparent))
            {
                if (hit.transform.GetComponent<ITransparency>() != null)
                {
                    //_transMaterial = hit.transform.GetComponent<TransparencyMaterial>();
                    _transMaterial = hit.transform.GetComponent<ITransparency>();

                    _transMaterial.Fade(_durationFade);
                }
            }

            //if (Physics.BoxCast(_centerBoxCast, _boxCastSize / 2, _dir, out RaycastHit hitBox, _rotationBoxCast, _dist, _maskTransparent))
            //{
            //    if (hitBox.transform.GetComponent<ITransparency>() != null)
            //    {
            //        _transMaterial = hitBox.transform.GetComponent<ITransparency>();

            //        _transMaterial.Fade(_durationFade);
            //    }
            //}

            else if (_transMaterial != null)
            {
                //Debug.Log($"Desaparecer {_transMaterial}");
                _transMaterial.Appear(_durationFade);
                _transMaterial = null;
            }

            //else
            //{
            //    Debug.Log("Desaparecer");
            //    _transMaterial.Appear(_durationFade);
            //    _transMaterial = null;
            //}
        }
    }

    private void OnDrawGizmos()
    {
        if (!_target) return;
        //LINEA
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _target.position);

        Vector3 direction = (_target.position - transform.position).normalized; //Dirección del Raycast (hacia el punto final)

        float distance = Vector3.Distance(transform.position, _target.position); //Caluclo la distancia

        //CUBO
        // Dibujar el cubo en el punto de inicio con la dirección y tamaño correcto
        //Gizmos.color = Color.red;
        //var centerBox = transform.position + direction * (distance / 2);

        // Guarda la matriz actual
        //Gizmos.matrix = Matrix4x4.TRS(centerBox, _rotationBoxCast, _boxCastSize);

        // Dibuja el cubo en la posición y rotación especificadas
        //Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        // Restaura la matriz original
        //Gizmos.matrix = Matrix4x4.identity;
    }

}
