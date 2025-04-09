using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorCollider : MonoBehaviour
{
    public DualDoor door;
    private bool _isPlayerPassed;
    public bool IsPlayerPassed { get { return _isPlayerPassed; } }
    public CloseDoorCollider otherCloseDoorCollider;

    private void Update()
    {
        if (_isPlayerPassed && otherCloseDoorCollider.IsPlayerPassed)
        {
            door.CloseTheDoor();
            StartCoroutine(PlayerPass());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
            _isPlayerPassed = true;
    }

    IEnumerator PlayerPass()
    {
        yield return new WaitForSeconds(2f);
        _isPlayerPassed = false;
    }
}
