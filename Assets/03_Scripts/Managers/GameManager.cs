using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ModelMonkey playerGM;

    [Header("Character Swap")]
    public Characters[] possibleCharacters = new Characters[2];
    //[HideInInspector]public Animator[] animTransCamera = new Animator[2];
    [SerializeField]private bool _controllerMonkey = true;
    public GameObject[] camerasPlayers = new GameObject[2];
    [SerializeField]private Animator _animCamMonkey;
    [SerializeField]private Animator _animCamBanana;
    //public KeyCode keyToChangeCharacter;

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

        //Activo el Mono
        _controllerMonkey = true;
        possibleCharacters[0].GetComponent<ModelMonkey>().enabled = true;

        //Desactivo la banana
        possibleCharacters[1].GetComponent<ModelBanana>().enabled = false;
    }

    private void Update()
    {
        if(_controllerMonkey) possibleCharacters[1].GetComponent<ModelBanana>().enabled = false;
    }


    public void Swap()
    {
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

        _animCamMonkey.SetBool("Active", true);
        camerasPlayers[1].GetComponent<Camera>().enabled = true;
        _animCamBanana.SetBool("Active", true);
        yield return new WaitForSeconds(1);

        possibleCharacters[0].GetComponent<ModelMonkey>().enabled = false;
        camerasPlayers[0].GetComponent<Camera>().enabled = false;

        possibleCharacters[1].GetComponent<BananaGuide>().enabled = false;
        possibleCharacters[1].GetComponent<ModelBanana>().enabled = true;

        Time.timeScale = 1;
        _controllerMonkey = false;

    }

    IEnumerator SwitchCamMonkey()
    {
        _animCamBanana.SetBool("Active", false);
        camerasPlayers[0].GetComponent<Camera>().enabled = true;
        _animCamBanana.SetTrigger("Enter");
        _animCamMonkey.SetBool("Active", false);
        yield return new WaitForSeconds(1);


        possibleCharacters[0].GetComponent<ModelMonkey>().enabled = true;
        camerasPlayers[1].GetComponent<Camera>().enabled = false;


        possibleCharacters[1].GetComponent<BananaGuide>().enabled = true;
        possibleCharacters[1].GetComponent<ModelBanana>().enabled = false;

        Time.timeScale = 1;


        _controllerMonkey = true;

    }
}
