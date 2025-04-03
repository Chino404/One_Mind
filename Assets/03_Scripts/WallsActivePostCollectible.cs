using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsActivePostCollectible : MonoBehaviour
{
    public GameObject[] walls;
    

    private void Start()
    {
        foreach (var item in walls)
        {
            item.SetActive(false);
        }
    }

    private void Update()
    {
        if(GameManager.instance.isTakeCollBongo&&GameManager.instance.isTakeCollFrank)
        {
            foreach (var item in walls)
            {
                item.SetActive(true);
            }
        }
    }
}
