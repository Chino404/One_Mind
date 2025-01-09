using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonsManager : Mechanism
{
    [SerializeField] private float _timeToSwitch;

    public GameObject[] pistonsA;

    public GameObject[] pistonsB;

    public override void ActiveMechanism()
    {
        StartCoroutine(SwitchPistons());
    }

    IEnumerator SwitchPistons()
    {
        while(true)
        {
            for (int i = 0; i < pistonsA.Length; i++)
            {
                pistonsA[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < pistonsB.Length; i++)
            {
                pistonsB[i].gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(_timeToSwitch);

            for (int i = 0; i < pistonsA.Length; i++)
            {
                pistonsA[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < pistonsB.Length; i++)
            {
                pistonsB[i].gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(_timeToSwitch);
        }
    }
}
