using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [SerializeField] private bool _stop;
    public bool Stop { get { return _stop; } }

    public bool action;

    public bool changeCharacter;
}
