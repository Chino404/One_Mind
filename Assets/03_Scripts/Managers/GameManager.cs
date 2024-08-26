using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterTarget
{
    Bongo,
    Frank
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Rewind> rewinds;

    [Header("Characters")]
    public Transform frank;
    public Transform bongo;
    public List<PointsForTheCamera> points = new ();

    private bool _controllerMonkey = true; //Si se puede usar al mono
    public bool ContollerMonkey {  get { return _controllerMonkey; } }


    public List<Enemy> enemies = new();

    [Range(0f, 4f)]
    public float weightSeparation, weightAlignment, weightSeek;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log("Start GAMEMANAGER");

        //Activo los controles del Mono
        _controllerMonkey = true;

        foreach (var item in points)
        {
            if(item.player == null)
            {
                if (item.characterTarget == CharacterTarget.Bongo) item.player = bongo;
                else if (item.characterTarget == CharacterTarget.Frank) item.player = frank;
            }
        }

        foreach (var item in rewinds)
        {
            item.Save();
            
        }
    }

    public void RemoveAll()
    {
        foreach (var item in rewinds)
        {
            rewinds.Remove(item);
        }
    }
    
}
