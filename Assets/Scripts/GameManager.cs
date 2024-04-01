using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Animal> _animalsList;
    public List<Human> _humanList;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    if(_animalsList.Count > 0)
        //    {
        //        for (int i = 0; i < _animalsList.Count; i++)
        //        {
        //            _animalsList[i].Action();
        //        }
        //    }
        //}
    }
}
