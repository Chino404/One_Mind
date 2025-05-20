using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEditor.Progress;

public class PlayCinematic : Connected
{
    [Space(10), SerializeField]private bool _isCinematic;
    public PlayableDirector[] cinematic;

    [Space(10)]
    [SerializeField] private Transform _refPlayer;
    private Characters _character;
    public Characters ChasracterRef { get { return _character; } }

    [Space(5),SerializeField] private Transform _posStartCinematic;
    [SerializeField] private float _maxDistance;

    public override void Awake()
    {
        base.Awake();

        if (_refPlayer) _refPlayer = null;

        if (!_posStartCinematic) Debug.LogError($"Falta la posicion en donde va a arrancar la cinematica para el personaje en: <color=yellow>{gameObject.name}</color>");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isCinematic) return;

        if (other.GetComponent<Characters>())
        {
            _isActive = true;
            _refPlayer = other.transform;
            _character = other.GetComponent<Characters>();

            if (!_connectedObject.IsActive) return;

            CorrectPosition();
            _connectedObject.GetComponent<PlayCinematic>().CorrectPosition();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isCinematic) return;


        if (other.GetComponent<Characters>())
        {
            _isActive = false;
            _refPlayer = null;
            _character = null;
        }
    }

    public void CorrectPosition()
    {
        StartCoroutine(MoveToPlayer());
    }

    IEnumerator MoveToPlayer()
    {
        //Pregunto si esá más lejos a la distancia máxima del punto.
        if(Vector3.Distance(_refPlayer.position, _posStartCinematic.position) <= _maxDistance)
        {
            //Si no está demasiado lejos, lo muevo por código.
            while (Vector3.Distance(_refPlayer.position, _posStartCinematic.position) >= 0.5f)
            {
                Vector3 dir = _posStartCinematic.position - _refPlayer.position;
                dir.Normalize();

                _character.MoveCinematic(dir);
                _character.IsDoingAnimation = true;

                yield return new WaitForFixedUpdate();
            }
        }

        //Si ya llego o está cerca, hago que el movimiento sea cero.
        _character.MoveCinematic(Vector3.zero);

        //Aviso que ya no está en la animación.
        _character.IsDoingAnimation = false;

        //Seteo su posicion y su forward al del transform por las dudas.
        _refPlayer.forward = _posStartCinematic.forward;
        _refPlayer.position = _posStartCinematic.position;

        //Si el otro player sigue en animacion, espero.
        while (_connectedObject.GetComponent<PlayCinematic>().ChasracterRef.IsDoingAnimation)
        {
            yield return null;
        }

        //Cuando ya esten lso dos, ejecuto la cinematica.
        foreach (var item in cinematic)
        {
            if (item) item.Play();
            else Debug.LogWarning($"Falta la cinematica en: <color=yellow>{gameObject.name}</color>");
        }
    }

    private void OnDrawGizmos()
    {
        if (!_posStartCinematic) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_posStartCinematic.position, _maxDistance);
    }

    public override void Save()
    {

    }

    public override void Load()
    {

    }
}
