using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    [SerializeField] private float _speed=5f;
    [SerializeField] private float _railDistance=2f;
    [SerializeField] private float _changeRailSpeed = 10f;
    [SerializeField] private Minecart _otherMinecart;
    private int _currentRail = 1;
    [HideInInspector]public bool isWithCharacter;
    
    //private bool _canMove=true;
    bool _isMoving;
    bool _inputLocked;

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        float moveInput = Input.GetAxisRaw("Horizontal");

        
        
        if (moveInput < 0 && _currentRail >0)
        {
            if (!_isMoving)
            {
                _isMoving = true;
                ChangeRail(-1);
            }
                
        }

        if (moveInput > 0 && _currentRail < 2)
        {
            if (!_isMoving)
            {
                _isMoving = true;
                ChangeRail(1);

            }
        }
        Debug.Log(_currentRail);
        Debug.Log("move input " + moveInput);
        
    }

    void ChangeRail(int direction)
    {
        

        _currentRail += direction;
        float targetX = (_currentRail - 1) * _railDistance;

        StopAllCoroutines();
        StartCoroutine(MoveToPosition(targetX));

        //_canMove = false;
        //Invoke("EnableMovement", 0.2f);
    }

    IEnumerator MoveToPosition(float targetX)
    {
        //while (Vector3.Distance(transform.position, targetPos) > 0f)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, targetPos, _changeRailSpeed * Time.deltaTime);
        //    yield return null;
        //}
        Vector3 startPos = transform.position;
        float targetPos = targetX;
        float startX = startPos.x;
        float elapsedTime = 0f;
        float duration = 0.2f; // Duración del cambio de riel

        while (elapsedTime < duration)
        {
            // Mover en el eje X de manera proporcional
            float newX = Mathf.Lerp(startX, targetPos, elapsedTime / duration);
            transform.position = new Vector3(newX, startPos.y, startPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(targetPos,startPos.y,startPos.z);
        _isMoving = false;
    }

    //void EnableMovement()
    //{
    //    _canMove = true;
    //}

}
