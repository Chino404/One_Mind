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
    
    public Minecart otherMinecart;
    [HideInInspector]public bool isWithCharacter;
    
    private bool _isMoving;

    private MinecartCollider _triggerCollider;
    [HideInInspector]public Characters player;

    private void Start()
    {
        _triggerCollider = GetComponentInChildren<MinecartCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 18)//si colisiona con el collider del fin del riel
        {
            Debug.Log("llego al final del riel");
            StartCoroutine(StopMoving());
            this.enabled = false;


        }
    }

    private void Update()
    {
        if(!isWithCharacter||!otherMinecart.isWithCharacter) return;
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

    IEnumerator StopMoving()
    {
        
        isWithCharacter = false;
        //otherMinecart.isWithCharacter = false;
        _triggerCollider.enabled = false;
        yield return null;
        
        
        //player.GetComponent<Characters>().enabled = true;
        //otherMinecart.player.GetComponent<Characters>().enabled = true;
        
    }

    //void EnableMovement()
    //{
    //    _canMove = true;
    //}

}
