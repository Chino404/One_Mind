using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoState 
{
    ParamsMemento _data;

    /// <summary>
    /// Pongo que guardar por parametros
    /// </summary>
    /// <param name="parameter"></param>
    public void Rec(params object[] parameter) //object puede ser cualquier tipo de dato y el params te arma el array con lo que pases
    {
        //Debug.Log("guarde");
        _data = new ParamsMemento(parameter);
    }

    public bool IsRemember() => _data != null;

    public ParamsMemento Remember() => _data;
    
}
