using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartCollider : MonoBehaviour
{
    private Minecart _minecart;
    private Characters _player;

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
                _player = other.GetComponent<Characters>();
                other.transform.SetParent(_minecart.transform);
                other.GetComponent<Characters>().enabled = false;
            }
        }
    }

    private void Update()
    {
        if (!_player) return;
        _player.transform.localPosition = new Vector3(0, 1, 0);
    }
}
