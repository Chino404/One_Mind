using UnityEngine;

public abstract class Rewind : MonoBehaviour
{
    protected MementoState _currentState;

    public virtual void Awake()
    {
        _currentState = new MementoState();
        GameManager.instance.rewinds.Add(this);
    }

    /// <summary>
    /// Guardar
    /// </summary>
    public abstract void Save();

    /// <summary>
    /// Cargar
    /// </summary>
    public abstract void Load();

    public virtual void OnDestroy()
    {
        GameManager.instance.rewinds.Remove(this);
    }
}
