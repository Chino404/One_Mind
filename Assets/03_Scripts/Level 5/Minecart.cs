using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    [SerializeField] private float _speed=5f;
    [SerializeField] private float _changeRailSpeed = 10f;
    //[SerializeField] private float _railDistance=2f;
    [SerializeField] private Transform[] _rails;
    private int _currentRailIndex = 1;
    
    [SerializeField] private Minecart _otherMinecart;
    [HideInInspector]public bool isWithCharacter;
    
    private bool _isMoving;
    

    private void Update()
    {
        if(!isWithCharacter&&!_otherMinecart.isWithCharacter) return;
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!_isMoving && moveInput != 0)
        {
            if (moveInput < 0 && _currentRailIndex > 0) // Mover a la izquierda
            {
                ChangeRail(-1);
            }
            else if (moveInput > 0 && _currentRailIndex < _rails.Length - 1) // Mover a la derecha
            {
                ChangeRail(1);
            }
        }   
    }

    void ChangeRail(int direction)
    {

        _isMoving = true;
        _currentRailIndex += direction;
        Transform targetRail = _rails[_currentRailIndex];

        
        StartCoroutine(MoveToPosition(targetRail));

        //_canMove = false;
        //Invoke("EnableMovement", 0.2f);
    }

    IEnumerator MoveToPosition(Transform targetRail)
    {
        //while (Vector3.Distance(transform.position, targetPos) > 0f)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, targetPos, _changeRailSpeed * Time.deltaTime);
        //    yield return null;
        //}
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(targetRail.position.x, startPos.y, startPos.z);
        
        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        _isMoving = false;
    }

    

    //void EnableMovement()
    //{
    //    _canMove = true;
    //}

}
