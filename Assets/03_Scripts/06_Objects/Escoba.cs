
using UnityEngine;

public class Escoba : MonoBehaviour
{
    [Header("Movimiento de Barrido")]
    public Transform parteInferior; // Referencia al Transform de la parte móvil
    public GameObject modelo3D;     // Referencia explícita al modelo 3D
    public float anguloMaximo = 45.0f; // Ángulo máximo de barrido en grados
    public float velocidad = 3.0f;     // Velocidad del barrido

    private Quaternion rotacionInicial;

    void Start()
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
    }

    void Update()
    {
        if (parteInferior != null && modelo3D != null)
        {
            float angulo = Mathf.Sin(Time.time * velocidad) * anguloMaximo;

            // Rotar el punto de referencia
            parteInferior.localRotation = rotacionInicial * Quaternion.Euler(0, 0, angulo);

            // Asegurarse de que el modelo 3D siga el movimiento
            modelo3D.transform.localRotation = parteInferior.localRotation;
        }
    }
}

