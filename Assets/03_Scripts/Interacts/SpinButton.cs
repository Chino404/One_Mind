using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinButton : MonoBehaviour, IPress
{
    [SerializeField] private GameObject[] _floors;

    private bool _isPressing;

    

    public void Pressed()
    {
        //if (_isPressing) return;
        //StartCoroutine(MoveFloors());
        Vector3 pos = default;


        for (int i = 0; i < _floors.Length; i++)
        {

            //Vector3 pos = _floors[i].transform.position;

            Vector3 newPos = _floors[i + 1].transform.position;

            if (i == _floors.Length)
                newPos = _floors[0].transform.position;

            pos = newPos;
        }
        for (int i = 0; i < _floors.Length; i++)
        {
            _floors[i].transform.position = pos;
        }
    }
    IEnumerator MoveFloors()
    {
        _isPressing = true;
        yield return new WaitForSeconds(0.5f);

        
        _isPressing = false;

    }
    public void Depressed()
    {
        
    }

    
}
