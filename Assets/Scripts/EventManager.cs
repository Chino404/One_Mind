using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void MyEvents(params object[] parameters);//El "params" antes del array me permite pasarle cosas sueltas y el me arma el array

    static Dictionary<string, MyEvents> _myEvents = new Dictionary<string, MyEvents>();

    public static void Subscribe(string name, MyEvents events)
    {
        if (_myEvents.ContainsKey(name)) //Pregunto si ya existe dicho evento y lo sumo
            _myEvents[name] += events;
        else                           //Sino lo creo
            _myEvents.Add(name, events);
    }

    public static void Unsubscribe(string name, MyEvents events)
    {
        if (_myEvents.ContainsKey(name))
        {
            _myEvents[name] -= events;

            if (_myEvents[name] == null) //Si ya no tengo ningun evento con ese nombre, lo remuevo de mi lista
                _myEvents.Remove(name);
        }
    }

    public static void Trigger(string name, params object[] parameters)
    {
        if (_myEvents.ContainsKey(name)) //Si tengo un evento con ese nombre, lo llamo
            _myEvents[name](parameters);
    }
}
