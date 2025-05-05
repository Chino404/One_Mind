using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baculo : Weapons
{
    [SerializeField] private float _activationRange = 5f; 
    [SerializeField] private LayerMask _obstructionLayers; 
    public override void Attack()
    {
        base.Attack();
        
        Debug.Log("ataco con el baculo");
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, _activationRange);

        foreach (Collider obj in objectsInRange)
        {
            BuildableObject activatable = obj.GetComponent<BuildableObject>();

            if (activatable != null && HasLineOfSight(obj.transform))
            {
                activatable.Activate();
                break; // Activamos solo un objeto
            }
        }
    }

    private bool HasLineOfSight(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        if (Physics.Raycast(transform.position, direction, distance, _obstructionLayers))
        {
            return false; // Hay un obstáculo en el camino
        }

        return true; // No hay obstrucciones
    }
}

