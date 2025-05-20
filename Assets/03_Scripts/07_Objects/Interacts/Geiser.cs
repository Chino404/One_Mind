using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeModifyParticleGeiser { overwrite, add, substract}

public class Geiser : MonoBehaviour, IImpulse
{
    [SerializeField, Range(0,50f)] private float _actualForceGeiser = 2;
    [Tooltip("Fuerza del geiser original")]private float _iniForceGeiser;
    [Tooltip("Fuerza del geiser cuando se está planeando")]private float _forceGeiserOnPenguin;

    [SerializeField]private Vector3 _scaleCollider;
    private Vector3 _iniScaleCollider;
    private BoxCollider _myCollider;

    [Space(10), Header("-> Particle")]
    [SerializeField] private bool _isManualSpeed;
    [SerializeField] private float _actualSpeedParticle;
    [SerializeField] private float _actualSpeedLifeTimeParticle;
    private float _iniSpeedParticle;
    private ParticleSystem _myParticle;
    public Rigidbody _characterRb;

    public Rigidbody playerRb;
    

    private void Awake()
    {
        _myParticle = GetComponentInChildren<ParticleSystem>();
        _myCollider = GetComponent<BoxCollider>();

        //_scaleCollider.y = _myCollider.size.y;
        _scaleCollider = _myCollider.size;

        var main = _myParticle.main; //Para poder llegar a sus variables
        _actualSpeedParticle = main.startSpeed.constant;
        _actualSpeedLifeTimeParticle = main.startLifetime.constant;

        _iniScaleCollider = _myCollider.size;
        _iniSpeedParticle = main.startSpeed.constant;
        _iniForceGeiser = _actualForceGeiser;
    }

    private void Start()
    {
        _forceGeiserOnPenguin = _actualForceGeiser / 1.5f;
        _characterRb = GameManager.instance.modelBongo.GetComponent<Rigidbody>();
        //Debug.Log(_characterRb.velocity);
    }

    

    public void StartScale(float startScaleY, float startSpeedParticle, float starSpeedLifeTime)
    {
        _scaleCollider.y = startScaleY;
        _myCollider.size = _scaleCollider;

        _actualSpeedParticle = startSpeedParticle;
        _actualSpeedLifeTimeParticle = starSpeedLifeTime;

        ModifyParticle(startSpeedParticle, starSpeedLifeTime, TypeModifyParticleGeiser.overwrite);
    }

    /// <summary>
    /// Modifico la escala del Geiser en el eje Y-
    /// </summary>
    /// <param name="addScaleY"></param>
    /// <param name="newSpeedParticle"></param>
    public void ModifyScaleYGeiser(float addScaleY, float newSpeedParticle, float newSpeedLifeTimeParticle, TypeModifyParticleGeiser type)
    {
        //_scaleCollider.y = newScaleY;
        _myCollider.size += new Vector3(0, addScaleY, 0);

        if (_isManualSpeed) return;

        ModifyParticle(newSpeedParticle, newSpeedLifeTimeParticle, type);

        //Para automatizar las particulas
        //var porcentaje = newScaleY / (_iniScaleCollider.y * 1.5f);
    }

    public void RevertChange(float subtractScaleY, float speedParticle, float speedLifeTime, TypeModifyParticleGeiser type)
    {
        //_myCollider.size = _iniScaleCollider;
        _myCollider.size -= new Vector3(0, subtractScaleY, 0);

        if(_myCollider.size.y <= 0) _myCollider.size = new Vector3(_myCollider.size.x, 0, _myCollider.size.z);

        if (type == TypeModifyParticleGeiser.add) type = TypeModifyParticleGeiser.substract;
        else if (type == TypeModifyParticleGeiser.substract) type = TypeModifyParticleGeiser.add;

        ModifyParticle(speedParticle, speedLifeTime, type);
    }

    private void ModifyParticle(float valueSpeed, float valuSpeedLifeTime, TypeModifyParticleGeiser type)
    {
        if (_isManualSpeed) return;

        var main = _myParticle.main; //Para poder llegar a sus variables

        if(type == TypeModifyParticleGeiser.overwrite)
        {
            _actualSpeedParticle = valueSpeed;
            _actualSpeedLifeTimeParticle = valuSpeedLifeTime;
        }
        else if (type == TypeModifyParticleGeiser.add)
        {
            _actualSpeedParticle += valueSpeed;
            _actualSpeedLifeTimeParticle += valuSpeedLifeTime;
        }
        else
        {
            _actualSpeedParticle -= valueSpeed;
            _actualSpeedLifeTimeParticle -= valuSpeedLifeTime;
        }

        main.startSpeed = _actualSpeedParticle;
        main.startLifetime = _actualSpeedLifeTimeParticle;
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();

        playerRb = rb != null ? rb : null;
    }

    private void OnTriggerStay(Collider other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();

        //playerRb = rb != null ? rb : null;

        var bongo = other.gameObject.GetComponent<ModelBongo>();

        if (bongo != null && bongo.IsFly) _actualForceGeiser = _forceGeiserOnPenguin;
        else _actualForceGeiser = _iniForceGeiser;
        

        Action(rb);
    }

    private void OnTriggerExit(Collider other)
    {
        //var rb = other.gameObject.GetComponent<Rigidbody>();
        
        if (other==playerRb)
        {
            playerRb.velocity = _characterRb.velocity;
            playerRb = null;

        }
    }

    private void OnDisable()
    {
        if (!playerRb) return;

        playerRb.GetComponent<Characters>().IsImpulse = false;
        playerRb.velocity = _characterRb.velocity;
        playerRb = null;
        
        Debug.Log($"characterRb" + _characterRb.velocity);
        Debug.Log(playerRb.velocity);
    }

    public void Action(Rigidbody rb)
    {
        if (rb != null)
        {
            //rb.AddForce(transform.up * _actualForceGeiser, ForceMode.Impulse);

            // Aplica la fuerza en la dirección del transform.up del géiser
           
            Vector3 geiserForce = transform.up * _actualForceGeiser;

            rb.velocity = geiserForce;
            

        }
    }
}
