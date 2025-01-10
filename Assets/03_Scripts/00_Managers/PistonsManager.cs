using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonsManager : Mechanism
{
    [SerializeField] private float _timeToSwitch;

    public Mechanism[] pistonsA;

    public Mechanism[] pistonsB;

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
                //pistonsA[i].gameObject.SetActive(true);
                pistonsA[i].ActiveMechanism();
            }

            for (int i = 0; i < pistonsB.Length; i++)
            {
                //pistonsB[i].gameObject.SetActive(false);
                pistonsB[i].DeactiveMechanism();
            }

            yield return new WaitForSeconds(_timeToSwitch);

            for (int i = 0; i < pistonsA.Length; i++)
            {
                //pistonsA[i].gameObject.SetActive(false);
                pistonsA[i].DeactiveMechanism();
            }

            for (int i = 0; i < pistonsB.Length; i++)
            {
                //pistonsB[i].gameObject.SetActive(true);
                pistonsB[i].ActiveMechanism();
            }

            yield return new WaitForSeconds(_timeToSwitch);
        }
    }
}
