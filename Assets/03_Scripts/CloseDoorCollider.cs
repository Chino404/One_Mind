using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeCollider
{
    Close,
    Open
}

public class CloseDoorCollider : MonoBehaviour
{
    public TypeCollider type;
    public DualDoor door;
    //private bool _isPlayerPassed;
    //public bool IsPlayerPassed { get { return _isPlayerPassed; } }
    public CloseDoorCollider otherCloseDoorCollider;
    private bool _isCLose;
    private void Update()
    {
        if (type == TypeCollider.Open) return;
        if (_isCLose) return;
        if (door.doorCanClose && otherCloseDoorCollider.door.doorCanClose)
        {

            door.CloseTheDoor();
            _isCLose = true;
            //StartCoroutine(PlayerPass());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            if (type==TypeCollider.Close)
                door.doorCanClose = true;
            else if (type==TypeCollider.Open)
                door.doorCanClose = false;
        }
    }

    
}
