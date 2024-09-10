using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ExplosiveInsect : Rewind, IExplosion
{
    [SerializeField] private WayPoints _point;
    [SerializeField]private ExplosiveThings _particles;

   

    public override void Save()
    {
        _currentState.Rec(gameObject.activeInHierarchy);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();
        gameObject.SetActive((bool)col.parameters[0]);
    }

    public void Execute()
    {
        OldAudioManager.instance.PlaySFX(OldAudioManager.instance.explosion);
        _particles.Explode();
        _point.action = false;
        gameObject.SetActive(false);
    }
}
