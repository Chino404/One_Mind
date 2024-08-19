using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JugadorAsignado
{
    Bongo,
    BananaBot
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Rewind> rewinds;

    [Header("Character Swap")]
    public Characters[] players = new Characters[2];
    public Transform assignedPlayer;
    public PointsForTheCamera points;

    private bool _inChange = false;
    private bool _controllerMonkey = true; //Si se puede usar al mono
    public bool ContollerMonkey {  get { return _controllerMonkey; } }
    private float _duration;

    //public CameraTracker cam;
    //public CameraTarget cam;


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

        assignedPlayer = players[0].transform;
        if(points.player == null) points.player = assignedPlayer;

        //Desactivo la banana
        if (players[1] == null) Debug.LogError("NO SE ASIGNO LA BANANA");
        players[1].GetComponent<ModelBanana>().enabled = false;

        foreach (var item in rewinds)
        {
            item.Save();
        }

    }

    private void Update()
    {

        if (_controllerMonkey && players[1] != null) players[1].GetComponent<ModelBanana>().enabled = false;

        //if(Input.GetKeyDown(KeyCode.Q)) Swap();


    }

    public void RemoveAll()
    {
        foreach (var item in rewinds)
        {
            rewinds.Remove(item);
        }
    }

    public void Swap()
    {
        if(_controllerMonkey)
        {
            SwitchCamBanana();
        }
        else
        {
            SwitchCamMonkey();
        }

        points.player = assignedPlayer;

    }


    void SwitchCamBanana()
    {
        _inChange = true;
        _controllerMonkey = false;

        assignedPlayer = players[1].transform;
        //cam.Target = players[1].transform;

        players[1].GetComponent<BananaGuide>().enabled = false;
        players[1].GetComponent<ModelBanana>().enabled = true;
        _inChange = false;

    }

    void SwitchCamMonkey()
    {
        players[1].GetComponent<ModelBanana>().enabled = false;
        _inChange = true;
        _controllerMonkey = true;

        assignedPlayer = players[0].transform;
        //cam.Target = players[0].transform;

        players[1].GetComponent<BananaGuide>().enabled = true;
        _inChange = false;
    }

    
}
