using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected Vector3 _lastPosition;   // Última posición registrada de la plataforma
    protected Quaternion _lastRotation; // Última rotación registrada de la plataforma

    protected Vector3 _platformMovement; // Movimiento de la plataforma entre frames
    protected Quaternion _platformRotation; // Cambio de rotación de la plataforma entre frames  

    protected Rigidbody _rbCharacter;
    protected bool _isTrigger;

    public virtual void Start()
    {
        // Guardamos la posición y rotación inicial de la plataforma
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    public virtual void Update()
    {
        // Calculamos cuánto se ha movido la plataforma en este frame
        _platformMovement = transform.position - _lastPosition;

        // Calculamos el cambio de rotación de la plataforma
        _platformRotation = transform.rotation * Quaternion.Inverse(_lastRotation);

        if (_rbCharacter) MovePlataform();

        // Actualizamos la última posición y rotación
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    public void MovePlataform()
    {
        // Añado el movimiento de la plataforma a la posición del jugador
        _rbCharacter.MovePosition(_rbCharacter.position + GetPlatformMovement());

        // Primero, ajusto la posición del jugador en función de la rotación de la plataforma
        Vector3 relativePos = _rbCharacter.position - transform.position;
        relativePos = GetPlatformRotation() * relativePos;
        _rbCharacter.MovePosition(transform.position + relativePos);

        // Después, roto al jugador con la plataforma (si se desea que el jugador rote)
        _rbCharacter.MoveRotation(GetPlatformRotation() * _rbCharacter.rotation);
    }

    public Vector3 GetPlatformMovement()
    {
        // Devuelve el movimiento de la plataforma
        return _platformMovement;
    }

    public Quaternion GetPlatformRotation()
    {
        // Devuelve la rotación de la plataforma
        return _platformRotation;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Characters>() && _isTrigger)
        {
            _rbCharacter = other.GetComponent<Rigidbody>();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Characters>() && _isTrigger)
        {
            _rbCharacter = null;
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>() && !_isTrigger)
        {
            _rbCharacter = collision.gameObject.GetComponent<Rigidbody>();
        }
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>() && !_isTrigger)
        {
            _rbCharacter = null;
        }
    }
}
