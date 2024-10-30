using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraTransparency : MonoBehaviour
{
    [Header("PARAMETERS")]
    [Space(5)]

    [SerializeField] private LayerMask _maskTransparent;
    [SerializeField, Range(0, 1)] private float _durationFade = 0.3f;
    [HideInInspector] public Transform target;

    private float _dist;
    //private TransparencyMaterial _transMaterial;
    private ITransparency _transMaterial;

    private void Update()
    {
        if (target != null)
        {
            _dist = Vector3.Distance(transform.position, target.position);

            if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out RaycastHit hit, _dist, _maskTransparent))
            {
                if(hit.transform.GetComponent<ITransparency>() != null)
                {
                    //_transMaterial = hit.transform.GetComponent<TransparencyMaterial>();
                    _transMaterial = hit.transform.GetComponent<ITransparency>();

                    _transMaterial.Fade(_durationFade);
                }
            }
            else if (_transMaterial != null)
            {
                _transMaterial.Appear(_durationFade);
                _transMaterial = null;
            }
        }
    }

}
