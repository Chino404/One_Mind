using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morse : Rewind
{
    // Variables configurables desde el Inspector
    [Header("Rotaci�n Configurable")]
    [Tooltip("�ngulo a rotar en grados sobre el eje Y.")]
    public float targetAngleY;
    [SerializeField] private float _startRotation;

    [Tooltip("Tiempo que tardar� la rotaci�n en completarse (segundos).")]
    public float rotationTime;

    // Variable para verificar si ya est� rotando
    private bool isRotating = false;

    [Space(10), SerializeField] private MovePlataform _platformMove;

    private Animator _myAnimator;


    public override void Awake()
    {
        base.Awake();

        _myAnimator = GetComponentInChildren<Animator>();

        _startRotation = transform.eulerAngles.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Characters>())
        {
            StartCoroutine(RotationMorsa());
        }
    }

    IEnumerator RotationMorsa()
    {
        // Evitar m�ltiples rotaciones simult�neas
        isRotating = true;

        //// Rotaci�n inicial y objetivo
        //float startYRotation = transform.eulerAngles.y;
        //float endYRotation = startYRotation + targetAngleY;

        //// Tiempo transcurrido
        //float elapsedTime = 0f;

        //while (elapsedTime < rotationTime)
        //{
        //    // Incrementa el tiempo transcurrido
        //    elapsedTime += Time.deltaTime;

        //    // Interpolar la rotaci�n entre el �ngulo inicial y el objetivo
        //    float newYRotation = Mathf.Lerp(startYRotation, endYRotation, elapsedTime / rotationTime);

        //    // Aplicar la rotaci�n al objeto
        //    transform.eulerAngles = new Vector3(
        //        transform.eulerAngles.x,
        //        newYRotation,
        //        transform.eulerAngles.z
        //    );

        //    yield return null; // Espera al siguiente frame
        //}

        _myAnimator.SetTrigger("Punch");

        yield return new WaitForSeconds(0.75f);
        _platformMove.IsActiveMove = true;

        //elapsedTime = 0f;

        //while (elapsedTime < rotationTime)
        //{
        //    // Incrementa el tiempo transcurrido
        //    elapsedTime += Time.deltaTime;

        //    // Interpolar la rotaci�n entre el �ngulo inicial y el objetivo
        //    float newYRotation = Mathf.Lerp(startYRotation, endYRotation, elapsedTime / rotationTime);

        //    // Aplicar la rotaci�n al objeto
        //    transform.eulerAngles = new Vector3(
        //        transform.eulerAngles.x,
        //        newYRotation,
        //        transform.eulerAngles.z
        //    );

        //    yield return null; // Espera al siguiente frame
        //}

        //// Asegurarse de que el objeto llegue al �ngulo final exacto
        //transform.eulerAngles = new Vector3(
        //    transform.eulerAngles.x,
        //    endYRotation,
        //    transform.eulerAngles.z
        //);

        // Finaliza la rotaci�n
        isRotating = false;
    }

    public override void Load()
    {
        _currentState.Rec(transform.rotation);
    }

    public override void Save()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, (float)col.parameters[0], transform.eulerAngles.z);
        transform.rotation = (Quaternion)col.parameters[0];

    }

}

