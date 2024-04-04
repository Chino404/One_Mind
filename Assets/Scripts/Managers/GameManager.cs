using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] _players;
    int _playerIndex;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ChangePlayer(_players[0], _players[1]);

    }

    void ChangePlayer(GameObject player1, GameObject player2)
    {
        
    }
}
