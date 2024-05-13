using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ModelMonkey playerGM;

    //[Header("Character Swap")]
    //public Characters actualCharacter;
    public Characters[] possibleCharacters = new Characters[2];
    [SerializeField]private bool _controllerMonkey = true;
    public GameObject[] camerasPlayers = new GameObject[2];
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
            possibleCharacters[0].GetComponent<ModelMonkey>().enabled = false;
            camerasPlayers[0].gameObject.SetActive(false);


            possibleCharacters[1].GetComponent<BananaGuide>().enabled = false;
            possibleCharacters[1].GetComponent<ModelBanana>().enabled = true;
            camerasPlayers[1].gameObject.SetActive(true);

            _controllerMonkey = false;
        }
        else
        {
            possibleCharacters[0].GetComponent<ModelMonkey>().enabled = true;
            camerasPlayers[0].gameObject.SetActive(true);


            possibleCharacters[1].GetComponent<BananaGuide>().enabled = true;
            possibleCharacters[1].GetComponent<ModelBanana>().enabled = false;
            camerasPlayers[1].gameObject.SetActive(false);

            _controllerMonkey = true;
        }
    }

    
}
