using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NormalPunch : Hits
{
    [SerializeField] float _damage;
    [SerializeField] Entidad entity;

    private Collider _myCollider;
    private bool _action;

    public MinigunBar minigunBar;

    private void Awake()
    {
        _myCollider = GetComponent<Collider>();
        
    }


    private void Update()
    {
        if (_myCollider.enabled)
        {
            ExecuteAudio();
            _action = true;
        }
        else _action = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamageEntity(_damage, _entity.transform.position);
            minigunBar.PunchEnemy();

        }

        else if (other.gameObject.layer == 18)
            minigunBar.PunchThings();
    }

    private void ExecuteAudio()
    {
        if (_action) return;

        if (entity == Entidad.Frog) AudioManager.instance.PlaySFX(AudioManager.instance.hitFrog);
    }

    public enum Entidad
    {
        Frog,
        Monkey
    }

}
