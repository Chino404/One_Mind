using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSystemPlayer : MonoBehaviour
{
    private NormalPunch _normalPunch;
    [SerializeField] private SpinPunch _spinPunch;
    [SerializeField] private getUpPunch _getUpPunch;

    private void Awake()
    {
        _normalPunch = GetComponentInChildren<NormalPunch>();
        _spinPunch = GetComponentInChildren<SpinPunch>();
        _getUpPunch = GetComponentInChildren<getUpPunch>();

        EventManager.Subscribe("NormalAttack", NoramlAttack);
        EventManager.Subscribe("SpinAttack", SpinAttack);
        EventManager.Subscribe("GetUpAttack", GetUpAttack);
    }

    private void Start()
    {
        _normalPunch.gameObject.SetActive(false);
        _spinPunch.gameObject.SetActive(false);
        _getUpPunch.gameObject.SetActive(false);
    }

    public void NoramlAttack(params object[] parameters)
    {
        _normalPunch.Damage = (int)parameters[0];

        var timeActive = (float)parameters[1]; //Provisorio, dsp es el tiempo de la animacion

        StartCoroutine(Punch(timeActive));
    }

    IEnumerator Punch(float time)
    {
        _normalPunch.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        _normalPunch.gameObject.SetActive(false);
    }

    public void SpinAttack(params object[] parameters)
    {
        _spinPunch.Damage = (int)parameters[0];

        var timeActive = (float)parameters[1]; //Provisorio, dsp es el tiempo de la animacion

        StartCoroutine(SpinPunch(timeActive));
    }

    IEnumerator SpinPunch(float time)
    {
        _spinPunch.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        _spinPunch.gameObject.SetActive(false);
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
        EventManager.Unsubscribe("NormalAttack", NoramlAttack);
        EventManager.Unsubscribe("SpinAttack", SpinAttack);
        EventManager.Unsubscribe("GetUpAttack", GetUpAttack);
    }

}
