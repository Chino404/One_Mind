using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlueCrystal : MonoBehaviour
{
    [SerializeField] private float _durationPath=10f;
    public GameObject path;

    private void Start()
    {
        path.SetActive(false);
    }

    public void SpawnPath()
    {
        path.SetActive(true);
        StartCoroutine(DesactivatePath());
    }

    IEnumerator DesactivatePath()
    {
        yield return new WaitForSeconds(_durationPath);
        path.SetActive(false);
    }
}
