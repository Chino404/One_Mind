using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTransparency : MonoBehaviour
{
    Camera _mainCamera;
    Rigidbody _player;

    public float Holesize = 0.1f;
    // Start is called before the first frame update

    private void Awake()
    {
        _mainCamera = Camera.main;
        _player = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColiders = Physics.OverlapSphere(_player.transform.position, 5f);
        foreach (var hitColider in hitColiders)
        {
            float x = 0f;
            if (Vector3.Distance(hitColider.transform.position, _mainCamera.transform.position)<Vector3.Distance(_player.centerOfMass+_player.transform.position, _mainCamera.transform.position))
            {
                x = Holesize;
            }

            try
            {
                Material[] materials = hitColider.transform.GetComponent<Renderer>().materials;
                for (int m = 0; m < materials.Length; m++)
                {
                    materials[m].SetFloat("_step", x);
                }
            }
            catch{ }
        }
            
    }
}
