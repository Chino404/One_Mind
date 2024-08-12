using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoDeBananaBot
{
    EnPosicion,
    RegresandoAPosicion,
    AtaqueCargado
}

public enum EstadoDeBongo
{
    Normal,
    Escalando,
    Golpeando,
    Minigun,
    CargandoAtaqueElectrico
}

public abstract class Characters : Entity
{
    //Referencias
    protected Rigidbody _rbCharacter;
    protected Animator _animatorCharacter;

    
}
