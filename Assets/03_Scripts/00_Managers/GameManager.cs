using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterTarget
{
    Bongo,
    Frank
}
[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Rewind> rewinds;

    [Header("Characters")]
    [HideInInspector]public ModelBongo bongo;
    public CameraTracker bongoCamera;

    [HideInInspector]public ModelFrank frank;
    public CameraTracker frankCamera;

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

        Debug.Log("AWAKE GAMEMANAGER");
    }

    private void Start()
    {

        //Activo los controles del Mono
        //_controllerMonkey = true;

        if (!bongo) Debug.LogError("Falta la referencia de BONGO");
        if (!frank) Debug.LogError("Falta la referencia de FRANK");
        

        foreach (var item in rewinds)
        {
            item.Save();
            
        }

        //foreach (var item in points)
        //{
        //    if (item == null) continue;

        //    if (item.player == null)
        //    {
        //        if (item.characterTarget == CharacterTarget.Bongo) item.player = bongo.transform;
        //        else if (item.characterTarget == CharacterTarget.Frank) item.player = frank.transform;
        //    }
        //}
    }

    public void RemoveAll()
    {
        foreach (var item in rewinds)
        {
            rewinds.Remove(item);
        }
    }
    
}
