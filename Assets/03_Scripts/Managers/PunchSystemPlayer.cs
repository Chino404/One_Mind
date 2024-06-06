using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSystemPlayer : MonoBehaviour
{
    [SerializeField] private getUpPunch _getUpPunch;

    private void Awake()
    {
        _getUpPunch = GetComponentInChildren<getUpPunch>();

        EventManager.Subscribe("GetUpAttack", GetUpAttack);
    }

    private void Start()
    {
        _getUpPunch.gameObject.SetActive(false);
    }


    public void GetUpAttack(params object[] parameters)
    {
        _getUpPunch.Damage = (int)parameters[0];

        var timeActive = (float)parameters[1]; //Provisorio, dsp es el tiempo de la animacion

        _getUpPunch.ForceToUp = (float)parameters[2];

        StartCoroutine(GetUpPunch(timeActive));
    }

    IEnumerator GetUpPunch(float time)
    {
        _getUpPunch.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        _getUpPunch.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("GetUpAttack", GetUpAttack);
    }

}
