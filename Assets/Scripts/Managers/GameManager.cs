using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Character Swap")]
    public Characters character;
    public Characters[] possibleCharacters;
    public KeyCode keyToChangeCharacter;

    //public CameraTracker cam;
    int _playerIndex;

    [Header("Tarea de Mod y Alg")]
    public List<Animal> _animalsList;
    public List<Human> _humanList;

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
        if(character==null&&possibleCharacters.Length>=1)
        {
            character = possibleCharacters[0];
        }
        Swap();
        //cam = GetComponent<CameraTracker>();

    }

    void Update()
    {
        if(Input.GetKeyDown(keyToChangeCharacter))
        {
            for (int i = 0; i < possibleCharacters.Length; i++)
            {
                _playerIndex = i;
                if (possibleCharacters[i] == character)
                {
                    if (_playerIndex >= 0)
                    {
                        _playerIndex++;
                        
                    }
                    else if (_playerIndex> possibleCharacters.Length)
                    {
                        _playerIndex = 0;
                        
                    }

                }
                Swap();
                return;
                
                

            }
        }
    }

    public void Swap()
    {
        character = possibleCharacters[_playerIndex];
        character.GetComponent<Model>().enabled = true;
        for (int i = 0; i < possibleCharacters.Length; i++)
        {
            if(possibleCharacters[i]!=character)
            {
                possibleCharacters[i].GetComponent<Model>().enabled = false;
            }
        }
        //cam.target = character;
        Debug.Log(character.name);
        Debug.Log(_playerIndex);
        
    }

    
}
