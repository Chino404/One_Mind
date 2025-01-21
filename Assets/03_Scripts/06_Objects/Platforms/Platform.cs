using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected Vector3 _lastPosition;   // �ltima posici�n registrada de la plataforma
    protected Quaternion _lastRotation; // �ltima rotaci�n registrada de la plataforma

    protected Vector3 _platformMovement; // Movimiento de la plataforma entre frames
    protected Quaternion _platformRotation; // Cambio de rotaci�n de la plataforma entre frames  

    protected Rigidbody _rbCharacter;
    protected bool _isTrigger;

    public virtual void Start()
    {
        // Guardamos la posici�n y rotaci�n inicial de la plataforma
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    public virtual void Update()
    {
        // Calculamos cu�nto se ha movido la plataforma en este frame
        _platformMovement = transform.position - _lastPosition;

        // Calculamos el cambio de rotaci�n de la plataforma
        _platformRotation = transform.rotation * Quaternion.Inverse(_lastRotation);

        if (_rbCharacter) MovePlataform();

        // Actualizamos la �ltima posici�n y rotaci�n
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    public void MovePlataform()
    {
        // A�ado el movimiento de la plataforma a la posici�n del jugador
        _rbCharacter.MovePosition(_rbCharacter.position + GetPlatformMovement());

        // Primero, ajusto la posici�n del jugador en funci�n de la rotaci�n de la plataforma
        Vector3 relativePos = _rbCharacter.position - transform.position;
        relativePos = GetPlatformRotation() * relativePos;
        _rbCharacter.MovePosition(transform.position + relativePos);

        // Despu�s, roto al jugador con la plataforma (si se desea que el jugador rote)
        _rbCharacter.MoveRotation(GetPlatformRotation() * _rbCharacter.rotation);
    }

    public Vector3 GetPlatformMovement()
    {
        // Devuelve el movimiento de la plataforma
        return _platformMovement;
    }

    public Quaternion GetPlatformRotation()
    {
        // Devuelve la rotaci�n de la plataforma
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
