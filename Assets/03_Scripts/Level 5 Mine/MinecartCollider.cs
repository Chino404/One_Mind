using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartCollider : MonoBehaviour
{
    private Minecart _minecart;
    

    private void Start()
    {
        _minecart = GetComponentInParent<Minecart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            if (other.GetComponent<Characters>())
            {
                _minecart.isWithCharacter = true;
                
                _minecart.player = other.GetComponent<Characters>();
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            if (other.GetComponent<Characters>())
            {
                _minecart.isWithCharacter = false;


            }
        }
    }

    private void Update()
    {
        //if (!_player) return;
        if (_minecart.isWithCharacter&&_minecart.otherMinecart.isWithCharacter)
        {
            _minecart.player.transform.SetParent(_minecart.transform);
            _minecart.player.transform.forward = _minecart.transform.forward;
            //_minecart.player.GetComponent<Characters>().enabled = false;
            _minecart.player.transform.localPosition = new Vector3(0, 1, 0);
        }

        
    }
}
