using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSystemPlayer : MonoBehaviour
{
    private Punch _normalPunch;
    [SerializeField]private Punch _spinPunch;

    private void Awake()
    {
        _normalPunch = GetComponentInChildren<Punch>();
    }

    private void Start()
    {
        EventManager.Subscribe("NormalAttack", NoramlAttack);
        EventManager.Subscribe("SpinAttack", SpinAttack);

        _normalPunch.gameObject.SetActive(false);
        _spinPunch.gameObject.SetActive(false);
    }

    public void NoramlAttack(params object[] parameters)
    {
        _normalPunch._damagePunch = (int)parameters[0];

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
        _spinPunch._damagePunch = (int)parameters[0];

        var timeActive = (float)parameters[1]; //Provisorio, dsp es el tiempo de la animacion

        StartCoroutine(SpinPunch(timeActive));
    }

    IEnumerator SpinPunch(float time)
    {
        _spinPunch.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        _spinPunch.gameObject.SetActive(false);
    }

}
