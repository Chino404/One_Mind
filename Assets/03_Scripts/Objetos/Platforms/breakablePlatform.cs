using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class breakablePlatform : MonoBehaviour
{
    [SerializeField, Tooltip("Tiempo que tarda para ROMPERSE"), Range(0,5f)] private float _timeToBreaking = 3;
    [SerializeField, Tooltip("Tiempo que tarda para RECOMPONERSE"), Range(0,5f)] private float _timeToRecover = 3;
    private bool _isBreaking;

    //Provisorio hasta tener animaciones
    private Collider _myCollider;
    private MeshRenderer _myRenderer;

    private void Awake()
    {
        _myCollider = GetComponent<Collider>();
        _myRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Characters>())
        {
            if(!_isBreaking) StartCoroutine(Breaking());
        }
    }

    IEnumerator Breaking()
    {
        _isBreaking = true;
        AudioManager.instance.Play(SoundId.IceBreak);

        yield return new WaitForSeconds(_timeToBreaking);

        _myCollider.enabled = false;
        _myRenderer.enabled = false;

        yield return new WaitForSeconds(_timeToRecover);

        _myCollider.enabled = true;
        _myRenderer.enabled = true;

        _isBreaking = false;
    }

}
