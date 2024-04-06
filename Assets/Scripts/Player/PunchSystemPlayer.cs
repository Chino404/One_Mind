using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSystemPlayer : MonoBehaviour
{
    private Punch noramlPunch;

    private void Awake()
    {
        noramlPunch = GetComponentInChildren<Punch>();
    }

    private void Start()
    {
        EventManager.Subscribe("NormalAttack", NoramlAttack);
        noramlPunch.gameObject.SetActive(false);
    }

    public void NoramlAttack(params object[] parameters)
    {
        noramlPunch._damagePunch = (int)parameters[0];

        var timeActive = (float)parameters[1]; //Provisorio, dsp es el tiempo de la animacion

        StartCoroutine(Punch(timeActive));
    }

    IEnumerator Punch(float time)
    {
        noramlPunch.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        noramlPunch.gameObject.SetActive(false);
    }
}
