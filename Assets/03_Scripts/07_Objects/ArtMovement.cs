
using System.Collections;
using UnityEngine;

public class ArtMovement : MonoBehaviour
{
    [Header("Movimiento de Barrido")]
    public Transform parteInferior; // Referencia al Transform de la parte móvil
    public GameObject modelo3D;     // Referencia explícita al modelo 3D

    [Space(10)]
    public float anguloMaximo = 45.0f; // Ángulo máximo de barrido en grados
    public float velocidad = 3.0f;     // Velocidad del barrido

    [Space(10)]
    public bool Canmove = false;
    private Quaternion rotacionInicial;

    public bool isWithSkull;

    private AudioSetting _audioSetting;
    private bool _isSoundPlaying = false;

    private void Awake()
    {
        _audioSetting = GetComponent<AudioSetting>();
    }

    void Start()
    {

        if(Canmove)
        {
            if (parteInferior == null)
            {
                Debug.LogError("La parte inferior no está asignada.");
                return;
            }

            if (modelo3D == null)
            {
                Debug.LogError("El modelo 3D no está asignado.");
                return;
            }

            rotacionInicial = parteInferior.localRotation;

            StartCoroutine(Rotate());

        }

        if (isWithSkull) StartCoroutine(LaughSkull());
    }

    IEnumerator Rotate()
    {
        while (parteInferior != null && modelo3D != null && Canmove)
        {
            float angulo = Mathf.Sin(Time.time * velocidad) * anguloMaximo;

            // Rotar el punto de referencia
            parteInferior.localRotation = rotacionInicial * Quaternion.Euler(0, 0, angulo);

            float zAngle = parteInferior.localRotation.eulerAngles.z;

            if (zAngle > 180f) zAngle -= 360f; // Ajuste para manejar el rango -180 a 180

            if (zAngle >= -0.5f && zAngle <= 0.5f)
            {
                _isSoundPlaying = false;
            }

            if ((zAngle >= 8f || zAngle <= -9f) && !_isSoundPlaying)
            {
                //Debug.Log("SONIDO CAGE");
                _isSoundPlaying = true;
                _audioSetting.Play(SoundId.Always, Random.Range(0,1));
            }

            // Asegurarse de que el modelo 3D siga el movimiento
            modelo3D.transform.localRotation = parteInferior.localRotation;


            yield return null; // Espera 1 frame y continúa
        }
    }

    IEnumerator LaughSkull()
    {
        while (isWithSkull)
        {
            _audioSetting?.Play(SoundId.OnlyActive, Random.Range(0,2));

            yield return new WaitForSeconds(6.5f);
        }
    }
}

