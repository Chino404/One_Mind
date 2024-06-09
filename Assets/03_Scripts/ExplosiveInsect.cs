using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveInsect : Rewind
{
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

}
