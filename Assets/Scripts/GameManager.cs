using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
     

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
