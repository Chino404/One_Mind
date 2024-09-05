using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public ModelBongo bongo;
    public ModelFrank frank;
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

        

        foreach (var item in rewinds)
        {
            item.Save();
            
        }

        foreach (var item in points)
        {
            if (item == null) continue;

            if (item.player == null)
            {
                if (item.characterTarget == CharacterTarget.Bongo) item.player = bongo.transform;
                else if (item.characterTarget == CharacterTarget.Frank) item.player = frank.transform;
            }
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
