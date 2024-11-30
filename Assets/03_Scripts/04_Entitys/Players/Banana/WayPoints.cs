using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : Rewind
{
    [SerializeField] private bool _stop;
    public bool Stop { get { return _stop; } }

    public bool action;

    public bool changeCharacter;

    public override void Save()
    {
        _currentState.Rec(_stop, action, changeCharacter);
    }

    public override void Load()
    {
        if (!_currentState.IsRemember()) return;

        var col = _currentState.Remember();

        _stop = (bool)col.parameters[0];
        action = (bool)col.parameters[1];
        changeCharacter = (bool)col.parameters[2];
    }
}
