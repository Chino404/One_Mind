using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleLife : MonoBehaviour
{
    [SerializeField] private float _valueHeal;

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<ICure>();

        if (obj != null)
        {
            obj.Heal(_valueHeal);
        }
    }
}
