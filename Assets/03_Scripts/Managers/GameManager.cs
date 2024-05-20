using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ModelMonkey playerGM;

    [Header("Character Swap")]
    public Characters[] possibleCharacters = new Characters[2];
    public GameObject[] camerasPlayers = new GameObject[2];
    private bool _inChange = false;
    private bool _controllerMonkey = true;
    public bool ContollerMonkey {  get { return _controllerMonkey; } }
    private Animator _animCamMonkey;
    private Animator _animCamBanana;
    private Animator _animSeparationCameras;
    public Animator AnimSeparationCameras { set { _animSeparationCameras = value; } }
    private float _duration;

    //public CameraTracker cam;
    int _playerIndex;

    public List<Enemy> enemies = new();

    [Range(0f, 4f)]
    public float weightSeparation, weightAlignment;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

    private void Start()
    {
        _animCamMonkey = camerasPlayers[0].GetComponent<Animator>();
        _animCamBanana = camerasPlayers[1].GetComponent<Animator>();

        AnimatorStateInfo stateInfo = _animSeparationCameras.GetCurrentAnimatorStateInfo(0);
        _duration = stateInfo.length;

        //Activo los controles del Mono
        _controllerMonkey = true;

        //Desactivo la banana
        possibleCharacters[1].GetComponent<ModelBanana>().enabled = false;
    }

    private void Update()
    {
        if(_controllerMonkey) possibleCharacters[1].GetComponent<ModelBanana>().enabled = false;
    }


    public void Swap()
    {
        if(_inChange) return;
        if(_controllerMonkey)
        {
            StartCoroutine(SwitchCamBanana());
        }
        else
        {
            StartCoroutine(SwitchCamMonkey());
        }
    }


    IEnumerator SwitchCamBanana()
    {
        Time.timeScale = 0;
        _inChange = true;
        _animCamMonkey.SetTrigger("Exit");
        camerasPlayers[1].GetComponent<Camera>().enabled = true;
        _animCamBanana.SetTrigger("Enter");
        _animSeparationCameras.SetTrigger("Enter");
        yield return new WaitForSecondsRealtime(1);

        _controllerMonkey = false;
        camerasPlayers[0].GetComponent<Camera>().enabled = false;

        possibleCharacters[1].GetComponent<BananaGuide>().enabled = false;
        possibleCharacters[1].GetComponent<ModelBanana>().enabled = true;

        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(_duration);
        _inChange = false;

    }

    IEnumerator SwitchCamMonkey()
    {
        Time.timeScale = 0;
        _inChange = true;
        _animCamBanana.SetTrigger("Exit");
        camerasPlayers[0].GetComponent<Camera>().enabled = true;
        _animCamMonkey.SetTrigger("Enter");
        _animSeparationCameras.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(1);

        _controllerMonkey = true;
        camerasPlayers[1].GetComponent<Camera>().enabled = false;


        possibleCharacters[1].GetComponent<BananaGuide>().enabled = true;
        possibleCharacters[1].GetComponent<ModelBanana>().enabled = false;

        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(_duration);
        _inChange = false;
    }
}
